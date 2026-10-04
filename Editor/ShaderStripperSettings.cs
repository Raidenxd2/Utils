using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShaderStripperSettings", menuName = "Shader Stripper Settings", order = 0)]
public class ShaderStripperSettings : ScriptableObject
{
    [Tooltip("Logs shader stripping information")]
    public bool DebugMode;
    
    public List<string> ShaderKeywordNames;
    public List<string> ShadersToStrip;
    public List<string> PassesToStrip;
    public List<ShaderStripperShader> Shaders;
}

[Serializable]
public class ShaderStripperShader
{
    public string ShaderName;
    public List<string> ShaderKeywordNames;
    public List<string> PassesToStrip;
}