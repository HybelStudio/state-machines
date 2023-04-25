using UnityEngine;

namespace Hybel.StateMachines
{
    /// <summary>
    /// Base class for all <see cref="object"/>s that uses a <see cref="ActiveStateMachine{TState}"/>.
    /// </summary>
    /// <typeparam name="TState">Defines what type to use as the representation of a state.</typeparam>
    /// <typeparam name="TTrigger">Defines what type to use as the representation of a trigger.
    /// <para>Triggers are used when you wish to change state using <see cref="Fire(TTrigger)"/>.</para>
    /// </typeparam>
    public abstract class PassiveStateObject<TState, TTrigger>
    {
        private PassiveStateMachine<TState, TTrigger> _stateMachine;

        private bool _stateSet;

        /// <summary>
        /// References the statemachine that controls this behaviour.
        /// </summary>
        public PassiveStateMachine<TState, TTrigger> StateMachine => _stateMachine;

        public TState CurrentState => StateMachine.CurrentState;

        public PassiveStateObject()
        {
            _stateMachine = new PassiveStateMachine<TState, TTrigger>();
            _stateMachine.OnEnterState += OnEnterState;
            _stateMachine.OnExitState += OnExitState;
            _stateSet = false;
        }

        public PassiveStateObject(TState initialState)
        {
            _stateMachine = new PassiveStateMachine<TState, TTrigger>(initialState);
            _stateMachine.OnEnterState += OnEnterState;
            _stateMachine.OnExitState += OnExitState;
            _stateSet = false;
        }

        /// <summary>
        /// Called whenever the state machine enters a new state.
        /// <para>Execute functionality connected to entering the state here.</para>
        /// </summary>
        /// <param name="state">The state which is being entered.</param>
        protected abstract void OnEnterState(TState state);

        /// <summary>
        /// Called whenever the state machine exits a state.
        /// <para>Execute functionality connected to exiting the state here.</para>
        /// </summary>
        /// <param name="state">The state which is being exited.</param>
        protected abstract void OnExitState(TState state);

        /// <summary>
        /// Explicity sets the state of the state machine once. This is required for the state machine to work.
        /// </summary>
        /// <param name="state">The state in which to enter.</param>
        public void SetInitialState(TState state)
        {
            if (_stateSet == true)
            {
#if CLOGGER
                this.LogError("State Machines", "State machine already has initial state.");
#elif UNITY_EDITOR
                Debug.LogError("State machine already has initial state.");
#endif
            }

            _stateSet = true;
            StateMachine.SetState(state);
        }

        /// <summary>
        /// Wrapper for <see cref="PassiveStateMachine{TState, TTrigger}.Configure"/>.
        /// </summary>
        public StateMachineConfigurer<TState, TTrigger, PassiveStateMachine<TState, TTrigger>> Configure() => _stateMachine.Configure();

        /// <summary>
        /// Wrapper for the StateMachies Fire() method. Call this whenever you want to give the StateMachine an input.
        /// </summary>
        public void Fire(TTrigger trigger) => StateMachine.Fire(trigger);
    }
}