#if UNITY_EDITOR || DEBUG || UTILSPACKAGE_DEBUG
using UnityEngine;
using UnityEngine.Diagnostics;

namespace raiden.utils
{
    public class BaseDebugOperations
    {
        [DebugOperation("Debug Log")]
        public static void DebugLog()
        {
            Debug.Log("DebugLog");
        }

        [DebugOperation("Toggle cursor visibiltiy")]
        public static void ToggleCursorVisibility()
        {
            Cursor.visible = !Cursor.visible;
        }

        [DebugOperation("Toggle cursor lockstate")]
        public static void ToggleCursorLockState()
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        [DebugOperation("Freeze game")]
        public static void FreezeGame()
        {
            if (Application.isEditor)
            {
                DebugUI.instance.ShowDialog("This action cannot be performed in the Unity Editor.");
                return;
            }

            while (true)
            {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
                bool a = true;
#pragma warning restore CS0219 // Variable is assigned but its value is never used
            }
        }

        [DebugOperation("Force crash")]
        public static void ForceCrash()
        {
            if (Application.isEditor)
            {
                DebugUI.instance.ShowDialog("This action cannot be performed in the Unity Editor.");
                return;
            }

            Utils.ForceCrash(ForcedCrashCategory.FatalError);
        }
    }
}
#endif