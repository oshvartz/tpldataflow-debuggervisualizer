using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPLDataFlowDebuggerVisualizer.Tester
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var batchRequests = new BatchBlock<int>(batchSize: 10);


            BroadcastBlock<int> br = new BroadcastBlock<int>(x => x);
            ActionBlock<int> actionx = new ActionBlock<int>((i) => Console.WriteLine(i));

            br.LinkTo(actionx, x => x > 10);



            MyProcessBlock pb = new MyProcessBlock();

            pb.LinkTo(actionx);



            var sendToDb = new ActionBlock<int[]>(reqs => Console.WriteLine(reqs.Length));
            batchRequests.LinkTo(sendToDb);


            for (int i = 0; i < 100; i++)
            {
                batchRequests.Post(i);

            }



            WriteOnceBlock<int> wo = new WriteOnceBlock<int>(x => x);
            ActionBlock<int> action = new ActionBlock<int>((i) => Console.WriteLine(i));
            wo.LinkTo(action, new DataflowLinkOptions { PropagateCompletion = true });

            wo.Post(1);
            wo.Post(2);
            wo.Post(3);
            wo.Post(4);
            wo.Post(5);
            wo.Complete();




            BroadcastBlock<int> b = new BroadcastBlock<int>(x => x);



            TransformBlock<int, int> t2 = new TransformBlock<int, int>(x => x);
            TransformBlock<int, int> t = new TransformBlock<int, int>(x => x);

            JoinBlock<int, int> join = new JoinBlock<int, int>();

            ActionBlock<Tuple<int, int>> action2 = new ActionBlock<Tuple<int, int>>((i) => Console.WriteLine("{0}-{1}", i.Item1, i.Item2), new ExecutionDataflowBlockOptions { BoundedCapacity = 10, MaxMessagesPerTask = 30 });

            b.LinkTo(t, x => x > 10);
            b.LinkTo(t2);

            t.LinkTo(action, x => x > 10);
            t2.LinkTo(action, x => x > 10);
            t.LinkTo(join.Target1);
            t2.LinkTo(join.Target1);
            join.LinkTo(action2);

            DataFlowDebuggerSide.TestShowVisualizer(b);


        }
    }
}
