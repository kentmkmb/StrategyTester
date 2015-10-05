using System;
using System.Runtime.Serialization;


namespace CVARC.V2
{
    [Serializable]
    [DataContract]
    public class RMCommand : ICommand, ISimpleMovementCommand, ITowerBuilderCommand, IGripperCommand, ICombinedCommand
    {
        [DataMember]
        public SimpleMovement SimpleMovement
        {
            get;
            set;
        }

        [DataMember]
        public TowerBuilderAction TowerBuilderCommand
        {
            get;
            set;
        }

        [DataMember]
        public GripperAction GripperCommand
        {
            get;
            set;
        }

        [DataMember]
        public string[] CombinedCommand
        {
            get;
            set;
        }
    }
}
