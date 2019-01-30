using System;

namespace SvgMakerCore.Core.Operation
{
    public interface IMergeableOperation : IOperation
    {
        IOperationMergeJudge MergeJudge { get;set; }
        IOperation Merge(OperationManager operationManager);
    }

    internal interface ICustomTriggerOperation 
    {
        event Action OnExecuted;
        event Action OnPreviewExecuted;
    }
    public class MergeableOperation<T> : IMergeableOperation , ICustomTriggerOperation
    {
        private T PrevProperty { get; set; }
        private T Property { get; }
        private Action<T> Setter { get; }
        public IOperationMergeJudge MergeJudge { get; set; }

        public event Action OnExecuted;
        public event Action OnPreviewExecuted;
        
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
            OnPreviewExecuted?.Invoke();
            Setter.Invoke(Property);
            OnExecuted?.Invoke();
        }

        public void Rollback()
        {
            OnPreviewExecuted?.Invoke();
            Setter.Invoke(PrevProperty);
            OnExecuted?.Invoke();
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
            return $"Key        = {MergeJudge}\n" +
                   $"New  Value = {Property,0:.00}\n" +
                   $"Prev Value = {PrevProperty,0:.00}";
        }
    }
}
