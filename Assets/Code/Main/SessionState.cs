using System;

namespace Code.Main
{
    [Serializable]
    public class SessionState
    {
        public int[,] FieldState;
        public int CurrentStep;
        public int CurrentPlayer;
        public int StepType;
    }
}
