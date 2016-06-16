using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace TPLSimpleExample
{
    class Program
    {
        static void Main()
        {
            var rand = new Random(DateTime.Now.Millisecond);

            var broadcastBlock = new BroadcastBlock<int>(x => x);

            var transformPositive = new TransformBlock<int, int>(x =>
            {
                Thread.Sleep(1000);
                return x;
            });

            var transformNegative = new TransformBlock<int, int>(x =>
            {
                Thread.Sleep(2000);
                return x * -1;
            });

            var join = new JoinBlock<int, int>();

            var batchBlock = new BatchBlock<Tuple<int, int>>(5);

            var sumBlock = new ActionBlock<Tuple<int, int>[]>(tuples =>
                                {
                                    foreach (var tuple in tuples)
                                        Console.WriteLine("{0}+({1})={2}", tuple.Item1,
                                                        tuple.Item2,
                                                        tuple.Item1 + tuple.Item2);
                                });


            broadcastBlock.LinkTo(transformPositive, new DataflowLinkOptions { PropagateCompletion = true });
            broadcastBlock.LinkTo(transformNegative, new DataflowLinkOptions { PropagateCompletion = true });

            transformPositive.LinkTo(join.Target1, new DataflowLinkOptions { PropagateCompletion = true });
            transformNegative.LinkTo(join.Target2, new DataflowLinkOptions { PropagateCompletion = true });

            join.LinkTo(batchBlock, new DataflowLinkOptions { PropagateCompletion = true });
            batchBlock.LinkTo(sumBlock, new DataflowLinkOptions {PropagateCompletion = true});

            for (int i = 0; i < 30; i++)
            {
                broadcastBlock.Post(rand.Next(100));
                Thread.Sleep(1000);
            }

            broadcastBlock.Complete();

            Console.ReadLine();
        }
    }
}
