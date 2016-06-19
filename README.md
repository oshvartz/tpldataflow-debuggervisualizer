# TPL DataFlow Debugger Visualizer
Enables you to to view a graphic view (integrated in VS 15) of your TPL data flow network including the state of each block
# Visual Studio Gallery
https://visualstudiogallery.msdn.microsoft.com/20f5adc0-984f-4158-8e09-7406a6581b5b
# TPL Data Flow
The Task Parallel Library (TPL) provides dataflow components to help increase the robustness of concurrency-enabled applications. These dataflow components are collectively referred to as the TPL Dataflow Library. This dataflow model promotes actor-based programming by providing in-process message passing for coarse-grained dataflow and pipelining tasks.These dataflow components are useful when you have multiple operations that must communicate with one another asynchronously or when you want to process data as it becomes available. For example, consider an application that processes image data from a web camera. By using the dataflow model, the application can process image frames as they become available. If the application enhances image frames, for example, by performing light correction or red-eye reduction, you can create a pipeline of dataflow components. Each stage of the pipeline might use more coarse-grained parallelism functionality, such as the functionality that is provided by the TPL, to transform the image. (MSDN)

# We used
Graph# http://graphsharp.codeplex.com/


Read more in my blog
http://blogs.microsoft.co.il/blogs/oshvartz/
