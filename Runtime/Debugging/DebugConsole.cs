#if UNITY_EDITOR || DEBUG || UTILSPACKAGE_DEBUG
using TMPro;
using UnityEngine;

namespace raiden.utils
{
    public class DebugConsole : MonoBehaviour
    {
        [SerializeField] private GameObject DebugLine;
        [SerializeField] private Transform DebugLineRoot;

        [SerializeField] private Color ErrorColor;
        [SerializeField] private Color WarningColor;

        private bool ShowStacktraceForLog;

        private void Start()
        {
            Application.logMessageReceived += OnLog;
        }
        
        private void OnLog(string logString, string stackTrace, LogType type)
        {
            TMP_Text DebugLineText = Instantiate(DebugLine, DebugLineRoot).GetComponent<TMP_Text>();

            switch (type)
            {
                case LogType.Error:
                case LogType.Exception:
                    DebugLineText.color = ErrorColor;
                    break;
                case LogType.Warning:
                    DebugLineText.color = WarningColor;
                    break;
            }

            if (type == LogType.Log)
            {
                if (ShowStacktraceForLog)
                {
                    DebugLineText.text = logString + "\n" + stackTrace;
                }
                else
                {
                    DebugLineText.text = logString;
                }
            }
            else
            {
                DebugLineText.text = logString + "\n" + stackTrace;
            }
        }

        public void Clear()
        {
            foreach (Transform log in DebugLineRoot)
            {
                Destroy(log.gameObject);
            }
        }

        public void ChangeShowStacktraceForLog(bool val)
        {
            ShowStacktraceForLog = val;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLog;
        }
    }
}
#endif