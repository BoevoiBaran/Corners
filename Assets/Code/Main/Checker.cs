using Code.Extensions;
using Settings;
using UnityEngine;

namespace Code.Main
{
    public enum CheckerColor
    {
        Unknown,
        White,
        Black
    }
    
    [RequireComponent(typeof(Renderer))]
    public class Checker : MonoBehaviour
    {
        public bool IsActive { get; private set; }
        public CheckerColor CheckerColor { get; private set; }
        
        private Renderer _nodeRenderer;
        
        private MaterialPropertyBlock _whiteProperty;
        private MaterialPropertyBlock _blackProperty;
        private MaterialPropertyBlock _greenProperty;

        private void Awake()
        {
            _nodeRenderer = GetComponent<Renderer>();

            _whiteProperty = new MaterialPropertyBlock();
            _whiteProperty.SetColor(Constants.ColorProperty, Color.white);

            _blackProperty = new MaterialPropertyBlock();
            _blackProperty.SetColor(Constants.ColorProperty, Color.black);
            
            _greenProperty = new MaterialPropertyBlock();
            _greenProperty.SetColor(Constants.ColorProperty, Color.green);
        }

        public void Show()
        {
            IsActive = true;
            gameObject.SetSafeActive(true);
        }

        public void Hide()
        {
            IsActive = false;
            gameObject.SetSafeActive(false);
        }

        public void SetupChecker(CheckerColor color)
        {
            CheckerColor = color;
            
            SetCheckerColor(CheckerColor);
        }

        public void SetCheckerSelected(bool selected)
        {
            if (selected)
            {
                _nodeRenderer.SetPropertyBlock(_greenProperty);
            }
            else
            {
                SetCheckerColor(CheckerColor);
            }
        }

        private void SetCheckerColor(CheckerColor color)
        {
            switch (color)
            {
                case CheckerColor.White:
                    _nodeRenderer.SetPropertyBlock(_whiteProperty);
                    break;
                case CheckerColor.Black:
                    _nodeRenderer.SetPropertyBlock(_blackProperty);
                    break;
                default:
                    Debug.LogError($"Incorrect checker color type:{color}");
                    break;
            }
        }
    }
}
