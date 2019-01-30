using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SvgMakerCore.Core.Operation
{
    public class OperationManager :  IEnumerable<IOperation>
    {
        private readonly UndoStack<IOperation> _undoStack;
        public bool CanUndo => _undoStack.CanUndo;
        public bool CanRedo => _undoStack.CanRedo;

        public OperationManager(int capacity)
        {
            _undoStack = new UndoStack<IOperation>(capacity);
        }

        public void Undo()
        {
            if (!CanUndo)
                return;

            PreStackChanged();
            _undoStack.Undo().Rollback();
            OnStackChanged();
        }

        public void Redo()
        {
            if (!CanRedo)
                return;

            PreStackChanged();
            _undoStack.Redo().Execute();
            OnStackChanged();
        }

        public IOperation Peek()
        {
            return _undoStack.Peek();
        }

        public IOperation Pop()
        {
            PreStackChanged();
            var result = _undoStack.Pop();
            OnStackChanged();
            return result;
        }

        public IOperation Execute(IOperation operation)
        {
            Debug.Assert(operation != null);
            PreStackChanged();
            _undoStack.Push(operation).Execute();
            OnStackChanged();
            return operation;
        }

        #region Enumrable

        public IEnumerator<IOperation> GetEnumerator()
        {
            return _undoStack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region PropertyChanged

        private int _preStackChangedCall;

        public event Action<object,EventArgs> StackChanged;

        private void PreStackChanged()
        {
            Debug.Assert(_preStackChangedCall == 0 , "不正な呼び出し" );
            _preStackChangedCall++;
        }

        private void OnStackChanged()
        {
            Debug.Assert(_preStackChangedCall == 1, "不正な呼び出し");
            _preStackChangedCall--;
            StackChanged?.Invoke(this, new EventArgs());
        }
        #endregion
    }
}
