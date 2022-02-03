using UnityEngine;

namespace Settings
{
    public static class Constants
    {
        #region Game
    
        public const string GameAssetPath = "Game/";
        public const int GameFieldX = 8;
        public const int GameFieldY = 8;
        public const int NeedHoldNodesForVictory = 9;
        
    
        #endregion
    
        #region Ui
    
        public const string UiAssetPath = "UI/";
    
        #endregion

        #region Shader property

        public static readonly int ColorProperty = Shader.PropertyToID("_Color");

        #endregion
        
        public static readonly int[,] CornersFieldStartPositions =
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
    }
}
