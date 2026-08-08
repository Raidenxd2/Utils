using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShaderStripperSettings", menuName = "Shader Stripper Settings", order = 0)]
public class ShaderStripperSettings : ScriptableObject
{
    [Tooltip("Logs shader stripping information")]
    public bool DebugMode;
    
    public List<string> ShaderKeywordNames;
    public List<string> ShadersToStrip;
}