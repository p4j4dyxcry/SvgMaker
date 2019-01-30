using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SvgMakerCore.Core.Reflection;

namespace SvgMakerCore.Core.Operation
{
    public static class OperationExtensions
    {
        public static IOperation ExecuteFromManager(this IOperation _this, OperationManager manager)
        {
            if (_this is IMergeableOperation mergeableOperation)
                _this = mergeableOperation.Merge(manager);
            return manager.Execute(_this);
        }

        public static IEnumerable<IOperation> CombineOperations(this IOperation _this, params IOperation[] subOperations)
        {
            yield return _this;
            foreach (var operation in subOperations)
                yield return operation;
        }

        public static IOperation GenerateSetOperation<T, TProperty>(this T _this, string propertyName, TProperty newValue)
        {
            var oldValue = (TProperty)FastReflection.GetProperty(_this, propertyName);
            
            return GenerateAutoMergeOperation(_this, propertyName, newValue, oldValue, $"{_this.GetHashCode()}.{propertyName}");
        }

        public static IOperation GenerateSetOperation<T, TProperty>(this T _this, Expression<Func<T, TProperty>> selector, TProperty newValue)
        {
            var propertyName = selector.GetMemberName();
            
            return GenerateSetOperation(_this, propertyName, newValue);
        }

        public static IOperation GenerateAutoMergeOperation<T, TProperty,TMergeKey>
            (this T _this,string propertyName, TProperty newValue ,TProperty oldValue, TMergeKey mergeKey)
        {
            return new MergeableOperation<TProperty>(
                x => { FastReflection.SetProperty(_this, propertyName, x); },
                newValue,
                oldValue, new KeyOperationMergeJudge<TMergeKey>(mergeKey));
        }

        public static IOperation AddPostAction(this IOperation _this, Action action)
        {
            if (_this is ICustomTriggerOperation triggerOperation)
            {
                triggerOperation.OnExecuted += action;
                return _this;
            }

            return new DelegateOperation(
                () =>
                {
                    _this.Execute();
                    action.Invoke();
                },
                () =>
                {
                    _this.Rollback();
                    action.Invoke();
                });
        }

        public static IOperation AddPreAction(this IOperation _this, Action action)
        {
            if (_this is ICustomTriggerOperation triggerOperation)
            {
                triggerOperation.OnPreviewExecuted += action;
                return _this;
            }

            return new DelegateOperation(
                () =>
                {
                    action.Invoke();
                    _this.Execute();
                },
                () =>
                {
                    action.Invoke();
                    _this.Rollback();
                });
        }
    }
    
    public static class ListOperationExtensions
    {
        public static IOperation ToAddOperation<T>(this IList<T> _this, T value)
            => new InsertOperation<T>(_this, value);

        public static IOperation ToRemoveOperation<T>(this IList<T> _this, T value)
            => new RemoveOperation<T>(_this, value);

        public static IOperation ToRemoveAtOperation(this IList _this, int index)
            => new RemoveAtOperation(_this, index);

        public static IOperation ToAddRangeOperation<T>(this IList<T> _this, params T[] values)
            => ToAddRangeOperation(_this, values as IEnumerable<T>);

        public static IOperation ToAddRangeOperation<T>(this IList<T> _this, IEnumerable<T> values)
        {
            return values
                .Select(x => new InsertOperation<T>(_this, x))
                .ToCompositeOperation();
        }

        public static IOperation ToRemoveRangeOperation<T>(this IList<T> _this, params T[] values)
            => ToRemoveRangeOperation(_this, values as IEnumerable<T>);

        public static IOperation ToRemoveRangeOperation<T>(this IList<T> _this, IEnumerable<T> values)
        {
            return values
                .Select(x => new RemoveOperation<T>(_this, x))
                .ToCompositeOperation();
        }

        public static IOperation ToClearOperation<T>(this IList<T> _this)
            => new ClearOperation<T>(_this);
    }


    public static class CompositeOperationExtensions
    {
        public static IOperation ToCompositeOperation(this IEnumerable<IOperation> operations)
        {
            return new CompositeOperation(operations.ToArray());
        }

        public static IOperation Union(this IOperation _this, params IOperation[] operations)
        {
            return new CompositeOperation(_this.CombineOperations(operations).ToArray());
        }

        public static IOperation Union(this IOperation _this, IEnumerable<IOperation> operations)
        {
            return Union(_this, operations.ToArray());
        }
    }

}
