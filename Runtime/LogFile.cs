#if SERIAL_ENABLELOGFILE
using System;
using System.IO;
using UnityEngine;

namespace SerialPackage.Runtime
{
    class LogFile
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Awake()
        {
            if (PlayerPrefs.GetInt("SerialPackage_CreateLogFile", 0) == 1)
            {
                if (!File.Exists(Application.persistentDataPath + "/SerialPackage_CreateLogFile"))
                {
                    return;
                }
            }

            if (File.Exists(Application.temporaryCachePath + "/SerialPackage_Log.txt"))
            {
                File.Move(Application.temporaryCachePath + "/SerialPackage_Log.txt", Application.temporaryCachePath + "/SerialPackage_Log_archive" + UnityEngine.Random.Range(0, 999999) + ".txt");
            }

            File.Create(Application.temporaryCachePath + "/SerialPackage_Log.txt");

            Application.logMessageReceived += HandleLog;

            Debug.Log("(LogFile) CreateLogFile enabled.");
        }

        static void HandleLog(string logString, string stackTrace, LogType type)
        {
            File.AppendAllText(Application.temporaryCachePath + "/SerialPackage_Log.txt", "[" + type.ToString() + "] " + logString + Environment.NewLine + stackTrace);
        }
    }
}
#endif