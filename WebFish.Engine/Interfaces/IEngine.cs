using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFish.Common;

namespace WebFish.Engine.Interfaces
{
    public interface IEngine
    {
        void Initalize();
        void BeginGame();
        void SendCommand(string command);
        void Quit();

        bool IsInitalized();

        //CalculationResult GetResult(CalculationRequest request);
        void StartCalculation(CalculationRequest request);
    }
}
