namespace SvgMakerCore.Wpf
{
    public interface IOperation
    {
        void Execute();
        void Rollback();
    }
}
