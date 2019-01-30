using System;
using System.Net.NetworkInformation;
using System.Windows.Input;
using SvgMakerCore.Core;
using SvgMakerCore.Core.Operation;

namespace SvgMakerCore.Utility
{
    public static class OperationEx
    {
        public static void Execute(this IOperationController controller, Action execute, Action rollback)
        {
            controller.Execute(new DelegateOperation(execute, rollback));
        }

        public static ICommand ToCommand(this IOperation operation, IOperationController controller)
        {
            return new DelegateCommand(()=>controller.Execute(operation));
        }
    }
}
