using UnityEngine;

namespace Extensions.Unity
{
    public static class MonoBehaviourExtensions
    {
        public static T AddMissingComponent<T>(this GameObject agent) where T : Component
        {
            var c = agent.GetComponent<T>();
            return c != null ? c : agent.AddComponent<T>();
        }
        public static T AddMissingComponent<T>(this Component agent) where T : Component
        {
            return agent.gameObject.AddMissingComponent<T>();
        }
    }
}