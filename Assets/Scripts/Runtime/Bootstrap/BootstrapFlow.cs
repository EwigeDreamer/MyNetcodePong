using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace MyPong
{
    public class BootstrapFlow : IStartable
    {
        public void Start()
        {
            SceneManager.LoadScene(Constants.Scenes.Core);
        }
    }
}
