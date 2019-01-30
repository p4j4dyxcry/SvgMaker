using System;
using System.Threading.Tasks;

namespace SvgMakerCore.Core.Operation
{
    /// <summary>
    /// 識別子とタイムスタンプからマージ可能か判断する
    /// </summary>
    public class KeyOperationMergeJudge<T> : IOperationMergeJudge
    {
        public T Key { get; }

        public TimeSpan Permission { get; set; } = TimeSpan.MaxValue;

        private DateTime TimeStamp { get; } = DateTime.Now;

        public bool CanMerge(IOperationMergeJudge operationMergeJudge)
        {
            if (operationMergeJudge is KeyOperationMergeJudge<T> timeStampMergeInfo)
            {
                return Equals(Key, timeStampMergeInfo.Key) &&
                       TimeStamp - timeStampMergeInfo.TimeStamp < Permission;
            }
            return false;
        }

        public KeyOperationMergeJudge(T key)
        {
            Key = key;
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}
