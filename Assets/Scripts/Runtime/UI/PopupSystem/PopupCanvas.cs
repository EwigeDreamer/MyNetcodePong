using UnityEngine;


namespace MyPong.UI.Popups.Misc
{
    public class PopupCanvas : MonoBehaviour
    {
        [SerializeField] private Transform _popupContainer;

        public Transform PopupContainer => _popupContainer;
    }
}