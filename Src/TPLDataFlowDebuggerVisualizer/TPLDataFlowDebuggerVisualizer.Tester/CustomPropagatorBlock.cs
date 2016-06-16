using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace TPLDataFlowDebuggerVisualizer.Tester
{
    public class CustomPropagatorBlock : IPropagatorBlock<int, int>
    {
        private readonly ActionBlock<int> _targetImage;
        private readonly ActionBlock<int> _targetCommand;
        private readonly BroadcastBlock<int> _broadcastBlock = new BroadcastBlock<int>(i => i);



        public CustomPropagatorBlock()
        {
            _targetImage = new ActionBlock<int>(i => { });
            _targetCommand = new ActionBlock<int>(i => { });
           
        }

     

      
    

        public int ConsumeMessage(DataflowMessageHeader messageHeader, ITargetBlock<int> target, out bool messageConsumed)
        {
            return ((ISourceBlock<int>)_broadcastBlock).ConsumeMessage(messageHeader, target, out messageConsumed);
        }

        public IDisposable LinkTo(ITargetBlock<int> target, DataflowLinkOptions linkOptions)
        {
            return _broadcastBlock.LinkTo(target, linkOptions);
        }

        public void ReleaseReservation(DataflowMessageHeader messageHeader, ITargetBlock<int> target)
        {
            ((ISourceBlock<int>)_broadcastBlock).ReleaseReservation(messageHeader, target);
        }

        public bool ReserveMessage(DataflowMessageHeader messageHeader, ITargetBlock<int> target)
        {
            return ((ISourceBlock<int>)_broadcastBlock).ReserveMessage(messageHeader, target);
        }

        public void Complete()
        {
            _broadcastBlock.Complete();
            _targetImage.Complete();
            TargetCommand.Complete();
        }

        public Task Completion
        {
            get { return _broadcastBlock.Completion; }
        }

        public ActionBlock<int> TargetCommand
        {
            get { return _targetCommand; }
        }

        public void Fault(Exception exception)
        {
            ((ISourceBlock<int>)_broadcastBlock).Fault(exception);
        }

        public DataflowMessageStatus OfferMessage(DataflowMessageHeader messageHeader, int messageValue, ISourceBlock<int> source, bool consumeToAccept)
        {
            return ((ITargetBlock<int>)_targetImage).OfferMessage(messageHeader, messageValue, source, consumeToAccept);
        }
    }
}
