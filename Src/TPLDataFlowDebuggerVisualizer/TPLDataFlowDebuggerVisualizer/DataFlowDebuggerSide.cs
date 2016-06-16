using System.Threading.Tasks.Dataflow;
using Microsoft.VisualStudio.DebuggerVisualizers;
using TPLDataFlowDebuggerVisualizer;
using TPLDataFlowDebuggerVisualizer.Common;
using TPLDataFlowDebuggerVisualizer.ViewModel;
using TPLDataFlowDebuggerVisualizer.Views;
using System.Runtime.Serialization;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(TransformBlock<,>),
Description = "TPL DataFlow Debugger Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(BroadcastBlock<>),
Description = "TPL DataFlow Debugger Visualizer")]
[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(ActionBlock<>),
Description = "TPL DataFlow Debugger Visualizer")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(BufferBlock<>),
Description = "TPL DataFlow Debugger Visualizer")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(WriteOnceBlock<>),
Description = "TPL DataFlow Debugger Visualizer")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(TransformManyBlock<,>),
Description = "TPL DataFlow Debugger Visualizer")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(JoinBlock<,>),
Description = "TPL DataFlow Debugger Visualizer")]


[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(JoinBlock<,,>),
Description = "TPL DataFlow Debugger Visualizer")]

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(BatchedJoinBlock<,>),
Description = "TPL DataFlow Debugger Visualizer")]


[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(BatchedJoinBlock<,,>),
Description = "TPL DataFlow Debugger Visualizer")]



[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(DataFlowDebuggerSide),
typeof(DataFlowVisualizerObjectSource),
Target = typeof(BatchBlock<>),
Description = "TPL DataFlow Debugger Visualizer")]


namespace TPLDataFlowDebuggerVisualizer
{
    public class DataFlowDebuggerSide : DialogDebuggerVisualizer
    {
        private readonly static DataContractSerializer DataContractSerializer = new DataContractSerializer(typeof(DataFlowDebuggerInfo));
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var dataFlowDebuggerInfo = (DataFlowDebuggerInfo)DataContractSerializer.ReadObject(objectProvider.GetData());
            var vm = new DataFlowGraphViewModel(dataFlowDebuggerInfo);
            var toolWindow = new ToolWindow {DataContext = vm};
            toolWindow.ShowDialog();

        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            var visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(DataFlowDebuggerSide), typeof(DataFlowVisualizerObjectSource));
            visualizerHost.ShowVisualizer();
        }
    }
}
