namespace Hybel.StateMachines
{
    /// <summary>
    /// Base class for all active states.
    /// And active state is used in an <see cref="ActiveStateMachine{TState}"/>.
    /// </summary>
    /// <typeparam name="TStateEntity">Use this to restricts the usage of the state to only one type of object or any of its derived types.</typeparam>
    public abstract class ActiveStateBase : IActiveState
    {
        private readonly object _stateObject;

        /// <summary>
        /// References the entity itself.
        /// </summary>
        protected object StateObject => _stateObject;

        /// <summary>
        /// Initializes base state fields.
        /// </summary>
        public ActiveStateBase(object entity) => _stateObject = entity;

        public abstract void Tick();

        public abstract void OnEnterState();

        public abstract void OnExitState();
    }

    /// <summary>
    /// Base class for all active states.
    /// And active state is used in an <see cref="ActiveStateMachine{TState}"/>.
    /// </summary>
    /// <typeparam name="TStateEntity">Use this to restricts the usage of the state to only one type of object or any of its derived types.</typeparam>
    public abstract class ActiveStateBase<TStateObject> : ActiveStateBase
    {
        protected new TStateObject StateObject => (TStateObject)base.StateObject;

        /// <summary>
        /// Initializes base state fields.
        /// </summary>
        public ActiveStateBase(TStateObject entity) : base(entity) { }

        public override abstract void Tick();

        public override abstract void OnEnterState();

        public override abstract void OnExitState();
    }
}