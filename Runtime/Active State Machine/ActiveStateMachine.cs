using System.Collections.Generic;

namespace Hybel.StateMachines
{
    /// <summary>
    /// Active State Machine that automatically transitions when conditions are met.
    /// </summary>
    /// <typeparam name="TState">Type to represent states used in the <see cref="ActiveStateMachine{TState}"/>.</typeparam>
    public class ActiveStateMachine<TState> : IStateMachine<TState, Condition, ActiveStateMachine<TState>> where TState : IActiveState
    {
        private TState _currentState;
        private readonly Dictionary<TState, List<Transition>> _transitions;                 // Dictionary of all 'from-to' transitions.
        private List<Transition> _anyTransitions;                                           // List of all 'from any-to' transitions.
        private List<Transition> _currentTransitions;                                       // List of all transitions related to _currentState.
        private readonly static List<Transition> _emptyTransitions = new List<Transition>(0);

        public TState CurrentState => _currentState;

        public ActiveStateMachine()
        {
            _transitions = new Dictionary<TState, List<Transition>>();
            _anyTransitions = new List<Transition>();
            _currentTransitions = new List<Transition>();
        }

        /// <summary>
        /// When calling this method, chain several <see cref="StateMachineConfigurer{TState, TTrigger}.At(TState, TState, TTrigger)"/>
        /// and <see cref="StateMachineConfigurer{TState, TTrigger}.AtAnyPermit(TState, TTrigger)"/> to configure states and transitions.
        /// </summary>
        public StateMachineConfigurer<TState, Condition, ActiveStateMachine<TState>> Configure() =>
            new StateMachineConfigurer<TState, Condition, ActiveStateMachine<TState>>(this);

        /// <summary>
        /// Checks for transitions, then changes states if needed, then calls the current state's Tick method.
        /// </summary>
        public void Tick()
        {
            Transition transition = GetTransition();

            if (transition != null)
                SetState(transition.To);

            _currentState.Tick();
        }

        /// <summary>
        /// Explicityly set the state of the state machine.
        /// </summary>
        public void SetState(TState state)
        {
            if (state.Equals(_currentState)) // Just return if the new state is the same as the current state. No need to do any logic.
                return;

            _currentState?.OnExitState();
            _currentState = state;

            _transitions.TryGetValue(_currentState, out _currentTransitions);

            if (_currentTransitions is null)
                _currentTransitions = _emptyTransitions;

            _currentState.OnEnterState();
        }

        /// <summary>
        /// Add a transition between states based on a given condition.
        /// </summary>
        public void AddTransition(TState from, TState to, Condition condition)
        {
            if (_transitions.TryGetValue(from, out var transitions) is false)
            {
                transitions = new List<Transition>();
                _transitions[from] = transitions;
            }

            transitions.Add(new Transition(to, condition));
        }

        /// <summary>
        /// Add a transition from any active state to another state based on a given condition.
        /// <para>This will change the current state no matter what the current state is.</para>
        /// </summary>
        public void AddAnyTransition(TState state, Condition condition) =>
            _anyTransitions.Add(new Transition(state, condition));

        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Condition())
                    return transition;

            foreach (var transition in _currentTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }

        private class Transition
        {
            private Condition _condition;
            private TState _to;

            public Condition Condition => _condition;

            public TState To => _to;

            public Transition(TState to, Condition condition)
            {
                _to = to;
                _condition = condition;
            }
        }
    }
}