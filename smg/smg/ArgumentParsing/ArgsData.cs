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
    }
}