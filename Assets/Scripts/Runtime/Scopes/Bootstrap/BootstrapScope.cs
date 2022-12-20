using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MyPong
{
    public class BootstrapScope : LifetimeScope
    {
        [SerializeField] private NetworkManager _networkManager;
        
        protected override void Awake()
        {
            IsRoot = true;
            DontDestroyOnLoad(this);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_networkManager).AsSelf();
            builder.Register<UnetWrapper>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<BootstrapFlow>();
        }
    }
}
