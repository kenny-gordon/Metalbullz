using Metalbullz.Core;
using Metalbullz.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Metalbullz.Managers
{
    /// <summary>
    /// Manages events and provides a convenient way to add and remove event listeners.
    /// </summary>
    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<string, UnityEvent> _events;
        private Dictionary<string, UnityEventBase> _typedEvents;

        /// <summary>
        /// Initializes the EventManager instance.
        /// </summary>
        protected override void Initialize()
        {
            _events = new Dictionary<string, UnityEvent>();
            _typedEvents = new Dictionary<string, UnityEventBase>();
        }

        /// <summary>
        /// Adds a listener to the specified event.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="listener">The listener to add.</param>
        public static void AddListener(string eventName, UnityAction listener)
        {
            if (Instance._events.TryGetValue(eventName, out UnityEvent unityEvent))
            {
                unityEvent.AddListener(listener);
            }
            else
            {
                unityEvent = new UnityEvent();
                unityEvent.AddListener(listener);
                Instance._events.Add(eventName, unityEvent);
            }
        }

        /// <summary>
        /// Removes a listener from the specified event.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="listener">The listener to remove.</param>
        public static void RemoveListener(string eventName, UnityAction listener)
        {
            if (Instance._events.TryGetValue(eventName, out UnityEvent unityEvent))
            {
                unityEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Triggers the specified event.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        public static void Trigger(string eventName)
        {
            if (Instance._events.TryGetValue(eventName, out UnityEvent unityEvent))
            {
                unityEvent.Invoke();
            }
        }

        /// <summary>
        /// Adds a listener to the specified typed event.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="listener">The listener to add.</param>
        public static void AddListener<T>(string eventName, UnityAction<T> listener)
        {
            if (Instance._typedEvents.TryGetValue(eventName, out UnityEventBase typedEvent))
            {
                if (typedEvent is TypedEvent<T> castEvent)
                {
                    castEvent.AddListener(listener);
                }
                else
                {
                    Debug.LogError($"Cannot add listener to TypedEvent with a different data type for event '{eventName}'.");
                }
            }
            else
            {
                var newTypedEvent = new TypedEvent<T>();
                newTypedEvent.AddListener(listener);
                Instance._typedEvents.Add(eventName, newTypedEvent);
            }
        }

        /// <summary>
        /// Removes a listener from the specified typed event.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="listener">The listener to remove.</param>
        public static void RemoveListener<T>(string eventName, UnityAction<T> listener)
        {
            if (Instance._typedEvents.TryGetValue(eventName, out UnityEventBase typedEvent))
            {
                if (typedEvent is TypedEvent<T> castEvent)
                {
                    castEvent.RemoveListener(listener);
                }
                else
                {
                    Debug.LogError($"Cannot remove listener from TypedEvent with a different data type for event '{eventName}'.");
                }
            }
        }

        /// <summary>
        /// Triggers the specified typed event with the provided data.
        /// </summary>
        /// <typeparam name="T">The type of the event data.</typeparam>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="data">The data to pass to the event listeners.</param>
        public static void Trigger<T>(string eventName, T data)
        {
            if (Instance._typedEvents.TryGetValue(eventName, out UnityEventBase typedEvent))
            {
                if (typedEvent is TypedEvent<T> castEvent)
                {
                    castEvent.Invoke(data);
                }
                else
                {
                    Debug.LogError($"Cannot trigger TypedEvent with a different data type for event '{eventName}'.");
                }
            }
        }
    }
}
