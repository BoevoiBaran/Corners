using System;

namespace Code.Main.StepLogicStrategy
{
    public class WalkStraightStepProcessor : IStepProcessor
    {
        public bool IsStepPossible(Node selectedCheckerNode, Node selectedEmptyNode, int[,] fieldState,
            CheckerColor currentPlayer)
        {
            var diffX = Math.Abs(selectedCheckerNode.X - selectedEmptyNode.X);
            var diffY = Math.Abs(selectedCheckerNode.Y - selectedEmptyNode.Y);

            if (diffX == diffY)
            {
                return false;
            }
            
            if (diffX == 0)
            {
                if (diffY == 1)
                {
                    return true;
                }
                if(diffY == 2)
                {
                    var middleY = selectedCheckerNode.Y > selectedEmptyNode.Y 
                        ? selectedEmptyNode.Y + 1 
                        : selectedEmptyNode.Y - 1;

                    var nodeState = fieldState[selectedCheckerNode.X, middleY];
                    return nodeState != 0 && nodeState != (int) currentPlayer;
                }
            }
            
            if(diffY == 0)
            {
                if (diffX == 1)
                {
                    return true;
                }
                
                if(diffX == 2)
                {
                    var middleX = selectedCheckerNode.X > selectedEmptyNode.X 
                        ? selectedEmptyNode.X + 1 
                        : selectedEmptyNode.X - 1;

                    var nodeState = fieldState[middleX, selectedCheckerNode.Y];
                    return nodeState != 0 && nodeState != (int) currentPlayer;
                }    
            }
            
            return false;
        }
    }
}
