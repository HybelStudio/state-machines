namespace Hybel.StateMachines
{
    /// <summary>
    /// Type used to configure a <see cref="PassiveStateMachine{TState, TInput}"/>.
    /// </summary>
    public class StateConfigurer<TState, TTrigger>
    {
        private readonly PassiveStateMachine<TState, TTrigger> _stateMachine;

        public StateConfigurer(PassiveStateMachine<TState, TTrigger> stateMachine) => _stateMachine = stateMachine;

        /// <summary>
        /// Create a new config where all transitions are from <paramref name="state"/>.
        /// </summary>
        public StateConfig At(TState state) => new StateConfig(_stateMachine, this, state);

        /// <summary>
        /// Create a new transition from any state to <paramref name="toState"/> when <paramref name="trigger"/> is fired.
        /// </summary>
        public StateConfigurer<TState, TTrigger> AtAnyPermit(TState toState, TTrigger trigger)
        {
            _stateMachine.AddAnyTransition(toState, trigger);
            return this;
        }

        public class StateConfig
        {
            private TState _state;
            private readonly PassiveStateMachine<TState, TTrigger> _stateMachine;
            private readonly StateConfigurer<TState, TTrigger> _stateConfigurer;

            public StateConfig(
                PassiveStateMachine<TState, TTrigger> stateMachine,
                StateConfigurer<TState, TTrigger> stateConfigurer,
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
}