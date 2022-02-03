using System;

namespace Code.Main.StepLogicStrategy
{
    public class WalkDiagonallyStepProcessor : IStepProcessor
    {
        public bool IsStepPossible(Node selectedCheckerNode, Node selectedEmptyNode, int[,] fieldState,
            CheckerColor currentPlayer)
        {
            var diffX = Math.Abs(selectedCheckerNode.X - selectedEmptyNode.X);
            var diffY = Math.Abs(selectedCheckerNode.Y - selectedEmptyNode.Y);

            if (diffX != diffY)
            {
                return false;
            }
            
            if (diffX == 1 && diffY == 1)
            {
                return true;
            }
            
            if (diffX == 2 && diffY == 2)
            {
                var middleX = selectedCheckerNode.X > selectedEmptyNode.X 
                    ? selectedEmptyNode.X + 1 
                    : selectedEmptyNode.X - 1;
                
                var middleY = selectedCheckerNode.Y > selectedEmptyNode.Y 
                    ? selectedEmptyNode.Y + 1 
                    : selectedEmptyNode.Y - 1;
                
                var nodeState = fieldState[middleX, middleY];
                return nodeState != 0 && nodeState != (int) currentPlayer;
            }

            return false;
        }
    }
}