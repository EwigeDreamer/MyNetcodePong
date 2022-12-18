using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using MyPong.Popups.Base;
using UnityEngine;
using Utilities;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace MyPong.Popups
{
    public class PopupService
    {
        private readonly LifetimeScope ParentScope;
        private readonly PopupCanvas PopupCanvas;

        public PopupService(LifetimeScope parentScope, PopupCanvas popupCanvas)
        {
            ParentScope = parentScope;
            PopupCanvas = popupCanvas;
        }


        private readonly StackLikeList<PopupScopeWrapper> _popups = new();

        private bool _inProgress = false;

        public async UniTask<T> OpenPopup<T>(IPopupData data = null) where T : BasePopup
        {
            await UniTask.WaitWhile(() => _inProgress);
            _inProgress = true;

            var prefab = await AssertService.LoadGameobjectFromAddressablesAsync<T>(ResourceType.Popup);
            var prevPopup = _popups.TryPeek(out var x) ? x.BasePopup : null;
            var nextPopup = Object.Instantiate(prefab, PopupCanvas.PopupContainer);
            var scope = TryInject(nextPopup);
            _popups.Push(new PopupScopeWrapper(nextPopup, scope));
            nextPopup.Init(data);

            var hidingTask = prevPopup != null ? prevPopup.Hide() : UniTask.CompletedTask;
            var showingTask = nextPopup.Show();

            await UniTask.WhenAll(hidingTask, showingTask);
            _inProgress = false;
            return nextPopup;
        }

        public async UniTask ClosePopup(BasePopup basePopup)
        {
            if (basePopup == null) return;
            if (basePopup.IsUnclosable) return;
            await UniTask.WaitUntil(() => _inProgress == false);
            _inProgress = true;

            var facade = _popups.FirstOrDefault(a => a.BasePopup == basePopup);
            _popups.Remove(facade);

            var hidingTask = basePopup.Hide();
            var showingTask = _popups.TryPeek(out var last) && !last.BasePopup.IsOpened
                ? last.BasePopup.Show()
                : UniTask.CompletedTask;

            await UniTask.WhenAll(hidingTask, showingTask);
            facade?.Dispose();
            _inProgress = false;
        }

        private LifetimeScope TryInject<T>(T popup) where T : BasePopup
        {
            if (popup is IInstaller installer)
            {
                var scope = ParentScope.CreateChild(builder => installer.Install(builder));
                scope.Container.Inject(popup);
                return scope;
            }

            return null;
        }
    }
    
    public class PopupScopeWrapper : IDisposable
    {
        public readonly BasePopup BasePopup;
        private readonly LifetimeScope Scope;

        public PopupScopeWrapper(BasePopup basePopup, LifetimeScope scope)
        {
            BasePopup = basePopup;
            Scope = scope;
        }

        public void Dispose()
        {
            BasePopup.Dispose();
            if (BasePopup != null)
                UnityEngine.Object.Destroy(BasePopup.gameObject);

            if (Scope != null)
                Scope.Dispose();
        }
    }
}
