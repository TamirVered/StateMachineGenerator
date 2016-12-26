using System;
using smg.Common.StateDescription.Attributes;
using smg.Common.StateDescription.LogicalRelations;

namespace smg.UnitTests
{
    /// <summary>
    /// A basic stateful <see cref="Type"/> for testing purposes.
    /// </summary>
    /// <example>
    /// The state group "Position" represents the following positions:
    ///        UP
    ///        |
    /// Left-Middle-Right
    ///        |
    ///       Down
    /// </example>
    [StateGroup("Position", "Left", "Right", "Middle", "Up", "Down")]
    [StateGroup("Movable", "On", "Off")]
    public class TestBasicStatefulType
    {
        private int movements = 0;

        [AvailableForStates("On", "Middle", Relation = typeof(AndLogicalRelation))]
        [AvailableForStates("On", "Down", Relation = typeof(AndLogicalRelation))]
        [ChangeToState("Up", "Middle")]
        [ChangeToState("Middle", "Down")]
        public void MoveUp()
        {
            movements++;
        }

        [AvailableForStates("On", "Middle", Relation = typeof(AndLogicalRelation))]
        [AvailableForStates("On", "Up", Relation = typeof(AndLogicalRelation))]
        [ChangeToState("Middle", "Up")]
        [ChangeToState("Down", "Middle")]
        public void MoveDown()
        {
            movements++;
        }

        [AvailableForStates("On", "Middle", Relation = typeof(AndLogicalRelation))]
        [AvailableForStates("On", "Right", Relation = typeof(AndLogicalRelation))]
        [ChangeToState("Middle", "Right")]
        [ChangeToState("Left", "Middle")]
        public void MoveLeft()
        {
            movements++;
        }

        [AvailableForStates("On", "Middle", Relation = typeof(AndLogicalRelation))]
        [AvailableForStates("On", "Left", Relation = typeof(AndLogicalRelation))]
        [ChangeToState("Middle", "Left")]
        [ChangeToState("Right", "Middle")]
        public void MoveRight()
        {
            movements++;
        }

        [AvailableForAllStates]
        public void Stay()
        {
            
        }

        [AvailableForStates("On")]
        [ChangeToState("Off")]
        public void TurnOff()
        {

        }

        [AvailableForStates("Off")]
        [ChangeToState("On")]
        public void TurnOn()
        {

        }

        [AvailableForAllStates]
        public int GetMovements()
        {
            return movements;
        }
    }
}
