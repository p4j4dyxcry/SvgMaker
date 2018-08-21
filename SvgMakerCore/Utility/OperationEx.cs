using System;
using System.Net.NetworkInformation;
using System.Windows.Input;
using SvgMakerCore.Core;
using SvgMakerCore.Core.Operation;

namespace SvgMakerCore.Utility
{
    public static class OperationEx
    {
        public static void Execute(this OperationManager manager, Action execute, Action rollback)
        {
            manager.Execute(new DelegeteOperation(execute, rollback));
        }

        public static ICommand ToCommand(this IOperation operation, OperationManager manager)
        {
            return new DelegateCommnd(()=>manager.Execute(operation));
        }
    }
}
