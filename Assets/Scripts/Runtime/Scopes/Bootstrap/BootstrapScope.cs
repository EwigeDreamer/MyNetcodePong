using MyPong.Networking;
using MyPong.UI;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MyPong
{
    public class BootstrapScope : LifetimeScope
    {
        [SerializeField] private NetworkManager _networkManager;
        [SerializeField] private SpawnEventService _spawnEventService;
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private ScreenLocker _screenLocker;
        
        protected override void Awake()
        {
            IsRoot = true;
            DontDestroyOnLoad(this);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_networkManager).AsSelf();
            builder.RegisterComponent(_spawnEventService).AsSelf();
            builder.RegisterComponent(_loadingScreen).AsSelf();
            builder.RegisterComponent(_screenLocker).AsSelf();
            builder.Register<UnetWrapper>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
    }
}
