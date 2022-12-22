using UnityEngine.SceneManagement;

namespace MyPong
{
    public static class Constants
    {
        public static class Scenes
        {
            public static readonly int Game = SceneUtility.GetBuildIndexByScenePath("game");
        }
    }
}
