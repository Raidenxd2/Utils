using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderStripper : IPreprocessShaders
{
    List<ShaderKeyword> m_KeywordsToStrip;
    
    public const string sssName = "Assets/ShaderStripperSettings.asset";

    public ShaderStripper()
    {
        // If sss doesn't exist don't do anything
#if UNITY_2023_1_OR_NEWER
        if (!AssetDatabase.AssetPathExists(sssName))
#else
        if (AssetDatabase.GetMainAssetTypeAtPath(sssName) == null)
#endif        
        {
            return;
        }
        
        ShaderStripperSettings sss = AssetDatabase.LoadAssetAtPath<ShaderStripperSettings>(sssName);
        
        // Adds all of the keywords specified in sss to m_KeywordsToStrip
        m_KeywordsToStrip = new();
        foreach (var keyword in sss.ShaderKeywordNames)
        {
            m_KeywordsToStrip.Add(new(keyword));
        }
    }

    // Use callbackOrder to set when Unity calls this shader preprocessor. Unity starts with the preprocessor that has the lowest callbackOrder value.
    public int callbackOrder { get { return 0; } }

    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
    {
        // If sss doesn't exist don't do anything
#if UNITY_6000_0_OR_NEWER
        if (!AssetDatabase.AssetPathExists(sssName))
#else
        if (AssetDatabase.GetMainAssetTypeAtPath(sssName) == null)
#endif
        {
            return;
        }
        
        ShaderStripperSettings sss = AssetDatabase.LoadAssetAtPath<ShaderStripperSettings>(sssName);
        
        // Strips the entire shader if shader.name is in sss.ShadersToStrip
        foreach (var sShader in sss.ShadersToStrip)
        {
            if (shader.name.StartsWith(sShader))
            {
                if (sss.DebugMode)
                {
                    Debug.Log("(Shader Stripper) Stripping shader " + shader.name);
                }
                
                data.Clear();
            }
        }
        
        // Strips keywords in sss.ShaderKeywordNames
        foreach (ShaderKeyword item in m_KeywordsToStrip)
        {
            for (int i = 0; i < data.Count; ++i)
            {
                if (data[i].shaderKeywordSet.IsEnabled(item))
                {
                    if (sss.DebugMode)
                    {
                        Debug.Log("(Shader Stripper) Stripping keyword " + item.name + " of shader " + shader.name);
                    }
                    
                    data.RemoveAt(i);
                    --i;
                }
            }
        }
    }
}