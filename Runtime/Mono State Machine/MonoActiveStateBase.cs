using UnityEngine;

namespace Hybel.StateMachines
{
    /// <summary>
    /// Base class for all states used by a MonoBehaviour.
    /// </summary>
    public abstract class MonoActiveStateBase : ActiveStateBase<MonoActiveStateBehaviour>
    {
        private GameObject _gameObject;
        private Transform _transform;

        /// <summary>
        /// References the behaviours GameObject.
        /// </summary>
        protected GameObject gameObject => _gameObject;

        /// <summary>
        /// References the behaviours Transform component.
        /// </summary>
        protected Transform transform => _transform;

        /// <summary>
        /// Initializes base state fields.
        /// </summary>
        public MonoActiveStateBase(MonoActiveStateBehaviour behaviour) : base(behaviour)
        {
            _gameObject = behaviour.gameObject;
            _transform = behaviour.transform;
        }

        public override abstract void Tick();

        public override abstract void OnEnterState();

        public override abstract void OnExitState();
    }

    /// <summary>
    /// Base class for all states used by a MonoBehaviour.
    /// </summary>
    /// <typeparam name="TStateObject">Use this to restricts the usage of the state to only one type of behaviour or its derived types.</typeparam>
    public abstract class MonoActiveStateBase<TStateObject> : MonoActiveStateBase where TStateObject : MonoActiveStateBehaviour
    {
        protected new TStateObject StateObject => (TStateObject)base.StateObject;

        /// <summary>
        /// Initializes base state fields.
        /// </summary>
        public MonoActiveStateBase(TStateObject behaviour) : base(behaviour) { }

        public override abstract void Tick();

        public override abstract void OnEnterState();

        public override abstract void OnExitState();
    }
}