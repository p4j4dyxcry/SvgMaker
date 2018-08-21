using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SvgMakerCore.Core
{
    public class AsyncProperty<T>
    {
        private enum States
        {
            Wait,
            Checking,
            Checked
        }

        private T _value;
        private T _checkingValue;

        private readonly Func<T> _generator;
        private readonly Action _generated;
        private States _state = States.Wait;
        private readonly object _lockObj = new object();
        private Dispatcher _dispathcer;


        public AsyncProperty(Func<T> generator, Action generated = null, T value = default(T) , Dispatcher dispatcher = null)
        {
            _generator = generator;
            _generated = generated;
            _checkingValue = value;
            _dispathcer = dispatcher ?? Application.Current?.Dispatcher;
        }

        public void Reset()
        {
            _checkingValue = _value;
            _state = States.Wait;
        }

        public T Value => GerOrGenerateValue();

        private T GerOrGenerateValue()
        {
            if (_state == States.Checked)
                return _value;
            if (_state == States.Checking)
                return _checkingValue;

            lock (_lockObj)
            {
                if (_state == States.Checking)
                    return _checkingValue;

                _state = States.Checking;

                Task.Run(() =>
                {
                    _value = _generator();
                    _state = States.Checked;

                    if (_dispathcer is null)
                        _generated?.Invoke();
                    else if(_generated != null)
                        _dispathcer.Invoke(_generated);
                });

                return _checkingValue;
            }
        }

        public static explicit operator T(AsyncProperty<T> val)
        {
            return val.Value;
        }
    }
}
