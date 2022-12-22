using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace MyPong
{
    public class BootstrapFlow : IStartable
    {
        public void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            
            SceneManager.LoadScene(Constants.Scenes.Game);
        }
    }
}
