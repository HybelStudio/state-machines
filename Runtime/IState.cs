namespace Hybel.StateMachines
{
    public interface IState
    {
        /// <summary>
        /// Should be called when the state is entered.
        /// </summary>
        public void OnEnterState();

        /// <summary>
        /// Should be called when the state is exited.
        /// </summary>
        public void OnExitState();
    }
}
