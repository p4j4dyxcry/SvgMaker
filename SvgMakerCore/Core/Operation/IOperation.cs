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
    }
}
