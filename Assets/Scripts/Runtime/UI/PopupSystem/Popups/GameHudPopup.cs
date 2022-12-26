using MyPong.Core;
using MyPong.Networking;
using MyPong.UI.Popups.Base;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace MyPong.UI.Popups
{
    [UnityEngine.Scripting.Preserve]
    public class GameHudPopupController : BasePopupController
    {
        public readonly UnetWrapper UnetWrapper;
        public GameHudPopupController(
            PopupService popupService,
            UnetWrapper unetWrapper)
            : base(popupService)
        {
            UnetWrapper = unetWrapper;
        }
    }
    
    public class GameHudPopup : BasePopupWithController<GameHudPopup.Data, GameHudPopupController>
    {
        [SerializeField] private Button _sutdownButton;
        [SerializeField] private TMP_Text _enemyScore;
        [SerializeField] private TMP_Text _myScore;
        
        public override bool IsUnclosable => false;
        public override bool IsOnlyOne => true;
        
        protected override void InternalInit()
        {
            _sutdownButton.OnClickAsObservable().Subscribe(_ =>
            {
                Controller.UnetWrapper.Shutdown();
                ClosePopup();
            }).AddTo(this);

            var player = Controller.UnetWrapper.GetLocalPlayer();
            _enemyScore.text = player.EnemyScoreRx.Value.ToString();
            _myScore.text = player.MyScoreRx.Value.ToString();
            player.EnemyScoreRx.Subscribe(v => _enemyScore.text = v.ToString()).AddTo(this);
            player.MyScoreRx.Subscribe(v => _myScore.text = v.ToString()).AddTo(this);
        }

        public override void Dispose() { }
        
        public class Data : IPopupData { }
    }
}