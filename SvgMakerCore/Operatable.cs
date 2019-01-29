using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using SvgMakerCore.Core;
using SvgMakerCore.Core.Operation;
using SvgMakerCore.Core.Reflection;

namespace SvgMakerCore
{
    public class Operatable : NotifyPropertyChanger
    {
        protected OperationManager OperationManager { get; }

        private bool _isPropertyChanging;

        private void BeginOperation()
        {
            Debug.Assert(_isPropertyChanging is false);
            _isPropertyChanging = true;
        }

        private void EndOperation()
        {
            Debug.Assert(_isPropertyChanging is true);
            _isPropertyChanging = false;
        }

        protected void SetPropertyEx<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (_isPropertyChanging)
                SetProperty(ref oldValue, newValue, propertyName);
            else if (Equals(oldValue, newValue) is false)
            {
                void SetAndPropertyChangedInvoke(T property)
                {
                    BeginOperation();
                    FastReflection.SetProperty(this, propertyName, property);
                    EndOperation();
                    OnPropertyChanged(propertyName);
                }

                var mergeInfo = new KeyOperationMergeJudge<string>(propertyName)
                {
                    Permission = TimeSpan.FromSeconds(1)
                };

                var operation = new MergeableOperation<T>(
                    SetAndPropertyChangedInvoke,
                    newValue,
                    oldValue,
                    mergeInfo);

                operation.MergeAndExecute(OperationManager);
            }
        }

        public Operatable(OperationManager operationManager)
        {
            OperationManager = operationManager;
        }
    }


    public static class MergeableOperationExtensions
    {
        public static IOperation ToMergeableOperation<T, TProperty>(this T _this, Expression<Func<T, TProperty>> selector, TProperty newValue) where T : NotifyPropertyChanger
        {
            var propertyName = selector.GetMemberName();
            var oldValue = (TProperty)FastReflection.GetProperty(_this, propertyName);

            return new MergeableOperation<TProperty>(x =>
                {
                    FastReflection.SetProperty(_this, propertyName, x);
                    _this.OnPropertyChanged(propertyName);
                },
                newValue,
                oldValue, new KeyOperationMergeJudge<string>($"{_this.GetHashCode()}.{propertyName}"));
        }
    }
}
