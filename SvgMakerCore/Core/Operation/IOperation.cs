
namespace SvgMakerCore.Core.Operation
{
    public interface IOperation
    {
        void RollForward();
        void Rollback();
    }
}
