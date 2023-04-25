using System;

namespace Hybel.StateMachines
{
    /// <summary>
    /// Type used to configure a <see cref="IStateMachine{TState, TTrigger, TStateMachine}"/>.
    /// </summary>
    public class StateMachineConfigurer<TState, TTrigger, TStateMachine> where TStateMachine : IStateMachine<TState, TTrigger, TStateMachine>
    {
        private readonly TStateMachine _stateMachine;

        public StateMachineConfigurer(TStateMachine stateMachine) => _stateMachine = stateMachine;

        /// <summary>
        /// Create a new config where all transitions are from <paramref name="state"/>.
        /// </summary>
        public StateConfig At(TState state) => new StateConfig(_stateMachine, this, state);

        /// <summary>
        /// Create a new transition from any state to <paramref name="toState"/> when <paramref name="trigger"/> is fired.
        /// </summary>
        public StateMachineConfigurer<TState, TTrigger, TStateMachine> AtAnyPermit(TState toState, TTrigger trigger)
        {
            _stateMachine.AddAnyTransition(toState, trigger);
            return this;
        }

        public class StateConfig
        {
            private TState _state;
            private readonly TStateMachine _stateMachine;
            private readonly StateMachineConfigurer<TState, TTrigger, TStateMachine> _stateConfigurer;

            public StateConfig(
                TStateMachine stateMachine,
                StateMachineConfigurer<TState, TTrigger, TStateMachine> stateConfigurer,
                TState state)
            {
                _stateMachine = stateMachine;
                _stateConfigurer = stateConfigurer;
                _state = state;
            }

            /// <summary>
            /// Permit a transition from the configured state to <paramref name="to"/> when <paramref name="trigger"/> is fired.
            /// </summary>
            public StateConfig Permit(TState to, TTrigger trigger)
            {
                _stateMachine.AddTransition(_state, to, trigger);
                return this;
            }

            /// <summary>
            /// Permit the state to enter itself when any of the <paramref name="triggers"/> are fired.
            /// </summary>
            public StateConfig PermitSelfEntry(params TTrigger[] triggers)
            {
                foreach (TTrigger trigger in triggers)
                    _stateMachine.AddTransition(_state, _state, trigger);

                return this;
            }

            /// <summary>
            /// Create a new config where all transitions are from <paramref name="state"/>.
            /// </summary>
            public StateConfig At(TState state) => _stateConfigurer.At(state);
        }
    }

    /// <summary>
    /// Type used to configure a <see cref="IStateMachine{TState, TTrigger, TStateMachine}"/>.
    /// </summary>
    public class StateMachineConfigurer<TState, TStateMachine> where TStateMachine : IStateMachine<TState, TStateMachine>
    {
        private readonly TStateMachine _stateMachine;

        public StateMachineConfigurer(TStateMachine stateMachine) => _stateMachine = stateMachine;

        /// <summary>
        /// Create a new config where all transitions are from <paramref name="state"/>.
        /// </summary>
        public StateConfig At(TState state) => new StateConfig(_stateMachine, this, state);

        /// <summary>
        /// Create a new transition from any state to <paramref name="toState"/> when <paramref name="trigger"/> is fired.
        /// </summary>
        public StateMachineConfigurer<TState, TStateMachine> AtAnyPermit(TState toState)
        {
            _stateMachine.AddAnyTransition(toState, toState.GetType());
            return this;
        }

        public class StateConfig
        {
            private TState _state;
            private readonly TStateMachine _stateMachine;
            private readonly StateMachineConfigurer<TState, TStateMachine> _stateConfigurer;

            public StateConfig(
                TStateMachine stateMachine,
                StateMachineConfigurer<TState, TStateMachine> stateConfigurer,
                TState state)
            {
                _stateMachine = stateMachine;
                _stateConfigurer = stateConfigurer;
                _state = state;
            }

            /// <summary>
            /// Permit a transition from the configured state to <paramref name="to"/> when <paramref name="trigger"/> is fired.
            /// </summary>
            public StateConfig Permit(TState to)
            {
                _stateMachine.AddTransition(_state, to, to.GetType());
                return this;
            }

            /// <summary>
            /// Permit the state to enter itself when any of the <paramref name="triggers"/> are fired.
            /// </summary>
            public StateConfig PermitSelfEntry(params Type[] triggers)
            {
                foreach (Type trigger in triggers)
                    _stateMachine.AddTransition(_state, _state, trigger);

                return this;
            }

            /// <summary>
            /// Create a new config where all transitions are from <paramref name="state"/>.
            /// </summary>
            public StateConfig At(TState state) => _stateConfigurer.At(state);
        }
    }
}