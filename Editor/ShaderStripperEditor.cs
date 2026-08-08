using UnityEditor;

[CustomEditor(typeof(ShaderStripperSettings))]
public class ShaderStripperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Show warning if the current sss isn't in Assets/ShaderStripperSettings.asset
#if UNITY_2023_1_OR_NEWER
        if (!AssetDatabase.AssetPathExists(ShaderStripper.sssName))
#else
        if (AssetDatabase.GetMainAssetTypeAtPath(ShaderStripper.sssName) == null)
#endif        
        {
            EditorGUILayout.HelpBox("The current ShaderStripperSettings isn't at Assets/ShaderStripperSettings.asset. Shaders won't be stripped.", MessageType.Error);
        }
        
        base.OnInspectorGUI();
    }
}