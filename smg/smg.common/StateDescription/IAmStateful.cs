namespace smg.Common.StateDescription
{
    /// <summary>
    /// Indicates that a class is stateful which allows safely throwing exception when state corruption occurs.
    /// </summary>
    interface IAmStateful
    {
        /// <summary>
        /// Contains the valid wrapper of the stateful object at this time,
        /// of the stateful object was used through a variable of a type representing a
        /// different state (a situation which is called 'state corruption'),
        /// an exception can be thrown by the wrapper.
        /// </summary>
        /// <example>
        /// var SomeStatefulObject = SomeFactory.GetUnconfiguredObject();
        /// if (SomeStatefulObject != null)
        /// {
        ///     SomeStatefulObject.Configure(); //This is fine because SomeStatefulObject is still unconfigured.
        /// }
        /// SomeStatefulObject.Configure(); // THIS IS BAD because SomeStatefulObject variable represents
        ///                                 // an unconfigured stateful object but the object has been configured
        ///                                 // (the variable represents a different state than the instance state itself).
        ///                                 // Exception will be thrown here because the wrapper will validate StateWrapper property
        ///                                 // of the stateful object matches the state the wrapper represents.
        /// </example>
        object StateWrapper { get; set; }
    }
}
