using System.Collections.Generic;
using System.Linq;

namespace OperationSystem.Operation
{
    public class CompositeOperation : IOperation
    {
        private readonly IList<IOperation> _operations ;

        public CompositeOperation(params IOperation[] operations)
        {
            _operations = new List<IOperation>();
            Add(operations);
        }

        public void Add(params IOperation[] operations)
        {
            foreach (var operation in operations)
                _operations.Add(operation);
        }

        public void RollForward()
        {
            foreach (var operation in _operations)
                operation.RollForward();
        }

        public void Rollback()
        {
            foreach (var operation in _operations.Reverse())
                operation.Rollback();
        }
    }
}
