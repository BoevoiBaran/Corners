using System;
using UnityEngine;

namespace Code.Main.StepLogicStrategy
{
    public static class StepProcessorFactory
    {
        public static IStepProcessor GetStepProcessor(StepType type)
        {
            switch (type)
            {
                case StepType.Straight:
                    return new WalkStraightStepProcessor();
                case StepType.Diagonally:
                    return new WalkDiagonallyStepProcessor();
                default:
                    Debug.LogError($"Incorrect session step type:{type}");
                    throw new ArgumentException();
            }
        }
    }
}
