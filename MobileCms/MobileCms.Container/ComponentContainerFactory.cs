using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using CuttingEdge.Conditions;

namespace MobileCms.Container
{
    public static class ComponentContainerFactory
    {
        private static IComponentContainer componentContainer = null;

        static ComponentContainerFactory()
        {
            componentContainer = GetComponentContainer();
        }

        /// <summary>
        /// 创建 IComponentContainer 组件
        /// </summary>
        public static IComponentContainer CreateContainer()
        {
            if (componentContainer != null)
            {
                return componentContainer;
            }
            else
            {
                componentContainer = GetComponentContainer();
                return componentContainer;
            }
        }

        private static IComponentContainer GetComponentContainer()
        {
            IComponentContainer result = null;
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ComponentContainer.config");
            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = configPath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
            string container = config.AppSettings.Settings["container"].Value;
            if (container.Equals("unity"))
            {
                Assembly assembly = Assembly.Load("MobileCms.Container");
                Type type = assembly.GetType("MobileCms.Container.UnityComponentContainer");
                result = (IComponentContainer)type.InvokeMember("GetComponentContainerByUnity", BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, null);
            }
            else
            {
                throw new NotImplementedException();
            }

            Condition.Ensures(result).IsNotNull();

            return result;
        }
    }
}
