# Enemy Character Event Management Example

This example provides a basic structure for implementing event logic for an enemy character in a game using the `EventManager` class.

## EventManager Class

The `EventManager` class is responsible for managing events and providing a convenient way to add and remove event listeners. It includes the following methods:

- `AddListener(eventName, listener)`: Adds a listener to the specified event.
- `RemoveListener(eventName, listener)`: Removes a listener from the specified event.
- `Trigger(eventName)`: Triggers the specified event.
- `AddListener<T>(eventName, listener)`: Adds a listener to the specified typed event.
- `RemoveListener<T>(eventName, listener)`: Removes a listener from the specified typed event.
- `Trigger<T>(eventName, data)`: Triggers the specified typed event with the provided data.

## Usage Example

```csharp
using Metalbullz.Managers;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    // ...

    private void Start()
    {
        // Add listeners to events
        EventManager.AddListener("AttackEvent", Attack);
        EventManager.AddListener<int>("DamageEvent", TakeDamage);
    }

    private void Attack()
    {
        // Logic for enemy attack
    }

    private void TakeDamage(int amount)
    {
        // Logic for enemy taking damage
    }

    private void OnDestroy()
    {
        // Remove listeners when the enemy object is destroyed
        EventManager.RemoveListener("AttackEvent", Attack);
        EventManager.RemoveListener<int>("DamageEvent", TakeDamage);
    }
}

```

In this example, the `Enemy` class adds listeners to the "AttackEvent" and "DamageEvent" events using the `AddListener` method. The corresponding event handling methods, `Attack` and `TakeDamage`, are implemented within the `Enemy` class. The `RemoveListener` method is called in the `OnDestroy` method to remove the listeners when the enemy object is destroyed.

You can customize the event names, data types, and event handling methods based on your game logic and requirements.

This example provides a basic structure for managing events in an enemy character. Feel free to modify and extend it to suit your specific needs.

Remember to include the necessary namespaces (`Metalbullz.Managers` and `UnityEngine.Events`) and ensure that the `EventManager` class is available in your project.
