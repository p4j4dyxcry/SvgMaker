namespace SvgMakerCore.Core.Operation
{
    public interface IOperation
    {
        void Execute();
        void Rollback();
    }
}
