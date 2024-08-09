using System;
using System.Collections.Generic;
using Omni.Core.Attributes;
using UnityEngine;
#if OMNI_RELEASE
using System.Runtime.CompilerServices;
#endif

namespace Omni.Core
{
    public sealed class NetworkIdentity : MonoBehaviour, IEquatable<NetworkIdentity>
    {
        internal string _prefabName;
        private readonly Dictionary<string, object> m_Services = new(); // (Service Name, Service Instance) exclusively to identity

        [SerializeField]
        [ReadOnly]
        private int m_Id;

        [SerializeField]
        [ReadOnly]
        private bool m_IsServer;

        [SerializeField]
        [ReadOnly]
        private bool m_IsLocalPlayer;

        public int IdentityId
        {
            get { return m_Id; }
            internal set { m_Id = value; }
        }

        /// <summary>
        /// Owner of this object. Only available on server, returns <c>LocalPeer</c> on client.
        /// </summary>
        public NetworkPeer Owner { get; internal set; }

        /// <summary>
        /// Indicates whether this object is obtained from the server or checked on the client.
        /// True if the object is obtained from the server, false if it is checked on the client.
        /// </summary>
        public bool IsServer
        {
            get { return m_IsServer; }
            internal set { m_IsServer = value; }
        }

        /// <summary>
        /// Indicates whether this object is owned by the local player.
        /// </summary>
        public bool IsLocalPlayer
        {
            get { return m_IsLocalPlayer; }
            internal set { m_IsLocalPlayer = value; }
        }

        /// <summary>
        /// Indicates whether this NetworkIdentity is registered.
        /// </summary>
        public bool IsRegistered
        {
            get { return Owner != null && m_Id != 0; }
        }

        /// <summary>
        /// Retrieves a service instance by its name from the service locator.
        /// Throws an exception if the service is not found or cannot be cast to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the service should be cast.</typeparam>
        /// <param name="serviceName">The name of the service to retrieve.</param>
        /// <returns>The service instance cast to the specified type.</returns>
        /// <exception cref="Exception">
        /// Thrown if the service is not found or cannot be cast to the specified type.
        /// </exception>
        public T Get<T>(string serviceName)
            where T : class
        {
            try
            {
                if (m_Services.TryGetValue(serviceName, out object service))
                {
#if OMNI_RELEASE
                    return Unsafe.As<T>(service);
#else
                    return (T)service;
#endif
                }
                else
                {
                    throw new Exception(
                        $"Could not find service with name: \"{serviceName}\" you must register the service before using it."
                    );
                }
            }
            catch (InvalidCastException)
            {
                throw new Exception(
                    $"Could not cast service with name: \"{serviceName}\" to type: \"{typeof(T)}\" check if you registered the service with the correct type."
                );
            }
        }

        /// <summary>
        /// Attempts to retrieve a service instance by its name from the service locator.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="serviceName">The name of the service to retrieve.</param>
        /// <param name="service">When this method returns, contains the service instance cast to the specified type if the service is found; otherwise, the default value for the type of the service parameter.</param>
        /// <returns>True if the service is found and successfully cast to the specified type; otherwise, false.</returns>
        public bool TryGet<T>(string serviceName, out T service)
            where T : class
        {
            service = default;
            if (m_Services.TryGetValue(serviceName, out object @obj))
            {
                if (@obj is T)
                {
                    service = Get<T>(serviceName);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Retrieves a service instance by its type name from the service locator.
        /// </summary>
        /// <typeparam name="T">The type to which the service should be cast.</typeparam>
        /// <returns>The service instance cast to the specified type.</returns>
        /// <exception cref="Exception">
        /// Thrown if the service is not found or cannot be cast to the specified type.
        /// </exception>
        public T Get<T>()
            where T : class
        {
            return Get<T>(typeof(T).Name);
        }

        /// <summary>
        /// Attempts to retrieve a service instance by its type from the service locator.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <param name="service">When this method returns, contains the service instance cast to the specified type if the service is found; otherwise, the default value for the type of the service parameter.</param>
        /// <returns>True if the service is found and successfully cast to the specified type; otherwise, false.</returns>
        public bool TryGet<T>(out T service)
            where T : class
        {
            service = default;
            string serviceName = typeof(T).Name;
            if (m_Services.TryGetValue(serviceName, out object @obj))
            {
                if (@obj is T)
                {
                    service = Get<T>(serviceName);
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Adds a new service instance to the service locator with a specified name.
        /// Throws an exception if a service with the same name already exists.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="service">The service instance to add.</param>
        /// <param name="serviceName">The name to associate with the service instance.</param>
        /// <exception cref="Exception">
        /// Thrown if a service with the specified name already exists.
        /// </exception>
        public void Register<T>(T service, string serviceName)
        {
            if (!m_Services.TryAdd(serviceName, service))
            {
                throw new Exception(
                    $"Could not add service with name: \"{serviceName}\" because it already exists."
                );
            }
        }

        /// <summary>
        /// Attempts to retrieve adds a new service instance to the service locator with a specified name.
        /// Throws an exception if a service with the same name already exists.
        /// </summary>
        /// <typeparam name="T">The type of the service to add.</typeparam>
        /// <param name="service">The service instance to add.</param>
        /// <param name="serviceName">The name to associate with the service instance.</param>
        /// <exception cref="Exception">
        /// Thrown if a service with the specified name already exists.
        /// </exception>
        public bool TryRegister<T>(T service, string serviceName)
        {
            return m_Services.TryAdd(serviceName, service);
        }

        /// <summary>
        /// Updates an existing service instance in the service locator with a specified name.
        /// Throws an exception if a service with the specified name does not exist.
        /// </summary>
        /// <typeparam name="T">The type of the service to update.</typeparam>
        /// <param name="service">The new service instance to associate with the specified name.</param>
        /// <param name="serviceName">The name associated with the service instance to update.</param>
        /// <exception cref="Exception">
        /// Thrown if a service with the specified name does not exist in the.
        /// </exception>
        public void UpdateService<T>(T service, string serviceName)
        {
            if (m_Services.ContainsKey(serviceName))
            {
                m_Services[serviceName] = service;
            }
            else
            {
                throw new Exception(
                    $"Could not update service with name: \"{serviceName}\" because it does not exist."
                );
            }
        }

        /// <summary>
        /// Attempts to retrieve updates an existing service instance in the service locator with a specified name.
        /// Throws an exception if a service with the specified name does not exist.
        /// </summary>
        /// <typeparam name="T">The type of the service to update.</typeparam>
        /// <param name="service">The new service instance to associate with the specified name.</param>
        /// <param name="serviceName">The name associated with the service instance to update.</param>
        /// <exception cref="Exception">
        /// Thrown if a service with the specified name does not exist in the.
        /// </exception>
        public bool TryUpdate<T>(T service, string serviceName)
        {
            if (m_Services.ContainsKey(serviceName))
            {
                m_Services[serviceName] = service;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes a service instance from the service locator by its name.
        /// </summary>
        /// <param name="serviceName">The name of the service to delete.</param>
        /// <returns>True if the service was successfully removed; otherwise, false.</returns>
        public bool Unregister(string serviceName)
        {
            return m_Services.Remove(serviceName);
        }

        /// <summary>
        /// Automatic instantiates a network identity on the client.
        /// </summary>
        /// <returns>The instantiated network identity.</returns>
        public void SpawnOnClient(SyncOptions options)
        {
            SpawnOnClient(
                options.Target,
                options.DeliveryMode,
                options.GroupId,
                options.CacheId,
                options.CacheMode,
                options.SequenceChannel
            );
        }

        /// <summary>
        /// Automatic instantiates a network identity on the client.
        /// </summary>
        /// <returns>The instantiated network identity.</returns>
        public void SpawnOnClient(
            Target target = Target.All,
            DeliveryMode deliveryMode = DeliveryMode.ReliableOrdered,
            int groupId = 0,
            int cacheId = 0,
            CacheMode cacheMode = CacheMode.None,
            byte sequenceChannel = 0
        )
        {
            if (!IsRegistered)
            {
                throw new Exception(
                    $"The game object '{name}' is not registered. Please register it first."
                );
            }

            if (!IsServer)
            {
                throw new Exception($"Only server can spawn the game object '{name}'.");
            }

            using var message = NetworkManager.Pool.Rent();
            message.WriteString(_prefabName);
            message.WriteIdentity(this);
            NetworkManager.Server.SendMessage(
                MessageType.Spawn,
                Owner,
                message,
                target,
                deliveryMode,
                groupId,
                cacheId,
                cacheMode,
                sequenceChannel
            );
        }

        /// <summary>
        /// Automatic destroys a network identity on the client.
        /// </summary>
        /// <returns>The instantiated network identity.</returns>
        public void Destroy(SyncOptions options)
        {
            Destroy(
                options.Target,
                options.DeliveryMode,
                options.GroupId,
                options.CacheId,
                options.CacheMode,
                options.SequenceChannel
            );
        }

        /// <summary>
        /// Automatic destroys a network identity on the client.
        /// </summary>
        /// <returns>The instantiated network identity.</returns>
        public void Destroy(
            Target target = Target.All,
            DeliveryMode deliveryMode = DeliveryMode.ReliableOrdered,
            int groupId = 0,
            int cacheId = 0,
            CacheMode cacheMode = CacheMode.None,
            byte sequenceChannel = 0
        )
        {
            if (!IsRegistered)
            {
                throw new Exception(
                    $"The game object '{name}' is not registered. Please register it first."
                );
            }

            if (!IsServer)
            {
                throw new Exception($"Only server can destroy the game object '{name}'.");
            }

            using var message = NetworkManager.Pool.Rent();
            message.Write(m_Id);
            NetworkManager.Server.SendMessage(
                MessageType.Destroy,
                Owner,
                message,
                target,
                deliveryMode,
                groupId,
                cacheId,
                cacheMode,
                sequenceChannel
            );

            NetworkHelper.Destroy(m_Id, IsServer);
        }

        // public override bool Equals(object obj)
        // {
        //     NetworkIdentity other = (NetworkIdentity)obj;
        //     return IdentityId == other.IdentityId;
        // }

        public override int GetHashCode()
        {
            return IdentityId.GetHashCode();
        }

        public bool Equals(NetworkIdentity other)
        {
            return IdentityId == other.IdentityId;
        }
    }
}
