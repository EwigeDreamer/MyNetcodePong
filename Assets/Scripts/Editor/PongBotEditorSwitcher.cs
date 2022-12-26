using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

namespace Editor
{
    public static class PongBotEditorSwitcher
    {
        private const string menuPrefix = "Pong";
        private const string botSymbol = "PONG_BOT";
        
        [MenuItem(menuPrefix + "/" + nameof(EnableBot))]
        private static void EnableBot()
        {
            PlayerSettings.GetScriptingDefineSymbols(GetTarget(), out var symbols);
            PlayerSettings.SetScriptingDefineSymbols(GetTarget(), symbols.Append(botSymbol).ToArray());
        }
        [MenuItem(menuPrefix + "/" + nameof(DisableBot))]
        private static void DisableBot()
        {
            PlayerSettings.GetScriptingDefineSymbols(GetTarget(), out var symbols);
            PlayerSettings.SetScriptingDefineSymbols(GetTarget(), symbols.Except(new[] {botSymbol}).ToArray());
        }

        [MenuItem(menuPrefix + "/" + nameof(EnableBot), true)]
        private static bool IsBotDisabled()
        {
            if (EditorApplication.isCompiling) return false;
            PlayerSettings.GetScriptingDefineSymbols(GetTarget(), out var symbols);
            return symbols.All(s => s != botSymbol);
        }

        [MenuItem(menuPrefix + "/" + nameof(DisableBot), true)]
        private static bool IsBotEnabled()
        {
            if (EditorApplication.isCompiling) return false;
            PlayerSettings.GetScriptingDefineSymbols(GetTarget(), out var symbols);
            return symbols.Any(s => s == botSymbol);
        }

        private static NamedBuildTarget GetTarget()
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            return namedBuildTarget;
        }
    }
}