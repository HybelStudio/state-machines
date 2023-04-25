namespace Hybel.StateMachines
{
    /// <summary>
    /// Base class for all passive states.
    /// And active state is used in an <see cref="PassiveStateMachine{TState, TTrigger}"/>.
    /// </summary>
    /// <typeparam name="TController">
    /// Pass in the type of object that controls the <see cref="PassiveStateMachine{TState, TTrigger}"/>.
    /// <para>This is useful for localizing your states' behaviour in a single place (the controller) so you don't have to pass a billion references into the states themselves.</para>
    /// </typeparam>
    public abstract class PassiveStateBase<TController> : IPassiveState
    {
        protected TController Controller;

        public PassiveStateBase(TController controller) => Controller = controller;

        public abstract void OnEnterState();

        public abstract void OnExitState();
    }
}