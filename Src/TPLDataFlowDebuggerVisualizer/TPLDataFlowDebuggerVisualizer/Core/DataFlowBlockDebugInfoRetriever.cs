using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks.Dataflow;
using TPLDataFlowDebuggerVisualizer.Common;


namespace TPLDataFlowDebuggerVisualizer.Core
{
    public static class DataFlowBlockDebugInfoRetriever
    {
        public static DataFlowDebuggerInfo GetDataFlowDebuggerInfo(IDataflowBlock dataflowBlock)
        {
            var nodesDic = new ConcurrentDictionary<int, DataFlowDebuggerInfo>();
            var res = GetDataFlowDebuggerInfoI(dataflowBlock,nodesDic);
            res.IsFirstBlock = true;
            return res;   
        }

   
        private static DataFlowDebuggerInfo GetDataFlowDebuggerInfoI(IDataflowBlock dataflowBlock, ConcurrentDictionary<int, DataFlowDebuggerInfo> nodesDic)
        {
            IEnumerable<IDataflowBlock> linkedTargets;
            
            DataFlowDebuggerInfo res = GetInnerDataFlowDebuggerInfo(dataflowBlock, nodesDic, out linkedTargets);
// ReSharper disable PossibleMultipleEnumeration
            if (linkedTargets != null && linkedTargets.Any())

            {
                foreach (var target in linkedTargets)
                {
                    res.LinkedTargets.Add(GetDataFlowDebuggerInfoI(target, nodesDic));
                }

            }

            return res;

        }

        private static DataFlowDebuggerInfo GetInnerDataFlowDebuggerInfo(IDataflowBlock dataflowBlock, ConcurrentDictionary<int, DataFlowDebuggerInfo> nodesDic, out IEnumerable<IDataflowBlock> linkedTargets)
        {
            DataFlowDebuggerInfo res;
            if (dataflowBlock.GetType().Name.StartsWith("JoinBlock"))
            {
                dataflowBlock = RetrieveJoinBlock(dataflowBlock);
            }

            var dataflowBlockType = dataflowBlock.GetType();
            var debugViewT = dataflowBlockType.GetNestedType("DebugView", BindingFlags.NonPublic);

            if (debugViewT == null)
            {
                res = CreateDtfDebuggerInfoForNonBuildInBlock(dataflowBlock, nodesDic, dataflowBlockType, out linkedTargets);
               return res;
            }

            var debugViewType = debugViewT.MakeGenericType(dataflowBlockType.GetGenericArguments());

            var dvInstance = debugViewType.GetConstructors()[0].Invoke(new object[] { dataflowBlock });
            var id = dataflowBlock.GetType().Name == "FilteredLinkPropagator`1" ? dataflowBlock.Completion.Id * -1: SafeGetProperty<int>(dvInstance, "Id");

            var dataflowBlockOptions = SafeGetProperty<DataflowBlockOptions>(dvInstance, "DataflowBlockOptions");
            res = nodesDic.GetOrAdd(id, new DataFlowDebuggerInfo
           {
               BlockType = GetBlockTypeStr(dataflowBlockType),
               CurrentDegreeOfParallelism = SafeGetProperty<int>(dvInstance, "CurrentDegreeOfParallelism"),
               Id = id,
               IsCompleted = SafeGetProperty<bool>(dvInstance, "IsCompleted"),
               IsDecliningPermanently = SafeGetProperty<bool>(dvInstance, "IsCompleted"),
               InputQueueLength = SafeGetEnumerableLength(dvInstance, "InputQueue"),
               OutputQueueLength = SafeGetEnumerableLength(dvInstance, "OutputQueue"),
               BoundedCapacity = dataflowBlockOptions != null ? dataflowBlockOptions.BoundedCapacity : -1,
               MaxMessagesPerTask = dataflowBlockOptions != null ? dataflowBlockOptions.MaxMessagesPerTask : -1,

           });

            linkedTargets = dataflowBlock.GetType().Name == "FilteredLinkPropagator`1" ? new List<IDataflowBlock> { SafeGetProperty<IDataflowBlock>(dvInstance, "LinkedTarget") } : GetLinkedTragets(dvInstance, "LinkedTargets");

            return res;

        }

        private static DataFlowDebuggerInfo CreateDtfDebuggerInfoForNonBuildInBlock(IDataflowBlock dataflowBlock, ConcurrentDictionary<int, DataFlowDebuggerInfo> nodesDic, Type dataflowBlockType, out IEnumerable<IDataflowBlock> linkedTargets)
        {
          
            var idx = dataflowBlock.GetHashCode();
            var res = nodesDic.GetOrAdd(idx, new DataFlowDebuggerInfo
            {
                BlockType = GetBlockTypeStr(dataflowBlockType),
                Id = idx
            });
            linkedTargets = new List<IDataflowBlock>();
            return res;
        }
       

        private static string GetBlockTypeStr(Type dataflowBlockType)
        {
            if (dataflowBlockType.IsGenericType)
            {
                var sb = new StringBuilder();
                var typeName = dataflowBlockType.GetGenericTypeDefinition().Name;
                var index =typeName.IndexOf("`", StringComparison.Ordinal);
                sb.Append(index > 0 ? typeName.Substring(0, index) : typeName);

                sb.Append("<");

                sb.Append(dataflowBlockType.GetGenericArguments().Select(GetBlockTypeStr).Aggregate((t1, t2) => t1 + "," + t2));

                sb.Append(">");
                return sb.ToString();
            }
            return dataflowBlockType.Name;
        }

        private static IDataflowBlock RetrieveJoinBlock(IDataflowBlock dataflowBlock)
        {
            var joinTargetType = dataflowBlock.GetType().GetField("m_sharedResources", BindingFlags.NonPublic | BindingFlags.Instance);
            Debug.Assert(joinTargetType != null, "joinTargetType != null");

            // ReSharper disable PossibleNullReferenceException
            var joinSourceField = joinTargetType.FieldType.GetField("m_ownerJoin", BindingFlags.NonPublic | BindingFlags.Instance);

            var sharedResources = joinTargetType.GetValue(dataflowBlock);

            var ownerJoin = joinSourceField.GetValue(sharedResources);
// ReSharper restore PossibleNullReferenceException
            dataflowBlock = ownerJoin as IDataflowBlock;
            return dataflowBlock;
        }

        private static IEnumerable<IDataflowBlock> GetLinkedTragets(object instance, string proertyName)
        {
            IEnumerable<IDataflowBlock> res = new List<IDataflowBlock>();
            var pi = instance.GetType().GetProperty(proertyName);
            if (pi != null)
            {
                object targets = pi.GetValue(instance, null);
                var tragetsType = targets.GetType();
                var debugViewType = tragetsType.GetNestedType("DebugView", BindingFlags.NonPublic).MakeGenericType(tragetsType.GetGenericArguments());

                dynamic dvInstance = debugViewType.GetConstructors()[0].Invoke(new[] { targets });

                res = SafeGetProperty<IEnumerable<IDataflowBlock>>(dvInstance, "Targets");

            }
            return res;
        }

        private static int SafeGetEnumerableLength(object instance, string proertyName)
        {
            int res = 0;
            var pi = instance.GetType().GetProperty(proertyName);
            if (pi != null)
            {
                dynamic enumerable = pi.GetValue(instance, null);
                res = Enumerable.Count(enumerable);
            }
            return res;
        }

        private static T SafeGetProperty<T>(object instance, string proertyName, bool isPrivate = false)
        {
            T res = default(T);
            PropertyInfo pi = !isPrivate ? instance.GetType().GetProperty(proertyName) : instance.GetType().GetProperty(proertyName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (pi != null)
            {

                res = (T)pi.GetValue(instance, null);
            }

            return res;
        }
    }
}
