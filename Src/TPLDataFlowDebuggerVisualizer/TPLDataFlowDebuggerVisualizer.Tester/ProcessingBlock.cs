using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPLDataFlowDebuggerVisualizer.Tester
{
    public class MyProcessBlock : ProcessingBlock<int>
    {

        protected override void OnComplete()
        {
            Console.WriteLine("OnComplete");
        }
    }

    public abstract class ProcessingBlock<T> : ISourceBlock<T>
    {
        

        private readonly BroadcastBlock<T> _broadcast = new BroadcastBlock<T>(i => i);

        protected ProcessingBlock()
        {
           
        }

        protected void SendAsync(T message)
        {
            _broadcast.SendAsync(message);
        }

        protected abstract void OnComplete();

        public T ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target, out bool messageConsumed)
        {
            return ((ISourceBlock<T>)_broadcast).ConsumeMessage(messageHeader, target, out messageConsumed);
        }

        public IDisposable LinkTo(ITargetBlock<T> target, DataflowLinkOptions linkOptions)
        {
            return _broadcast.LinkTo(target, linkOptions);
        }

        public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            ((ISourceBlock<T>)_broadcast).ReleaseReservation(messageHeader, target);
        }

        public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<T> target)
        {
            return ((ISourceBlock<T>)_broadcast).ReserveMessage(messageHeader, target);
        }

        public void Complete()
        {
            _broadcast.Complete();
            OnComplete();
        }

        public Task Completion
        {
            get { return _broadcast.Completion; }
        }

        public void Fault(Exception exception)
        {
            ((ISourceBlock<T>)_broadcast).Fault(exception);
            OnComplete();
        }
    }
}
