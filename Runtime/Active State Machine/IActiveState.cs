namespace Hybel.StateMachines
{
    public interface IActiveState : IState
    {
        /// <summary>
        /// Should contain behaviour for when the state is active.
        /// <para>Should be called every frame like the <c>Update()</c> Unity Method.</para>
        /// </summary>
        public void Tick();
    }
}
