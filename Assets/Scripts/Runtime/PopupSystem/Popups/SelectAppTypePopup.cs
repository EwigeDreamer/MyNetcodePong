using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyPong.Popups.Base;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MyPong.Popups
{
    public class SelectAppTypePopupController : BasePopupController
    {
        public SelectAppTypePopupController(PopupService popupService) : base(popupService) { }

        public void OpenHostSettings()
        {
            PopupService.OpenPopup<HostSettingsPopup>().Forget();
        }

        public void OpenClientSettings()
        {
            PopupService.OpenPopup<ClientSettingsPopup>().Forget();
        }
    }
    
    public class SelectAppTypePopup : BasePopupWithController<SelectAppTypePopup.Data, SelectAppTypePopupController>
    {
        [SerializeField] private Button _HostButton;
        [SerializeField] private Button _ClientButton;

        public override bool IsUnclosable => false;
        public override bool IsOnlyOne => false;

        protected override void InternalInit()
        {
            _HostButton.OnClickAsObservable().Subscribe(_ => Controller.OpenHostSettings()).AddTo(this);
            _ClientButton.OnClickAsObservable().Subscribe(_ => Controller.OpenClientSettings()).AddTo(this);
        }
        
        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
        
        public class Data : IPopupData { }
    }
}
