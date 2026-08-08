#if UNITY_EDITOR || DEBUG || UTILSPACKAGE_DEBUG
using System;
using System.Reflection;

namespace raiden.utils
{
    [AttributeUsage(AttributeTargets.Method)]
    public class DebugOperationAttribute : Attribute
    {
        public MethodInfo Method;
        public string name;

        public void SetMethod(MethodInfo method)
        {
            Method = method;
        }

        public void Load()
        {
            DebugOperationsManager.Methods.Add(Method);
            DebugOperationsManager.MethodNames.Add(name);
        }
        
        public DebugOperationAttribute(string name)
        {
            this.name = name;
        }
    }
}
#endif