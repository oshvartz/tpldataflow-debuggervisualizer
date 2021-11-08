# TPL DataFlow Debugger Visualizer
Enables you to view a graphic view (integrated in VS 2019) of your TPL data flow network including the state of each block

![alt text for screen readers](img/tpldataflow.png)

# versions supported
 **System.Threading.Tasks.Dataflow** version >= 4.9

# How to Install
* Build
* Copy the assemblies (see list) to: 
VisualStudioInstallPath\Common7\Packages\Debugger\Visualizers
* [Optional] (use use in .net core) copy assemblies in addition to:
VisualStudioInstallPath\Common7\Packages\Debugger\Visualizers\netcoreapp

## Assemblies List
* TPLDataFlowDebuggerVisualizer.dll
* QuickGraph.dll
* QuickGraph.Graphviz.dll
* QuickGraph.Serialization.dll
* GraphSharp.dll
* GraphSharp.Controls.dll
* WPFExtensions.dll
* QuickGraph.Data.dll



# We used
Graph# http://graphsharp.codeplex.com/


Read more in my blog
http://blogs.microsoft.co.il/blogs/oshvartz/
