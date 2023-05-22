
# State Machine Example

This example demonstrates how to use the `StateMachine` class provided by the `Metalbullz.StateMachine` namespace to manage states and transitions in a game object, specifically an enemy character.

This example provides a basic structure for implementing a state machine for an enemy character in a game. You can customize the state classes and condition functions to fit your specific game logic and requirements.

## Enemy Class

The `Enemy` class inherits from `MonoBehaviour` and represents the enemy object in the game.

### Initialization

In the `Start` method, an instance of the `StateMachine<Enemy>` is created, and the owner object (this) is passed to it. States and transitions are added to the state machine, and the initial state is set.

```csharp
private StateMachine<Enemy> _stateMachine;

private void Start()
{
    // Create an instance of the state machine and pass the owner object (this)
    _stateMachine = new StateMachine<Enemy>(this);

    // Add states to the state machine
    _stateMachine.AddState<IdleState>();
    _stateMachine.AddState<AttackState>();
    _stateMachine.AddState<ChaseState>();

    // Add transitions between states
    _stateMachine.AddTransition(new IdleState(), new AttackState(), CanAttack);
    _stateMachine.AddTransition(new IdleState(), new ChaseState(), CanChase);
    _stateMachine.AddTransition(new AttackState(), new ChaseState(), CanChase);
    _stateMachine.AddTransition(new ChaseState(), new AttackState(), CanAttack);

    // Set the initial state
    _stateMachine.ChangeState<IdleState>();
}
```
### Update

In the `Update` method, the `Tick` method of the state machine is called on each frame to execute the update logic of the current state.

```csharp
private void Update()
{
    // Update the state machine on each frame
    _stateMachine.Tick();
}
```

### Condition Functions
Condition functions are defined in the `Enemy` class to determine the conditions for transitions between states. In this example, `CanAttack` and `CanChase` are placeholder functions that return true, but you should replace them with your actual condition logic.

```csharp
private bool CanAttack()
{
    // Condition logic to check if the enemy can attack
    return true;
}

private bool CanChase()
{
    // Condition logic to check if the enemy can chase
    return true;
}
```

### Custom States
The example includes three custom state classes: `IdleState`, `AttackState`, and `ChaseState`. These classes inherit from the State abstract class provided by the `Metalbullz.StateMachine` namespace and implement the required methods: `OnEnter`, `OnExit`, and `OnUpdate`. You can customize these methods with the specific logic for each state.

```csharp
public class IdleState : State
{
    public override void OnEnter()
    {
        // Logic when entering the idle state
    }

    public override void OnExit()
    {
        // Logic when exiting the idle state
    }

    public override void OnUpdate()
    {
        // Logic during each update tick of the idle state
    }
}

public class AttackState : State
{
    public override void OnEnter()
    {
        // Logic when entering the attack state
    }

    public override void OnExit()
    {
        // Logic when exiting the attack state
    }

    public override void OnUpdate()
    {
        // Logic during each update tick of the attack state
    }
}

public class ChaseState : State
{
    public override void OnEnter()
    {
        // Logic when entering the chase state
    }

    public override void OnExit()
    {
        // Logic when exiting the chase state
    }

    public override void OnUpdate()
    {
        // Logic during each update tick of the chase state
    }
}
```

Each state class implements the `OnEnter` method, which is called when the state is entered, the `OnExit` method, which is called when the state is exited, and the `OnUpdate` method, which is called during each update tick of the state.

## The Final Code

```csharp
using Metalbullz.StateMachine;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private StateMachine<Enemy> _stateMachine;

    private void Start()
    {
        // Create an instance of the state machine and pass the owner object (this)
        _stateMachine = new StateMachine<Enemy>(this);

        // Add states to the state machine
        _stateMachine.AddState<IdleState>();
        _stateMachine.AddState<AttackState>();
        _stateMachine.AddState<ChaseState>();

        // Add transitions between states
        _stateMachine.AddTransition(new IdleState(), new AttackState(), CanAttack);
        _stateMachine.AddTransition(new IdleState(), new ChaseState(), CanChase);
        _stateMachine.AddTransition(new AttackState(), new ChaseState(), CanChase);
        _stateMachine.AddTransition(new ChaseState(), new AttackState(), CanAttack);

        // Set the initial state
        _stateMachine.ChangeState<IdleState>();
    }

    private void Update()
    {
        // Update the state machine on each frame
        _stateMachine.Tick();
    }

    // Example condition functions for transitions
    private bool CanAttack()
    {
        // Condition logic to check if the enemy can attack
        return true;
    }

    private bool CanChase()
    {
        // Condition logic to check if the enemy can chase
        return true;
    }
}

// Custom states for the enemy
public class IdleState : State
{
    public override void OnEnter()
    {
        // Logic when entering the idle state
    }

    public override void OnExit()
    {
        // Logic when exiting the idle state
    }

    public override void OnUpdate()
    {
        // Logic during each update tick of the idle state
    }
}

public class AttackState : State
{
    public override void OnEnter()
    {
        // Logic when entering the attack state
    }

    public override void OnExit()
    {
        // Logic when exiting the attack state
    }

    public override void OnUpdate()
    {
        // Logic during each update tick of the attack state
    }
}

public class ChaseState : State
{
    public override void OnEnter()
    {
        // Logic when entering the chase state
    }

    public override void OnExit()
    {
        // Logic when exiting the chase state
    }

    public override void OnUpdate()
    {
        // Logic during each update tick of the chase state
    }
}
```

Make sure to replace the placeholder logic and condition functions with your actual implementation.

I hope this manual page helps you understand and use the state machine example effectively!
