using System.Reflection;
using smg.ArgumentParsing.Attributes;

namespace smg.ArgumentParsing
{
    /// <summary>
    /// Data which is used to initialize <see cref="StateMachineGenerator"/>.
    /// </summary>
    public class ArgsData
    {
        /// <summary>
        /// Indicates that help display is required.
        /// </summary>
        [BooleanArg("/h", "-h", "/help", "-help", "/?", "-?")]
        public bool Help { get; private set; }

        /// <summary>
        /// Indicates that the generator should generate a DLL instead of .cs files.
        /// </summary>
        [BooleanArg("/a", "/assembly", "/d", "/dll")]
        public bool GenerateAssembly { get; private set; }

        /// <summary>
        /// Specifies the path to the <see cref="Assembly"/> which contains the class for the code to be generated according to.
        /// </summary>
        [IndexedArg(0)]
        public string SourceAssemblyPath { get; private set; }

        /// <summary>
        /// Specifies the class which will be used to generate the states according to.
        /// </summary>
        [IndexedArg(1)]
        public string StateClassName { get; private set; }

        /// <summary>
        /// Specifies the path for the generated .cs/DLLs to be saved to.
        /// </summary>
        [IndexedArg(2)]
        public string OutputPath { get; private set; }
    }
}