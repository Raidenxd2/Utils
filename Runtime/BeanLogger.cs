using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace SerialPackage.Runtime
{
    public static class BeanLogger
    {
        public static bool VerboseLogging;
        
        public static void Log(string text, Object source)
        {
#if UNITY_WEBGL
            Debug.Log(text);
#else
            string logString;
            if (VerboseLogging)
            {
                StackFrame frame = new(1, true);
                string method = frame.GetMethod().Name;
                string fileName = frame.GetFileName();
                int lineNumber = frame.GetFileLineNumber();
                logString = "[" + Time.realtimeSinceStartup + "] [LOG] [" + fileName + ":" + lineNumber + "] (" + method + ") " + text;
            }
            else
            {
                logString = "[LOG] " + text;
            }
            Debug.Log(logString, source);
#endif
        }

        public static void LogError(string text, Object source)
        {
#if UNITY_WEBGL
            Debug.LogError(text);
#else
            string logString;
            if (VerboseLogging)
            {
                StackFrame frame = new(1, true);
                string method = frame.GetMethod().Name;
                string fileName = frame.GetFileName();
                int lineNumber = frame.GetFileLineNumber();
                logString = "[" + Time.realtimeSinceStartup + "] [ERROR] [" + fileName + ":" + lineNumber + "] (" + method + ") " + text;
            }
            else
            {
                logString = "[ERROR] " + text;
            }
            Debug.LogError(logString, source);
#endif
        }

        public static void LogWarning(string text, Object source)
        {
#if UNITY_WEBGL
            Debug.LogWarning(text);
#else
            string logString;
            if (VerboseLogging)
            {
                StackFrame frame = new(1, true);
                string method = frame.GetMethod().Name;
                string fileName = frame.GetFileName();
                int lineNumber = frame.GetFileLineNumber();
                logString = "[" + Time.realtimeSinceStartup + "] [WARNING] [" + fileName + ":" + lineNumber + "] (" + method + ") " + text;
            }
            else
            {
                logString = "[WARNING] " + text;
            }
            Debug.LogWarning(logString, source);
#endif
        }
    }
}