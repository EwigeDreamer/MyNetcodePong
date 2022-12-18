using Cysharp.Threading.Tasks;
using MyPong.Popups;
using VContainer.Unity;

namespace MyPong
{
    public class GameFlow : IStartable
    {
        private readonly PopupService PopupService;

        public GameFlow(PopupService popupService)
        {
            PopupService = popupService;
        }

        public void Start()
        {
            PopupService.OpenPopup<SelectAppTypePopup>().Forget();
        }
    }
}
