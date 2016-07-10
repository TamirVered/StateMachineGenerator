using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using smg.ArgumentParsing;

namespace smg
{
    class StateMachineGenerator
    {
        static void Main(string[] args)
        {
            ArgsData argsData;
            if (!ArgsParser.TryParse(args, out argsData))
            {
                DisplayHelp();
                return;
            }
        }

        private static void DisplayHelp()
        {
            throw new NotImplementedException();
        }
    }
}
