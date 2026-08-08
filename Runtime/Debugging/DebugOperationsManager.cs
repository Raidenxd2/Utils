#if UNITY_EDITOR || DEBUG || UTILSPACKAGE_DEBUG
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace raiden.utils
{
    public static class DebugOperationsManager
    {
        public static List<MethodInfo> Methods;
        public static List<string> MethodNames;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            Methods = new();
            MethodNames = new();
            
            foreach (Assembly assembly in GetAllAssemblies())
            {
                string assemblyName = assembly.GetName().Name;

                if (assemblyName.Equals("KillItMyself.Runtime") || assemblyName.Equals("CC2") || assemblyName.Equals("com.raiden.utils.runtime"))
                {
                    List<DebugOperationAttribute> methods = new List<DebugOperationAttribute>();
                    foreach( Type type in assembly.GetExportedTypes() )
                    {
                        foreach( MethodInfo method in type.GetMethods( BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly ) )
                        {
                            foreach( DebugOperationAttribute consoleAttribute in method.GetCustomAttributes( typeof(DebugOperationAttribute), false ) )
                            {
                                consoleAttribute.SetMethod(method);
                                methods.Add(consoleAttribute);
                            }
                        }
                    }

                    for (int i = 0; i < methods.Count; i++)
                    {
                        methods[i].Load();
                    }
                }
            }
        }
        
        public static IReadOnlyList<Assembly> GetAllAssemblies()
        {
#if UNITY_6000_4_OR_NEWER
            return UnityEngine.Assemblies.CurrentAssemblies.GetLoadedAssemblies();
#elif UNITY_EDITOR || !NETFX_CORE
            return AppDomain.CurrentDomain.GetAssemblies();
#endif
        }
    }
}
#endif