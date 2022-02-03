using System.Collections.Generic;
using Settings;
using UnityEngine;

namespace Code.Main
{
    public class GameField : MonoBehaviour
    {
        private const float Offset = 0.5f;

        public List<Node> Nodes => _nodes;
        
        [SerializeField] private Transform gameFieldRoot;

        private readonly List<Node> _nodes = new List<Node>();

        public void Initialize(int[,] fieldState)
        {
            _nodes.Clear();
            
            CreateGameField(fieldState);
        }

        private void CreateGameField(int[,] fieldState)
        {
            var nodePrefab = Resources.Load<Node>($"{Constants.GameAssetPath}Node");

            var x = Constants.GameFieldX;
            var y = Constants.GameFieldY;

            var rows = fieldState.GetUpperBound(0) + 1;
            var columns = fieldState.Length / rows;

            if (rows != x || y != columns)
            {
                Debug.LogError("Incorrect start positions data");
                return;    
            }
            
            for (int indexX = 0; indexX < x; indexX++)
            {
                var row = indexX % 2;
                
                for (int indexY = 0; indexY < y; indexY++)
                {

                    var column = indexY % 2;

                    var nodeColor = NodeColor.Unknown;
                    
                    if (row > 0)
                    {
                        nodeColor = column > 0 ? NodeColor.White: NodeColor.Black;
                    }
                    else
                    {
                        nodeColor = column > 0 ? NodeColor.Black: NodeColor.White;
                    }

                    var node = Instantiate(nodePrefab, gameFieldRoot);
                    var nodePosition = new Vector3(indexX + Offset, 0.0f, indexY + Offset); 
                    
                    node.SetupNode(
                        nodePosition, 
                        nodeColor, 
                        fieldState[indexX, indexY],
                        indexX,
                        indexY);

                    _nodes.Add(node);
                }
            }
        }
    }
}
