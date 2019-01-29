using System;
using System.ComponentModel;
using System.Linq.Expressions;
using SvgMakerCore.Core.Reflection;

namespace SvgMakerCore.Core.Operation
{
    public interface IMergeableOperation : IOperation
    {
        IOperationMergeJudge MergeJudge { get;set; }
        IOperation Merge(OperationManager operationManager);
    }

    public class MergeableOperation<T> : IMergeableOperation
    {
        private T PrevProperty { get; set; }
        private T Property { get; }
        private Action<T> Setter { get; }
        public IOperationMergeJudge MergeJudge { get; set; }

        public MergeableOperation(
            Action<T> setter,
            T newValue,
            T oldValue,
            IOperationMergeJudge operationMergeJudge = null)
        {
            Setter = setter;
            PrevProperty = oldValue;
            Property = newValue;
            MergeJudge = operationMergeJudge;
        }

        public void Execute()
        {
            Setter.Invoke(Property);
        }

        public void Rollback()
        {
            Setter.Invoke(PrevProperty);
        }

        /// <summary>
        /// OperationManagerのUndoStackを考慮し、
        /// 重複するOperationだった場合はマージしてからOperationを実行します。
        /// </summary>
        /// <param name="operationManager"></param>
        public void MergeAndExecute(OperationManager operationManager)
        {
            var mergedOperation = Merge(operationManager);
            operationManager.Execute(mergedOperation);
        }
        /// <summary>
        /// OperationManagerのUndoStackとマージします。
        /// 統合されたOperationはUndoStackから除外されます。
        /// Operationが統合された場合OperationManagerのRedoStackはクリアされます。
        /// </summary>
        /// <param name="operationManager"></param>
        /// <returns></returns>
        public IOperation Merge(OperationManager operationManager)
        {
            if (operationManager.CanUndo is false)
                return this;

            if (MergeJudge is null)
                return this;

            var topCommand = operationManager.Peek();
            var prevValue = PrevProperty;
            var mergeInfo = MergeJudge;
            while (topCommand is MergeableOperation<T> propertyChangeOperation)
            {
                if (MergeJudge.CanMerge(propertyChangeOperation.MergeJudge) is false)
                    break;
                mergeInfo = propertyChangeOperation.MergeJudge;
                prevValue = propertyChangeOperation.PrevProperty;
                operationManager.Pop();
                topCommand = operationManager.Peek();
            }

            PrevProperty = prevValue;
            MergeJudge = mergeInfo;
            return this;
        }

        public override string ToString()
        {
            return $"Value = {Property,0:.00}\n" +
                   $"Prev  = {PrevProperty,0:.00}";
        }
    }
}
