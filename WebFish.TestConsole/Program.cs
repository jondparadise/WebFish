using System;
using WebFish.Engine;
using WebFish.Engine.Helpers;

namespace WebFish.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebFishEngine engine = new WebFishEngine())
            {
                while (engine.IsInitalized())
                {
                    var cmd = Console.ReadLine();
                    if (cmd.Equals("quit", StringComparison.OrdinalIgnoreCase))
                    {
                        engine.Quit();
                        break;
                    }
                    engine.SendCommand(cmd);
                }
            }
            

            //Console.WriteLine(FENHelper.FENStringFromTestBoard());

        }
    }
}
