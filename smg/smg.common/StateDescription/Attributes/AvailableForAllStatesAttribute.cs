using System;

namespace smg.Common.StateDescription.Attributes
{
    /// <summary>
    /// Indicates that a method is available for all of the states that the stateful object containing it might be at.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class AvailableForAllStatesAttribute : Attribute
    {
    }
}
