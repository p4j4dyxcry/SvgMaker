using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OperationSystem.Operation
{
    public class InsertOperation<T> : IOperation
    {
        private readonly Func<IList<T>> _generator;
        private readonly IList<T> _list;
        private readonly T _property;

        private IList<T> get_list()
            => _generator?.Invoke() ?? _list;


        public InsertOperation(Func<IList<T>> listGenerator, T insertValue)
        {
            _generator = listGenerator;
            _property = insertValue;
        }

        public InsertOperation(IList<T> list, T insertValue)
        {
            _list = list;
            _property = insertValue;
        }

        public void RollForward()
        {
            get_list().Add(_property);
        }

        public void Rollback()
        {
            var list = get_list();
            list.RemoveAt(list.Count - 1);
        }
    }

    public class RemoveOperation<T> : IOperation
    {
        private readonly Func<IList<T>> _generator;
        private readonly IList<T> _list;
        private readonly T _property;
        private int _insertIndex = -1;

        private IList<T> get_list()
            => _generator?.Invoke() ?? _list;


        public RemoveOperation(Func<IList<T>> listGenerator, T removeValue)
        {
            _generator = listGenerator;
            _property = removeValue;
        }

        public RemoveOperation(IList<T> list, T removeValue)
        {
            _list = list;
            _property = removeValue;
        }

        public void RollForward()
        {
            _insertIndex = get_list().IndexOf(_property);

            if (_insertIndex == -1)
                return;

            get_list().RemoveAt(_insertIndex);
        }

        public void Rollback()
        {
            if (_insertIndex == -1)
                return;
            get_list().Insert(_insertIndex, _property);
        }
    }

    public class RemoveAtOperation : IOperation
    {
        private readonly Func<IList> _generator;
        private readonly IList _list;
        private object _data;
        private readonly int _index ;

        private IList get_list()
            => _generator?.Invoke() ?? _list;


        public RemoveAtOperation(Func<IList> listGenerator, int index)
        {
            _generator = listGenerator;
            _index = index;
        }

        public RemoveAtOperation(IList list, int index)
        {
            _list = list;
            _index = index;
        }

        public void RollForward()
        {
            var list = get_list();
            _data = list[_index];
            list.RemoveAt(_index);
        }

        public void Rollback()
        {
            get_list().Insert(_index, _data);
        }
    }

    public class ClearOperation<T> : IOperation
    {
        private readonly Func<IList<T>> _generator;
        private readonly IList<T> _list;
        private T[] _prevData;

        private IList<T> get_list()
            => _generator?.Invoke() ?? _list;


        public ClearOperation(Func<IList<T>> listGenerator)
        {
            _generator = listGenerator;
        }

        public ClearOperation(IList<T> list)
        {
            _list = list;
        }

        public void RollForward()
        {
            _prevData = get_list().ToArray();
            get_list().Clear();
        }

        public void Rollback()
        {
            foreach (var data in _prevData)
                get_list().Add(data);
        }
    }
}
