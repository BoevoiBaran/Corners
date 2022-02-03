namespace Code.Main.StepLogicStrategy
{
    public interface IStepProcessor
    {
        bool IsStepPossible(Node selectedCheckerNode, Node selectedEmptyNode, int[,] fieldState,
            CheckerColor currentPlayer);
    }
}
