using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Utilities
{
    public enum ResourceType
    {
        Popup,
    }

    public static class AssertService
    {
        private const string POPUP_PREFABS_PATH = "UI/Popups";

        public static async UniTask<T> LoadGameobjectFromAddressablesAsync<T>(ResourceType type, string name = "")
            where T : Object
        {
            name = string.IsNullOrWhiteSpace(name) ? typeof(T).Name : name;
            var go = await LoadFromAddressablesAsync<GameObject>(type, name);
            return go.GetComponent<T>();
        }
        
        public static UniTask<T> LoadFromAddressablesAsync<T>(ResourceType type, string name = "")
            where T : Object
        {
            var path = GetAddressablesKey(type, name);
            return Addressables.LoadAssetAsync<T>(path).ToUniTask();
        }

        private static string GetAddressablesKey(ResourceType type, string name)
        {
            string prefix = string.Empty;

            switch (type) {
                case ResourceType.Popup:
                    prefix = POPUP_PREFABS_PATH;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return $"{prefix}/{name}";
        }
    }
}