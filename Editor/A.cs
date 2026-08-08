using UnityEditor;
using UnityEditor.Analytics;

public class A : Editor
{
    [InitializeOnLoadMethod]
    public static void B()
    {
        EditorAnalytics.enabled = false;
        EditorAnalytics.recordEventsEnabled = false;
        AnalyticsSettings.enabled = false;
        AnalyticsSettings.initializeOnStartup = false;
        AnalyticsSettings.packageRequiringCoreStatsPresent = false;
        PerformanceReportingSettings.enabled = false;

        EditorPrefs.SetBool("EnableEditorAnalytics", false);
    }
}