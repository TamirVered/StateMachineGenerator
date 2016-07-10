using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using smg.ArgumentParsing;

namespace smg
{
    class StateMachineGenerator
    {
        /// <summary>
        /// Contains the help message to be presented.
        /// </summary>
        private static readonly string HELP = "Made by Tamir Vered 2016" + Environment.NewLine
            + Environment.NewLine
            + "This utility allows generation of safe state machine wrappers according to attributed compiled class with many possible usages such as: " + Environment.NewLine
            + " - Safe factories." + Environment.NewLine
            + " - Unit testing tools." + Environment.NewLine
            + " - Safe stateful object handling." + Environment.NewLine
            + Environment.NewLine
            + "Usage:" + Environment.NewLine
            + "smg.exe source_assembly source_type_full_name destination_folder [/A | /D]" + Environment.NewLine
            + Environment.NewLine
            + "  /A | /D     Set output to compiled DLL instead of source files." + Environment.NewLine
            + Environment.NewLine
            + "For more information visit the project's github on https://github.com/TamirVered/StateMachineGenerator" + Environment.NewLine;

        /// <summary>
        /// Used as entry point.
        /// </summary>
        /// <param name="args">Arguments which will be used for the application to determine how to work (see: <see cref="HELP"/>).</param>
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

            Assembly assembly = SafelyGetAssembly(argsData.SourceAssemblyPath);
            if (assembly == null)
            {
                return;
            }

            Type type = SafelyGetType(assembly, argsData.StateClassName);
            if (type == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(argsData.OutputPath) ||
                !Directory.Exists(argsData.OutputPath))
            {
                Console.WriteLine("Output directory is invalid or does not exist. use /h for help.");
                DisplayHelp();
                return;
            }
        }

        /// <summary>
        /// Gets a <see cref="Type"/> from a given <see cref="Assembly"/>, returns null and print message if fails.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> from which the <see cref="Type"/> will be retrieved.</param>
        /// <param name="typeName">The full name of the <see cref="Type"/> to be retrieved.</param>
        /// <returns>A <see cref="Type"/> retrieved from the provided <see cref="Assembly"/> (<c>null</c> if failed).</returns>
        private static Type SafelyGetType(Assembly assembly, string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                Console.WriteLine("Source type name is invalid. use /h for help.");
                DisplayHelp();
                return null;
            }
            Type type;
            try
            {
                type = assembly.GetType(typeName);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Could not load type from the given assembly. use /h for help."
                                  + Environment.NewLine + "Inner error message: " + Environment.NewLine + exception.Message);
                DisplayHelp();
                return null;
            }
            if (type == null)
            {
                Console.WriteLine($"No type with full name {type.FullName} was found in the given assembly.");
                DisplayHelp();
                return null;
            }
            return type;
        }

        /// <summary>
        /// Loads an <see cref="Assembly"/> from a given path, returns null and print message if fails.
        /// </summary>
        /// <param name="path">The path from which the <see cref="Assembly"/> will be loaded.</param>
        /// <returns>An <see cref="Assembly"/> loaded from the provided path (<c>null</c> if failed).</returns>
        private static Assembly SafelyGetAssembly(string path)
        {
            if (string.IsNullOrWhiteSpace(path) ||
                !File.Exists(path))
            {
                Console.WriteLine("Source assembly path is invalid. use /h for help.");
                DisplayHelp();
                return null;
            }
            Assembly assembly;
            try
            {
                assembly = Assembly.LoadFrom(path);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Could not load assembly from the given path. use /h for help."
                                  + Environment.NewLine + "Inner error message: " + Environment.NewLine + exception.Message);
                DisplayHelp();
                return null;
            }
            return assembly;
        }

        private static void DisplayHelp()
        {
            Console.WriteLine(HELP);
        }
    }
}
