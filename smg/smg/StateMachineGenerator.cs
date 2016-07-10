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
        private static readonly string HELP = "Made by Tamir Vered 2016" + Environment.NewLine
            + Environment.NewLine
            + "This utility allows generation of safe state machine wrappers according to attributed compiled class with many possible usages such as: " + Environment.NewLine
            + " - Safe factories." + Environment.NewLine
            + " - Unit testing tools." + Environment.NewLine
            + " - Safe stateful object handling." + Environment.NewLine
            + Environment.NewLine
            + "Usage:" + Environment.NewLine
            + "smg.exe source_assembly source_type_name destination_folder [/A | /D]" + Environment.NewLine
            + Environment.NewLine
            + "  /A | /D     Set output to compiled DLL instead of source files." + Environment.NewLine
            + Environment.NewLine
            + "For more information visit the project's github on https://github.com/TamirVered/StateMachineGenerator" + Environment.NewLine;


        static void Main(string[] args)
        {
            ArgsData argsData;
            if (!ArgsParser.TryParse(args, out argsData))
            {
                DisplayHelp();
                return;
            }
            if (argsData.Help)
            {
                DisplayHelp();
                return;
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine(HELP);
        }
    }
}
