using System;
using System.Runtime.CompilerServices;

namespace SvgMakerCore.Wpf
{
    public class PropertyChangeOperation<T> : IOperation
    {
        private string PropertyName { get; }
        public TimeSpan Permission { get; set; } = TimeSpan.MaxValue;
        private DateTime TimeStamp { get; set; }
        private T PrevProperty { get; set; }
        private T Property { get; }

        private Action<T> Setter { get; }

        public PropertyChangeOperation(
            Action<T> setter,
            T newValue,
            T oldValue,
            string propertyName)
        {
            Setter = setter;
            PrevProperty = oldValue;
            Property = newValue;
            PropertyName = propertyName;
            TimeStamp = DateTime.Now;
        }

        public void Execute()
        {
            Setter.Invoke(Property);
        }

        public void Rollback()
        {
            Setter.Invoke(PrevProperty);
        }

        public void MergeAndExecute(OperationManager operationManager)
        {
            if (operationManager.CanUndo is false)
            {
                operationManager.Execute(this);
                return;
            }

            var topCommand = operationManager.Peek();
            var prevValue = PrevProperty;
            var prevTimeStamp = TimeStamp;
            while (topCommand is PropertyChangeOperation<T> propertyChangeOperation)
            {
                if (propertyChangeOperation.PropertyName != PropertyName)
                    break;

                if (TimeStamp - propertyChangeOperation.TimeStamp > Permission)
                    break;

                prevValue = propertyChangeOperation.PrevProperty;
                prevTimeStamp = propertyChangeOperation.TimeStamp;
                operationManager.Pop();
                topCommand = operationManager.Peek();
            }

            PrevProperty = prevValue;
            TimeStamp = prevTimeStamp;
            operationManager.Execute(this);
        }

        public override string ToString()
        {
            return $"PropertyChangeCommand<{typeof(T)}>\n" +
                   $"\tProperty = \"{PropertyName}\"\n" +
                   $"\tValue = {Property}\n" +
                   $"\tPrev  = {PrevProperty}\n" +
                   $"TimeStamp = {TimeStamp}";
        }
    }
}
