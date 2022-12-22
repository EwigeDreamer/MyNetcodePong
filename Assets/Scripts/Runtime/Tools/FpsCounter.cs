using System.Collections;
using UnityEngine;

namespace MyPong.Tools
{
    public class FpsCounter : MonoBehaviour
    {
        private float _count;
        private GUIStyle _style;
    
        private IEnumerator Start()
        {
            GUI.depth = 2;
            
            while (true)
            {
                _count = 1f / Time.unscaledDeltaTime;
                yield return new WaitForSeconds(0.25f);
            }
        }

        private GUIStyle GetStyle()
        {
            var style = GUI.skin.label;
            var size = Mathf.RoundToInt(Mathf.Min(Screen.width, Screen.height) / 20f);
            style.fontSize = size;
            return style;
        }
    
        private void OnGUI()
        {
            if (_style == null) _style = GetStyle();
            var size = _style.fontSize;
            GUI.Label(new Rect(size, size, size * 10, size), $"FPS: {_count:F1}", _style);
        }
    }
}
