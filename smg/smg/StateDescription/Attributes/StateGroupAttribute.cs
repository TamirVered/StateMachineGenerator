using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smg.StateDescription.Attributes
{
    /// <summary>
    /// Represents a group of states where only one of the states can be active.
    /// </summary>
    /// <example>
    /// If a stateful class has 2 state groups:
    ///  - Group 1: StateA, StateB, StateC.
    ///  - Group 2: StateX, StateY.
    /// The stateful class has 6 possible states:
    ///  - StateA + StateX
    ///  - StateB + StateX
    ///  - StateC + StateX
    ///  - StateA + StateY
    ///  - StateB + StateY
    ///  - StateC + StateY
    /// </example>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    internal sealed class StateGroupAttribute : Attribute
    {
        /// <summary>
        /// Name of the state group.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// All of the states available for the state group.
        /// </summary>
        public string[] States { get; private set; }

        /// <summary>
        /// Creates a new instance of <see cref="StateGroupAttribute"/>.
        /// </summary>
        /// <param name="name">Name of the state group.</param>
        /// <param name="states">All of the states available for the state group.</param>
        public StateGroupAttribute(string name, params string[] states)
        {
            Name = name;
            States = states;
        }
    }
}
