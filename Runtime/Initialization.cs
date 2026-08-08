using UnityEngine;

namespace raiden.utils
{
    public class Initialization
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            WindowsNative.DisableProcessWindowGhosting();
            WindowsNative.DMMain();
        }
#endif
    }
}