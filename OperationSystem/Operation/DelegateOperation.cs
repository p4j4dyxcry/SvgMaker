using System;
using System.Diagnostics;

namespace OperationSystem.Operation
{
    public class DelegateOperation : IOperation
    {
        private readonly Action _execute;
        private readonly Action _rollback;

        public DelegateOperation( Action execute , Action rollback)
        {
            Debug.Assert(execute != null);
            Debug.Assert(rollback != null);
            _execute = execute;
            _rollback = rollback;            
        }

        public void RollForward()
        {
            _execute.Invoke();
        }

        public void Rollback()
        {
            _rollback.Invoke();
        }
    }
}