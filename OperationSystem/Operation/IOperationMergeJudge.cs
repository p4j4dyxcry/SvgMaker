namespace OperationSystem.Operation
{
    public interface IOperationMergeJudge

    {
        /// <summary>
        /// マージ可能か判断します。
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        bool CanMerge(IOperationMergeJudge operation);
    }
}
