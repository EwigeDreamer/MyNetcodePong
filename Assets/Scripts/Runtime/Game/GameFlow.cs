using Cysharp.Threading.Tasks;
using MyPong.Popups;
using VContainer.Unity;

namespace MyPong
{
    public class GameFlow : IStartable
    {
        private readonly PopupService PopupService;
        private readonly UnetWrapper UnetWrapper;

        public GameFlow(
            PopupService popupService,
            UnetWrapper unetWrapper)
        {
            PopupService = popupService;
            UnetWrapper = unetWrapper;
        }

        public void Start()
        {
            PopupService.OpenPopup<SelectAppTypePopup>().Forget();
        }
    }
}
