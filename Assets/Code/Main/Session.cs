using System;
using Code.Main.StepLogicStrategy;
using Code.Session;
using Settings;
using UnityEngine;

namespace Code.Main
{
    public class Session : MonoBehaviour
    {
        public event Action<int> OnStepUpdate;
        
        private static int MaxRaycastHitsCount = 2;
        
        [SerializeField] private GameField gameField;

        private IStepProcessor _stepProcessor;
        private Camera _mainCamera;

        private Node _currentSelectedChecker;
        private int _currentStep;
        private StepType _currentStepType;
        private CheckerColor _currentPlayer;
        
        private readonly RaycastHit[] _rHits = new RaycastHit[MaxRaycastHitsCount];
        
        private int[,] _fieldState = 
        {
            { 2, 2, 2, 0, 0, 0, 0, 0},
            { 2, 2, 2, 0, 0, 0, 0, 0},
            { 2, 2, 2, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 0, 0, 0},
            { 0, 0, 0, 0, 0, 1, 1, 1},
            { 0, 0, 0, 0, 0, 1, 1, 1},
            { 0, 0, 0, 0, 0, 1, 1, 1},
        };

        public void InitializeSession(SessionContext sessionContext)
        {
            _mainCamera = Camera.main;
            _currentSelectedChecker = null;

            if (sessionContext.State != null)
            {
                RestoreSession(sessionContext);
            }
            else
            {
                StartNewSession(sessionContext);
            }

            gameField.Initialize(_fieldState);
            _stepProcessor = StepProcessorFactory.GetStepProcessor(_currentStepType);

            OnStepUpdateHolder();
        }

        private void StartNewSession(SessionContext sessionContext)
        {
            _currentStepType = sessionContext.StepType;
            _currentStep = 1;
            _currentPlayer = CheckerColor.White;
        }

        private void RestoreSession(SessionContext sessionContext)
        {
            _currentStepType = (StepType) sessionContext.State.StepType;
            _currentStep = sessionContext.State.CurrentStep;
            _currentPlayer = (CheckerColor) sessionContext.State.CurrentPlayer;
            
            _fieldState = sessionContext.State.FieldState;
        }

        public SessionState GetSessionState()
        {
            return new SessionState
            {
                CurrentPlayer = (int) _currentPlayer,
                StepType = (int) _currentStepType,
                CurrentStep = _currentStep,
                FieldState = _fieldState
            };
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var hitCount = Physics.RaycastNonAlloc(_mainCamera.ScreenPointToRay(Input.mousePosition), _rHits);
                if (hitCount <= 0)
                {
                    return;
                }
                
                for (var hitIndex = 0; hitIndex < hitCount; hitIndex++)
                {
                    var node = _rHits[hitIndex].transform.gameObject.GetComponent<Node>();

                    if (node == null)
                    {
                        continue;
                    }

                    Process(node);

                    break;
                }
            } 
        }
        
        private void Process(Node selectedNode)
        {
            if (TrySelectChecker(selectedNode))
            {
                return;
            }

            if (_currentSelectedChecker is null)
            {
                return;
            }

            if (_fieldState[selectedNode.X, selectedNode.Y] != 0)
            {
                return;
            }

            if (_stepProcessor.IsStepPossible(_currentSelectedChecker, selectedNode, _fieldState, _currentPlayer))
            {
                _fieldState[_currentSelectedChecker.X, _currentSelectedChecker.Y] = 0;
                _fieldState[selectedNode.X, selectedNode.Y] = (int) _currentPlayer;
                
                selectedNode.ShowCheckerOnNode(_currentPlayer);
                
                if (TryFinishSession(_fieldState, (int) _currentPlayer))
                {
                    Core.Instance.FinishSession(_currentStep, _currentPlayer);
                    return;
                }

                _currentStep++;
                OnStepUpdateHolder();

                _currentPlayer = _currentPlayer == CheckerColor.White 
                    ? CheckerColor.Black 
                    : CheckerColor.White;
            
                _currentSelectedChecker.DeselectChecker();
                _currentSelectedChecker.HideCheckerFromNode();
                _currentSelectedChecker = null;    
            }
        }
        
        private bool TrySelectChecker(Node selectedNode)
        {
            if (selectedNode.TrySelectChecker(_currentPlayer))
            {
                if (_currentSelectedChecker != null)
                {
                    _currentSelectedChecker.DeselectChecker();
                }

                _currentSelectedChecker = selectedNode;

                return true;
            }

            return false;
        }

        private bool TryFinishSession(int[,] fieldState, int currentPlayerStep)
        {
            var startField = Constants.CornersFieldStartPositions;
            var rows = startField.GetUpperBound(0) + 1;
            var columns = startField.Length / rows;
            var needHoldStartNodesType = currentPlayerStep == 1 ? 2 : 1;

            var holded = 0;
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    var startNodeState = startField[i, j];
                    if (startNodeState == needHoldStartNodesType)
                    {
                        var node = fieldState[i, j];
                        if (node == currentPlayerStep)
                        {
                            holded++;
                        }
                    }

                    if (holded == Constants.NeedHoldNodesForVictory)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void OnStepUpdateHolder()
        {
            OnStepUpdate?.Invoke(_currentStep);
        }
        
        public void RemoveAllListeners()
        {
            OnStepUpdate = null;
        }
    }
}
