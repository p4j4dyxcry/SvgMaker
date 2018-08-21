using System;
using System.Threading.Tasks;

namespace SvgMakerCore.Core.Operation
{
    /// <summary>
    /// 識別子とタイムスタンプからマージ可能か判断する
    /// </summary>
    public class KeyOperationMergeJudger<T> : IOperationMergeJudger
    {
        public T Key { get; }

        public TimeSpan Permission { get; set; } = TimeSpan.MaxValue;

        private DateTime TimeStamp { get; } = DateTime.Now;

        public bool CanMerge(IOperationMergeJudger operationMergeJudger)
        {
            if (operationMergeJudger is KeyOperationMergeJudger<T> timeStampMergeInfo)
            {
                return Equals(Key, timeStampMergeInfo.Key) &&
                       TimeStamp - timeStampMergeInfo.TimeStamp < Permission;
            }
            return false;
        }

        public KeyOperationMergeJudger(T key)
        {
            Key = key;
        }
    }
}
