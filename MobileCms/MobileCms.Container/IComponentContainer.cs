using System;
using System.Data.Objects;

namespace MobileCms.Container
{
    public interface IComponentContainer
    {
        #region Resolve overloads

        /// <summary>
        /// Resolve an instance of the default requested type from the container.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of object to get from the container.</typeparam>
        /// <returns>The retrieved object.</returns>
        T Resolve<T>();

        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> of object to get from the container.</typeparam>
        /// <param name="name">Name of the object to retrieve.</param>
        /// <returns>The retrieved object.</returns>
        T Resolve<T>(string name);

        /// <summary>
        /// Resolve an instance of the default requested type from the container.
        /// </summary>
        /// <param name="t"><see cref="Type"/> of object to get from the container.</param>
        /// <returns>The retrieved object.</returns>
        object Resolve(Type t);

        /// <summary>
        /// Resolve an instance of the requested type with the given name from the container.
        /// </summary>
        /// <param name="t"><see cref="Type"/> of object to get from the container.</param>
        /// <param name="name">Name of the object to retrieve.</param>
        /// <returns>The retrieved object.</returns>
        object Resolve(Type t, string name);

        /// <summary>
        /// ResolveObjectContext
        /// </summary>
        T ResolveObjectContext<T>() where T : ObjectContext, new();

        #endregion
    }
}
