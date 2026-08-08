using System;
using UnityEngine;

namespace SerialPackage.Edito
{
    [CreateAssetMenu(fileName = "ShaderSwitch", menuName = "Serial Package/ShaderSwitch", order = 0)]
    public class ShaderSwitchSO : ScriptableObject
    {
        public ShaderSwitch[] shaderSwitches;
    }

    [Serializable]
    public class ShaderSwitch
    {
        public Material material;
        public Shader PCShader;
        public Shader MobileShader;
    }
}