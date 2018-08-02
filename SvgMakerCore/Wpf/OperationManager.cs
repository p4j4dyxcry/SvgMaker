using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace SvgMakerCore.Wpf
{
    public class OperationManager : NotifyPropertyChanger , IEnumerable<IOperation>
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
            if (CanUndo)
            {
                PreStackChanged();
                _undoStack.Undo().Rollback();
                OnStackChanged();
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                PreStackChanged();
                _undoStack.Redo().Execute();
                OnStackChanged();
            }
        }

        public IOperation Push(IOperation operation)
        {
            Debug.Assert(operation != null);
            PreStackChanged();
            _undoStack.Push(operation).Execute();
            OnStackChanged();
            return operation;
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

        public void Execute(IOperation operation)
        {
            Push(operation).Execute();
        }

        private bool _prevCanRedo;
        private bool _prevCanUndo;
        private int _preStackChangedCall = 0;
        private void PreStackChanged()
        {
            Debug.Assert(_preStackChangedCall == 0);
            _preStackChangedCall++;
            _prevCanRedo = CanRedo;
            _prevCanUndo = CanUndo;
        }

        private void OnStackChanged()
        {
            Debug.Assert(_preStackChangedCall == 1);
            _preStackChangedCall--;
            if (_prevCanUndo != CanUndo)
                OnPropertyChanged(nameof(CanUndo));

            if (_prevCanRedo != CanRedo)
              OnPropertyChanged(nameof(CanRedo));

            OnPropertyChanged(nameof(OperationManager));
        }

        public IEnumerator<IOperation> GetEnumerator()
        {
            return _undoStack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
