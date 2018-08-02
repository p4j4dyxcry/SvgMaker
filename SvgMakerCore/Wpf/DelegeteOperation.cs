using System;

namespace SvgMakerCore.Wpf
{
    public class DelegeteOperation : IOperation
    {
        private readonly Action _execute;
        private readonly Action _rollback;

        public DelegeteOperation( Action execute , Action rollback)
        {
            _execute = execute;
            _rollback = rollback;
        }

        public void Execute()
        {
            _execute.Invoke();
        }

        public void Rollback()
        {
            _rollback.Invoke();
        }
    }
}