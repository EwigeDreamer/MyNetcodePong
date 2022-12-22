using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using MyPong.UI.Popups.Base;
using MyPong.UI.Popups.Misc;
using UnityEngine;
using Utilities;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace MyPong.UI.Popups
{
    [UnityEngine.Scripting.Preserve]
    public class PopupService
    {
        private readonly LifetimeScope ParentScope;
        private readonly PopupCanvas _popupCanvas;

        public PopupService(LifetimeScope parentScope, PopupCanvas popupCanvas)
        {
            ParentScope = parentScope;
            _popupCanvas = popupCanvas;
        }


        private readonly StackLikeList<PopupScopeWrapper> _popups = new();

        private bool _inProcess = false;

        public async UniTask OpenPopup<T>(IPopupData data = null) where T : BasePopup
        {
            await UniTask.WaitWhile(() => _inProcess);

            if (!CanOpenNew<T>())
            {
                Debug.LogError($"Can't open another {typeof(T).Name}, because it can be only one!");
                return;
            }
            
            _inProcess = true;
            var prefab = await AssertService.LoadGameobjectFromAddressablesAsync<T>(ResourceType.Popup);
            var prevPopup = _popups.TryPeek(out var x) ? x.Popup : null;
            var nextPopup = Object.Instantiate(prefab, _popupCanvas.PopupContainer);
            var scope = TryInject(nextPopup);
            _popups.Push(new PopupScopeWrapper(nextPopup, scope));
            nextPopup.Init(data);

            var hidingTask = prevPopup != null ? prevPopup.Hide() : UniTask.CompletedTask;
            var showingTask = nextPopup.Show();

            await UniTask.WhenAll(hidingTask, showingTask);
            _inProcess = false;
        }

        public async UniTask ClosePopup(BasePopup basePopup)
        {
            if (basePopup == null) return;
            if (basePopup.IsUnclosable) return;
            await UniTask.WaitUntil(() => _inProcess == false);
            
            _inProcess = true;
            var facade = _popups.FirstOrDefault(a => a.Popup == basePopup);
            _popups.Remove(facade);

            var hidingTask = basePopup.Hide();
            var showingTask = _popups.TryPeek(out var last) && !last.Popup.IsOpened
                ? last.Popup.Show()
                : UniTask.CompletedTask;

            await UniTask.WhenAll(hidingTask, showingTask);
            facade?.Dispose();
            _inProcess = false;
        }

        public async UniTask CloseAll<T>() where T : BasePopup
        {
            foreach (var p in _popups.Select(a => a).Reverse())
                if (p.Popup is T)
                    await ClosePopup(p.Popup);
        }

        private bool CanOpenNew<T>() where T : BasePopup
        {
            foreach(var p in _popups)
                if (p.Popup is T && p.Popup.IsOnlyOne)
                    return false;
            return true;
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
        public readonly BasePopup Popup;
        private readonly LifetimeScope Scope;

        public PopupScopeWrapper(BasePopup popup, LifetimeScope scope)
        {
            Popup = popup;
            Scope = scope;
        }

        public void Dispose()
        {
            Popup.Dispose();
            if (Popup != null)
                Object.Destroy(Popup.gameObject);

            if (Scope != null)
                Scope.Dispose();
        }
    }
}
