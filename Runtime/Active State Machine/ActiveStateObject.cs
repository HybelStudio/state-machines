using UnityEngine;

namespace Hybel.StateMachines
{
    /// <summary>
    /// Base class for all <see cref="object"/>s that uses a <see cref="ActiveStateMachine{TState}"/>.
    /// <para>Make sure to call <see cref="Tick"/> anytime the <see cref="ActiveStateMachine{TState}"/> should check <see cref="Condition"/>s and update the current state.</para>
    /// </summary>
    /// <typeparam name="TState">Defines what type to use as the representation of a state.
    /// <para>Use <see cref="IActiveState"/> if you want to use any state.</para>
    /// </typeparam>
    public class ActiveStateObject<TState> where TState : IActiveState
    {
        private ActiveStateMachine<TState> _stateMachine;

        /// <summary>
        /// References the statemachine that controls this behaviour.
        /// </summary>
        public ActiveStateMachine<TState> StateMachine => _stateMachine;

        public TState CurrentState => StateMachine.CurrentState;

        public ActiveStateObject() => _stateMachine = new ActiveStateMachine<TState>();

        public StateMachineConfigurer<TState, Condition, ActiveStateMachine<TState>> Configure() => _stateMachine.Configure();

        public void SetInitialState(TState state) => StateMachine.SetState(state);

        /// <summary>
        /// Wrapper for the StateMachies Tick() method. Call this whenever you want the StateMachine to check conditions in order to change state or Tick() the current state.
        /// </summary>
        public void Tick() => StateMachine.Tick();

        /// <summary>
        /// Shorthand for adding a transition to the statemachine.
        /// </summary>
        public void At(TState from, TState to, Condition condition) =>
            _stateMachine.AddTransition(from, to, condition);

        /// <summary>
        /// Shorthand for adding a 'from any state' transition to the statemachine.
        /// </summary>
        public void AtAny(TState to, Condition condition) =>
            _stateMachine.AddAnyTransition(to, condition);
    }
}