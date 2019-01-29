using System.Collections.Generic;
using System.Windows.Input;

namespace SvgMakerCore.Core.Operation
{
    public interface IOperation
    {
        void Execute();
        void Rollback();
    }

    public static class OperationEx
    {
        public static IOperation ExecuteToManager(this IOperation _this ,OperationManager manager)
        {
            manager.Execute(_this);
            return _this;
        }

        public static IEnumerable<IOperation> ToEnumerable(this IOperation _this)
        {
            yield return _this;
        }
    }
}
