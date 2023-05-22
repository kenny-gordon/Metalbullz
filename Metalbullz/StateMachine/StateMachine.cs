using System;
using System.Collections.Generic;
using UnityEngine;

namespace Metalbullz.StateMachine
{
    /// <summary>
    /// Represents a state machine that manages states and transitions between them.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner object that owns the state machine.</typeparam>
    public class StateMachine<TOwner>
    {
        private readonly TOwner _owner;
        private readonly Dictionary<Type, Func<State>> _stateFactories;
        private readonly Dictionary<Type, List<Transition>> _transitions;
        private State _currentState;
        private bool _isInitialStateSet;

        /// <summary>
        /// Initializes a new instance of the StateMachine class.
        /// </summary>
        /// <param name="owner">The owner object that owns the state machine.</param>
        public StateMachine(TOwner owner)
        {
            _owner = owner;
            _stateFactories = new Dictionary<Type, Func<State>>();
            _transitions = new Dictionary<Type, List<Transition>>();
            _currentState = null;
            _isInitialStateSet = false;
        }

        /// <summary>
        /// Adds a new state to the state machine.
        /// </summary>
        /// <typeparam name="TState">The type of the state to add.</typeparam>
        /// <returns>The added state.</returns>
        public State AddState<TState>() where TState : State, new()
        {
            Type stateType = typeof(TState);
            if (!_stateFactories.ContainsKey(stateType))
            {
                _stateFactories[stateType] = () =>
                {
                    var state = new TState();
                    state.SetOwner(_owner);
                    return state;
                };
                if (!_isInitialStateSet)
                {
                    _currentState = _stateFactories[stateType].Invoke();
                    _currentState.OnEnter();
                    _isInitialStateSet = true;
                }
                return _currentState;
            }
            else
            {
                Debug.LogError($"State of type '{stateType.Name}' already exists in the state machine.");
                return null;
            }
        }

        /// <summary>
        /// Removes a state from the state machine.
        /// </summary>
        /// <typeparam name="TState">The type of the state to remove.</typeparam>
        public void RemoveState<TState>() where TState : State
        {
            Type stateType = typeof(TState);
            if (_stateFactories.ContainsKey(stateType))
            {
                _stateFactories.Remove(stateType);
                _transitions.Remove(stateType);
            }
            else
            {
                Debug.LogWarning($"State '{stateType.Name}' does not exist in the state machine.");
            }
        }

        /// <summary>
        /// Adds a transition between two states in the state machine.
        /// </summary>
        /// <param name="fromState">The state from which the transition originates.</param>
        /// <param name="toState">The state to transition to.</param>
        /// <param name="condition">The condition that must be satisfied for the transition to occur.</param>
        public void AddTransition(State fromState, State toState, Func<bool> condition)
        {
            Type fromStateType = fromState.GetType();
            Type toStateType = toState.GetType();
            Transition transition = new Transition(toState, condition);

            if (!_transitions.ContainsKey(fromStateType))
            {
                _transitions[fromStateType] = new List<Transition>();
            }

            _transitions[fromStateType].Add(transition);
        }

        /// <summary>
        /// Adds a transition from any state to a specific state in the state machine.
        /// </summary>
        /// <param name="toState">The state to transition to.</param>
        /// <param name="condition">The condition that must be satisfied for the transition to occur.</param>
        public void AddAnyStateTransition(State toState, Func<bool> condition)
        {
            foreach (var stateTransitions in _transitions.Values)
            {
                stateTransitions.Add(new Transition(toState, condition));
            }
        }

        /// <summary>
        /// Removes a transition between two states in the state machine.
        /// </summary>
        /// <param name="fromState">The state from which the transition originates.</param>
        /// <param name="toState">The state to transition to.</param>
        public void RemoveTransition(State fromState, State toState)
        {
            Type fromStateType = fromState.GetType();
            Type toStateType = toState.GetType();

            if (_transitions.ContainsKey(fromStateType))
            {
                List<Transition> stateTransitions = _transitions[fromStateType];
                stateTransitions.RemoveAll(t => t.To.GetType() == toStateType);
            }
        }

        /// <summary>
        /// Changes the current state of the state machine to the specified state.
        /// </summary>
        /// <typeparam name="TToState">The type of the state to change to.</typeparam>
        public void ChangeState<TToState>() where TToState : State
        {
            Type toStateType = typeof(TToState);
            if (_stateFactories.ContainsKey(toStateType))
            {
                var toState = _stateFactories[toStateType].Invoke();
                _currentState.OnExit();
                _currentState = toState;
                _currentState.OnEnter();
            }
            else
            {
                Debug.LogError($"State '{toStateType.Name}' does not exist in the state machine.");
            }
        }

        /// <summary>
        /// Executes the update logic of the current state in the state machine.
        /// </summary>
        public void Tick()
        {
            if (_currentState == null)
            {
                Debug.LogError("Current state is not set in the state machine.");
                return;
            }

            // Check transitions from the current state
            List<Transition> transitions;
            if (_transitions.TryGetValue(_currentState.GetType(), out transitions))
            {
                foreach (var transition in transitions)
                {
                    if (transition.Condition())
                    {
                        _currentState.OnExit();
                        _currentState = transition.To;
                        _currentState.OnEnter();
                        return;
                    }
                }
            }

            // No transition occurred, update the current state
            _currentState.OnUpdate();
        }
    }
}
