using UnityEngine.Events;

namespace Metalbullz.Events
{
    /// <summary>
    /// Represents a generic event with a specific data type that can be used with the <see cref="EventManager"/>.
    /// </summary>
    /// <typeparam name="T">The data type of the event.</typeparam>
    /// <remarks>
    /// This class inherits from <see cref="UnityEvent{T}"/> and provides a typed event implementation.
    /// </remarks>
    [System.Serializable]
    public class TypedEvent<T> : UnityEvent<T> { }
}
