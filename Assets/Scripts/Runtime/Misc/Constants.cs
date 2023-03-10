using UnityEngine.SceneManagement;

namespace MyPong
{
    public static class Constants
    {
        public static class Scenes
        {
            public static readonly int Game = SceneUtility.GetBuildIndexByScenePath("game");
        }
        public static class Network
        {
#if PONG_BOT
            public const int PlayersCount = 1;
#else
            public const int PlayersCount = 2;
#endif
        }
        
        public static class Gameplay
        {
            public const int StartTimerSeconds = 3;
            public const int MaxScore = 10;
            public const float BoostersSpawnInterval = 20f;
        }
    }
}
