namespace Metalbullz.StateMachine
{
    /// <summary>
    /// Represents a state in a state machine.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Gets or sets the owner object associated with the state.
        /// </summary>
        protected object Owner { get; private set; }

        /// <summary>
        /// Sets the owner object for the state.
        /// </summary>
        /// <param name="owner">The owner object.</param>
        public void SetOwner(object owner)
        {
            Owner = owner;
        }

        /// <summary>
        /// Called when entering the state.
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// Called when exiting the state.
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// Called during each update tick of the state machine.
        /// </summary>
        public abstract void OnUpdate();
    }
}
