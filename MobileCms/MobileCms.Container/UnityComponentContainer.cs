using System;
using System.Configuration;
using System.Data.Objects;
using System.IO;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MobileCms.Cache;

namespace MobileCms.Container
{
    public class UnityComponentContainer : IComponentContainer
    {
        private static UnityComponentContainer instance;
        private static IUnityContainer unityContainer;
        private static object syncLock = new object();
        private static ICache cache;

        private UnityComponentContainer()
        {
        }

        /// <summary>
        /// 返回 ComponentContainerByUnity 的一个实例
        /// </summary>
        /// <returns></returns>
        public static UnityComponentContainer GetComponentContainerByUnity()
        {
            if (instance == null)
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        InitializeContainer();
                        instance = new UnityComponentContainer();
                    }
                }
            }

            return instance;
        }

        private static void InitializeContainer()
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ComponentContainer.config");
            map.ExeConfigFilename = configPath;
            Configuration config
              = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            UnityConfigurationSection section
              = (UnityConfigurationSection)config.GetSection("unity");
            unityContainer = new UnityContainer();
            section.Containers.Default.Configure(unityContainer);
            cache = unityContainer.Resolve<ICache>();
        }

        /// <summary>
        /// Resolve an instance of the default requested type from the container.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of object to get from the container.</typeparam>
        /// <returns>The retrieved object.</returns>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of object to get from the container.</typeparam>
        /// <param name="name">Name of the object to retrieve.</param>
        /// <returns>The retrieved object.</returns>
        public T Resolve<T>(string name)
        {
            return (T)Resolve(typeof(T), name);
        }

        /// <summary>
        /// Resolve an instance of the default requested type from the container.
        /// </summary>
        /// <param name="t"><see cref="Type"/> of object to get from the container.</param>
        /// <returns>The retrieved object.</returns>
        public object Resolve(Type t)
        {
            return Resolve(t, null);
        }

        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <param name="t"><see cref="Type"/> of object to get from the container.</param>
        /// <param name="name">Name of the object to retrieve.</param>
        /// <returns>The retrieved object.</returns>
        public object Resolve(Type t, string name)
        {
            object result = cache.Get(t.FullName);
            if (result == null)
            {
                lock (syncLock)
                {
                    if (result == null)
                    {
                        result = unityContainer.Resolve(t, name);
                        if (result != null)
                        {
                            cache.Add(t.FullName, result, TimeSpan.FromMinutes(30));
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// ResolveObjectContext
        /// </summary>
        public T ResolveObjectContext<T>() where T : ObjectContext, new()
        {
            T result = new T();

            //result.SavingChanges += new EventHandler(context_SavingChanges);

            return result;
        }
    }
}
