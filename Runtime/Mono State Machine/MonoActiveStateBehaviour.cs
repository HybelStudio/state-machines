using UnityEngine;

namespace Hybel.StateMachines
{
    /// <summary>
    /// Base class for all <see cref="MonoBehaviour"/>s that uses a <see cref="ActiveStateMachine{TState}"/>.
    /// <para>This automatically calls the <see cref="ActiveStateMachine{TState}.Tick()"/> method every frame.</para>
    /// <para>All states used by this <see cref="MonoActiveStateBehaviour"/> must derive from <see cref="MonoActiveStateBase"/> or <see cref="MonoActiveStateBase{TBehaviour}"/>.</para>
    /// </summary>
    public abstract class MonoActiveStateBehaviour : MonoBehaviour
    {
        private ActiveStateObject<IActiveState> _stateObject;

        /// <summary>
        /// Reference to the state machine that controls this behaviour.
        /// </summary>
        protected ActiveStateMachine<IActiveState> StateMachine => _stateObject.StateMachine;

        protected IActiveState CurrentState => StateMachine.CurrentState;

        private void Awake() => _stateObject = new ActiveStateObject<IActiveState>();

        /// <summary>
        /// Sets the starting <paramref name="state"/> of the state machine. Usually called in <c>Awake()</c> or <c>Start()</c>.
        /// </summary>
        protected void SetInitialState(IActiveState state) => StateMachine.SetState(state);

        private void Update() => StateMachine.Tick();

        /// <summary>
        /// Wrapper for <see cref="ActiveStateMachine{IActiveState}.Configure"/>.
        /// </summary>
        protected StateMachineConfigurer<IActiveState, Condition, ActiveStateMachine<IActiveState>> Configure() => StateMachine.Configure();
    }
}