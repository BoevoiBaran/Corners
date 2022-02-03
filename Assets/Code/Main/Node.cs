using Settings;
using UnityEngine;

namespace Code.Main
{
    public enum NodeColor
    {
        Unknown,
        White,
        Black
    }
    
    [RequireComponent(typeof(Renderer))]
    public class Node : MonoBehaviour
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        
        public int CheckerType => (int) checker.CheckerColor;
        public bool IsCheckerActive => checker.IsActive;

        [SerializeField] private Checker checker;
        
        private Renderer _nodeRenderer;
        private Transform _selfTransform;
        
        private MaterialPropertyBlock _whiteProperty;
        private MaterialPropertyBlock _blackProperty;

        private void Awake()
        {
            _nodeRenderer = GetComponent<Renderer>();
            _selfTransform = GetComponent<Transform>();
            
            _whiteProperty = new MaterialPropertyBlock();
            _whiteProperty.SetColor(Constants.ColorProperty, Color.white);

            _blackProperty = new MaterialPropertyBlock();
            _blackProperty.SetColor(Constants.ColorProperty, Color.red);
        }

        public void SetupNode(Vector3 nodePosition, NodeColor nodeColor, int startPositionValue, int x, int y)
        {
            _selfTransform.position = nodePosition;
            
            X = x;
            Y = y;

            switch (nodeColor)
            {
                case NodeColor.White:
                    _nodeRenderer.SetPropertyBlock(_whiteProperty);
                    break;
                case NodeColor.Black:
                    _nodeRenderer.SetPropertyBlock(_blackProperty);
                    break;
                default:
                    Debug.LogError($"Incorrect node color type:{nodeColor}");
                    break;
            }

            if (startPositionValue != 0)
            {
                checker.Show();
                checker.SetupChecker((CheckerColor)startPositionValue);    
            }
            else
            {
                checker.Hide();
            }
        }

        public void ShowCheckerOnNode(CheckerColor color)
        {
            checker.Show();
            checker.SetupChecker(color);
        }
        
        public void HideCheckerFromNode()
        {
            checker.Hide();
        }

        public bool TrySelectChecker(CheckerColor currentPlayerColor)
        {
            if (checker.IsActive && 
                currentPlayerColor == checker.CheckerColor)
            {
                checker.SetCheckerSelected(true);
                return true;
            }

            return false;
        }
        
        public void DeselectChecker()
        {
            if (checker.IsActive)
            {
                checker.SetCheckerSelected(false);
            }
        }
    }
}
