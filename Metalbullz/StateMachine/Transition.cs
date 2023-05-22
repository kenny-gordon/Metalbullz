namespace Metalbullz.StateMachine
{
    /// <summary>
    /// Represents a transition between states in a state machine.
    /// </summary>
    public class Transition
    {
        /// <summary>
        /// Gets the condition function for the transition.
        /// </summary>
        public Func<bool> Condition { get; }

        /// <summary>
        /// Gets the target state of the transition.
        /// </summary>
        public State To { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transition"/> class with the specified target state and condition.
        /// </summary>
        /// <param name="to">The target state of the transition.</param>
        /// <param name="condition">The condition function that determines if the transition should occur.</param>
        public Transition(State to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }
}
