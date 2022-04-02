using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFish.Engine.Interfaces;
using WebFish.Common;
using System.Text.RegularExpressions;

namespace WebFish.Engine
{
    public class WebFishEngine : IDisposable, IEngine
    {
        const string ENGINE_EXECUTABLE_FILENAME = "ceng.exe";
        public delegate void CalculationCompletedDelegate(CalculationResult result);
        public event CalculationCompletedDelegate CalculationCompleted;

        Process _engineProcess;
        StreamWriter _processWriter;
        private bool _initalized;
        private bool _calculating = false;
        private CalculationRequest _currentRequest = null;
        private Stopwatch _watch = new Stopwatch();
        
        public bool Initalized { get { return this._initalized; } }
        
        
        public WebFishEngine() {
            this.Initalize();
        }

        public void Dispose()
        {
            if (!this._engineProcess.HasExited)
            {
                this._engineProcess.Kill();
            }

            File.Delete(ENGINE_EXECUTABLE_FILENAME);
        }

        public void Initalize()
        {
            if (!this.Initalized)
            {
                if (File.Exists(ENGINE_EXECUTABLE_FILENAME))
                {
                    File.Delete(ENGINE_EXECUTABLE_FILENAME);
                }

                var resourceNames = typeof(WebFishEngine).Assembly.GetManifestResourceNames().ToList();
                var stockFishResourceName = resourceNames.FirstOrDefault(item => item.Contains("stockfish", StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrWhiteSpace(stockFishResourceName))
                {
                    var stockFishStream = typeof(WebFishEngine).Assembly.GetManifestResourceStream(stockFishResourceName);
                    byte[] data;
                    using (BinaryReader bReader = new BinaryReader(stockFishStream))
                    {
                        data = bReader.ReadBytes((int)stockFishStream.Length);
                        using (FileStream fStream = new FileStream(ENGINE_EXECUTABLE_FILENAME, FileMode.Create))
                        {
                            fStream.Write(data, 0, data.Length);
                        }

                        ProcessStartInfo pStartInfo = new ProcessStartInfo()
                        {
                            FileName = ENGINE_EXECUTABLE_FILENAME,
                            RedirectStandardInput = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false
                        };
                        
                        if ((this._engineProcess = Process.Start(pStartInfo)) != null)
                        {
                            this._engineProcess.OutputDataReceived += _engineProcess_OutputDataReceived;
                            this._engineProcess.Exited += _engineProcess_Exited;
                            this._processWriter = this._engineProcess.StandardInput;
                            Console.WriteLine("Started: " + this._engineProcess.StartTime.ToString());
                            this._engineProcess.BeginOutputReadLine();
                            this.Configure();
                        }
                    }
                }

                this._initalized = true;
            }
        }

        public bool IsInitalized()
        {
            return this._initalized;
        }

        private void _engineProcess_Exited(object sender, EventArgs e)
        {
            this._initalized = false;
        }

        private void _engineProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var command = e.Data;
            Console.WriteLine(command);
            if (!string.IsNullOrWhiteSpace(command))
            {
                if (command.EndsWith("OK", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Is command?: {command.Replace("ok", string.Empty)}");
                }

                Regex calculationCompleteRegex = new Regex(@"bestmove\s(.+)\sponder\s(.+)");
                if (calculationCompleteRegex.IsMatch(command))
                {
                    var matches = calculationCompleteRegex.Matches(command);
                    string move = string.Empty;
                    if(matches != null && matches.Count > 0)
                    {
                        move = matches.First().Groups[1].Value;
                        //var ponder = matches.First().Groups[2].Value;
                    }

                    this._watch.Stop();

                    this.CalculationCompleted?.Invoke(new CalculationResult()
                    {
                        GameId = this._currentRequest.GameId,
                        MoveId = this._currentRequest.MoveId,
                        MoveFEN = move,
                        RuntimeSeconds = ((double)this._watch.ElapsedMilliseconds/(double)1000)
                    });
                    
                    this._currentRequest = null;
                    this._calculating = false;
                }
            }
        }

        public void BeginGame()
        {
            
        }

        private void Configure()
        {
            this.Send("isready");
        }

        private void Send(string command)
        {
            this._processWriter.WriteLine(command + Environment.NewLine);
        }

        public void SendCommand(string command)
        {
            this.Send(command);
        }

        public void Quit()
        {
            Send("quit");
        }

        public void StartCalculation(CalculationRequest request)
        {
            if (!this._calculating)
            {
                this._watch.Restart();
                this._calculating = true;
                this._currentRequest = request;
                this.SendCommand($"position fen {request.FEN}");
                this.SendCommand($"go movetime 3000");
            }
            else
            {
                throw new ApplicationException("Request to calculate move with active request");
            }
            
        }
    }
}
