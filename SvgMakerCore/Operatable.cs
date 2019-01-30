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
                this.ToPropertyChangedOperation(newValue,propertyName)
                    .AddPreAction(BeginOperation)
                    .AddPostAction(EndOperation)
                    .ExecuteFromManager(OperationManager);
            }
        }

        public Operatable(OperationManager operationManager)
        {
            OperationManager = operationManager;
        }
    }


    public static class MergeableOperationExtensions
    {
        public static IOperation ToPropertyChangedOperation<T, TProperty>(this T _this, TProperty newValue , [CallerMemberName] string propertyName = "") where T : NotifyPropertyChanger
        {
            return _this.GenerateSetOperation(propertyName, newValue)
                .AddPostAction(() => _this.OnPropertyChanged(propertyName));
        }
    }
}
