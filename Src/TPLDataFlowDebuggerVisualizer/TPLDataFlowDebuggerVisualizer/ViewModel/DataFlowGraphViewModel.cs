using System.Collections.Concurrent;
using QuickGraph;
using TPLDataFlowDebuggerVisualizer.Common;


namespace TPLDataFlowDebuggerVisualizer.ViewModel
{
    public class DataFlowGraphViewModel
    {
        public DataFlowGraphViewModel(DataFlowDebuggerInfo dataFlowDebuggerInfo)
        {
            InitGrpah(dataFlowDebuggerInfo);
        }

      
        private void InitGrpah(DataFlowDebuggerInfo dataFlowDebuggerInfo)
        {
            var graph = new BidirectionalGraph<DataFlowDebuggerInfo, IEdge<DataFlowDebuggerInfo>>();
            var nodesDic = new ConcurrentDictionary<int, DataFlowDebuggerInfo>();
            BuildGraph(graph, dataFlowDebuggerInfo, nodesDic);
            GraphToVisualize = graph;
        }

        private void BuildGraph(BidirectionalGraph<DataFlowDebuggerInfo, IEdge<DataFlowDebuggerInfo>> graph, DataFlowDebuggerInfo dataFlowDebuggerInfo, ConcurrentDictionary<int, DataFlowDebuggerInfo> nodesDic)
        {
            DataFlowDebuggerInfo curDataFlowDebuggerInfo;
            if (!nodesDic.ContainsKey(dataFlowDebuggerInfo.Id))
            {
                graph.AddVertex(dataFlowDebuggerInfo);
                nodesDic.GetOrAdd(dataFlowDebuggerInfo.Id, dataFlowDebuggerInfo);
                curDataFlowDebuggerInfo = dataFlowDebuggerInfo;
            }
            else
            {

                curDataFlowDebuggerInfo = dataFlowDebuggerInfo;
            }

            if (curDataFlowDebuggerInfo.LinkedTargets != null && curDataFlowDebuggerInfo.LinkedTargets.Count > 0)
            {
                curDataFlowDebuggerInfo.LinkedTargets.ForEach(traget =>
                {
                    BuildGraph(graph, traget, nodesDic);
                    graph.AddEdge(new Edge<DataFlowDebuggerInfo>(curDataFlowDebuggerInfo, traget));


                });
            }
        }

        public IBidirectionalGraph<DataFlowDebuggerInfo, IEdge<DataFlowDebuggerInfo>> GraphToVisualize { get; set; }

    }
}
