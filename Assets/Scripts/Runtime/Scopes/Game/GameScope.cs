using MyPong.UI.Popups;
using MyPong.UI.Popups.Misc;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MyPong
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private PopupCanvas _popupCanvasPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_popupCanvasPrefab, Lifetime.Singleton);
            builder.Register<PopupService>(Lifetime.Singleton).WithParameter(this);

            builder.RegisterEntryPoint<GameFlow>();
        }
    }
}
