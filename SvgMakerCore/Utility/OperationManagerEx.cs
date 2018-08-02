using System;
using SvgMakerCore.Wpf;

namespace SvgMakerCore.Utility
{
    public static class OperationManagerEx
    {
        public static void Execute(this OperationManager manager, Action execute, Action rollback)
        {
            manager.Execute(new DelegeteOperation(execute, rollback));
        }
    }
}
