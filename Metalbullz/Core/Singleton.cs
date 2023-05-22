using UnityEngine;

namespace Metalbullz.Core
{
    /// <summary>
    /// A base class for implementing singleton MonoBehaviours.
    /// </summary>
    /// <typeparam name="T">The type of the singleton instance.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool _isInitialized = false;
        private static T _instance;

        /// <summary>
        /// Gets the instance of the singleton.
        /// </summary>
        /// <value>
        /// The instance of the singleton.
        /// </value>
        public static T Instance
        {
            get
            {
                if (!_isInitialized)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        Debug.LogError($"There needs to be an active {typeof(T)} script on a GameObject in your scene.");
                    }
                    else
                    {
                        (_instance as Singleton<T>).Initialize();
                        _isInitialized = true;
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initializes the singleton instance.
        /// </summary>
        protected virtual void Initialize() { }
    }
}
