using System.Runtime.InteropServices;

namespace raiden.utils
{
    /// <summary>
    /// External native DLL functions only on Windows
    /// </summary>
    public static class WindowsNative
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        /// <summary>
        /// BeanShootoutNative_DarkMode.DllMain: Makes the window title dark mode
        /// </summary>
        [DllImport("BeanShootoutNative_DarkMode", EntryPoint = "DllMain")]
        public static extern void DMMain();

        /// <summary>
        /// Prevents the window from going into a not responding state. This stupid fucking thing will end the program if you click right when the program starts responding and i have had it happen to me like 35981092341 times i swear to god
        /// </summary>
        [DllImport("user32", EntryPoint = "DisableProcessWindowsGhosting")]
        public static extern void DisableProcessWindowGhosting();
#endif
    }
}