using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TPLDataFlowDebuggerVisualizer.Common
{
    [DataContract(IsReference=true)]
    public class DataFlowDebuggerInfo
    {
        public DataFlowDebuggerInfo()
        {
            LinkedTargets = new List<DataFlowDebuggerInfo>();
        }
        [DataMember]
        public bool IsFirstBlock { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string BlockType { get; set; }
        [DataMember]
        public int CurrentDegreeOfParallelism { get; set; }
        [DataMember]
        public bool IsCompleted { get; set; }
        [DataMember]
        public bool IsDecliningPermanently { get; set; }
        [DataMember]
        public int InputQueueLength { get; set; }
        [DataMember]
        public int OutputQueueLength { get; set; }
        [DataMember]
        public int BoundedCapacity { get; set; }
        [DataMember]
        public int MaxMessagesPerTask { get; set; }
        [DataMember]
        public List<DataFlowDebuggerInfo> LinkedTargets { get; set; }
    }
}
