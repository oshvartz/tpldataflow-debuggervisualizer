using System.IO;
using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.DebuggerVisualizers;
using TPLDataFlowDebuggerVisualizer.Common;
using TPLDataFlowDebuggerVisualizer.Core;
using System.Runtime.Serialization;


namespace TPLDataFlowDebuggerVisualizer
{
    public class DataFlowVisualizerObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            var dataFlowDebuggerInfo = DataFlowBlockDebugInfoRetriever.GetDataFlowDebuggerInfo((IDataflowBlock)target);
            var ser = new DataContractSerializer(typeof(DataFlowDebuggerInfo));
            ser.WriteObject(outgoingData, dataFlowDebuggerInfo);
        }
    }
}
