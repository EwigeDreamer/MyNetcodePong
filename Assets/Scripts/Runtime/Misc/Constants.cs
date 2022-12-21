using UnityEngine.SceneManagement;

namespace MyPong
{
    public static class Constants
    {
        public static class Scenes
        {
            public static readonly int Core = SceneUtility.GetBuildIndexByScenePath("game");
        }
    }
}
