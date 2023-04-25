using System;
using System.Collections.Generic;

namespace Hybel.StateMachines
{
    /// <summary>
    /// Passive State Machine that only transitions when an input is given.
    /// <para>Use <see cref="Configure"/> to configure the state machine.</para>
    /// </summary>
    /// <typeparam name="TState">Type to represent states used in the <see cref="PassiveStateMachine{TState, TTrigger}"/>.</typeparam>
    /// <typeparam name="TTrigger">Type to represent inputs used the <see cref="PassiveStateMachine{TState, TTrigger}"/>.</typeparam>
    public class PassiveStateMachine<TState, TTrigger> : IStateMachine<TState, TTrigger, PassiveStateMachine<TState, TTrigger>>
    {
        private TState _currentState;
        private readonly Dictionary<TState, List<Transition>> _transitions;                 // Dictionary of all 'from-to' transitions.
        private List<Transition> _anyTransitions;                                           // List of all 'from any-to' transitions.
        private List<Transition> _currentTransitions;                                       // List of all transitions related to _currentState.
        private readonly static List<Transition> _emptyTransitions = new List<Transition>(0);

        /// <summary>
        /// Fired whenever a state is entered.
        /// </summary>
        public event Action<TState> OnEnterState;

        /// <summary>
        /// Fired whenever a state is exited.
        /// </summary>
        public event Action<TState> OnExitState;

        public TState CurrentState => _currentState;

        public PassiveStateMachine()
        {
            _transitions = new Dictionary<TState, List<Transition>>();
            _anyTransitions = new List<Transition>();
            _currentTransitions = new List<Transition>();
        }

        public PassiveStateMachine(TState initialState) : this() => SetState(initialState);

        /// <summary>
        /// When calling this method, chain several At(TState from),
        /// Permit(TState to, TTrigger trigger),
        /// PermitSelfEntry(params TTrigger[] triggers)
        /// and AtAnyPermit(TState to, TTrigger trigger) to configure states and transitions.
        /// </summary>
        public StateMachineConfigurer<TState, TTrigger, PassiveStateMachine<TState, TTrigger>> Configure() =>
            new StateMachineConfigurer<TState, TTrigger, PassiveStateMachine<TState, TTrigger>>(this);
        
        /// <summary>
        /// Fire a <paramref name="trigger"/>.
        /// <para>A transition will happen if the <see cref="PassiveStateMachine{TState, TTrigger}"/> configuration allows it.</para>
        /// </summary>
        /// <param name="trigger"></param>
        public bool Fire(TTrigger trigger)
        {
            Transition transition = GetTransition(trigger);
            if (transition is null)
                return false;

            SetState(transition.To);
            return true;
        }

        /// <summary>
        /// Explicityly set the state of the state machine.
        /// </summary>
        public void SetState(TState state)
        {
            if (state.Equals(_currentState)) // Just return if the new state is the same as the current state.
                return;

            if (_currentState != null)
                OnExitState?.Invoke(_currentState);

            _currentState = state;

            _transitions.TryGetValue(_currentState, out _currentTransitions);

            if (_currentTransitions is null)
                _currentTransitions = _emptyTransitions;

            OnEnterState?.Invoke(state);
        }

        /// <summary>
        /// Add a transition between states based on a given condition.
        /// </summary>
        public void AddTransition(TState from, TState to, TTrigger trigger)
        {
            if (_transitions.TryGetValue(from, out var transitions) is false)
            {
                transitions = new List<Transition>();
                _transitions[from] = transitions;
            }

            transitions.Add(new Transition(to, trigger));
        }

        /// <summary>
        /// Add a transition from any active state to another state based on a given condition.
        /// <para>This will change the current state no matter what the current state is.</para>
        /// </summary>
        public void AddAnyTransition(TState to, TTrigger trigger) =>
            _anyTransitions.Add(new Transition(to, trigger));

        private Transition GetTransition(TTrigger trigger)
        {
            foreach (Transition transition in _anyTransitions)
                if (transition.Trigger.Equals(trigger))
                    return transition;

            foreach (Transition transition in _currentTransitions)
                if (transition.Trigger.Equals(trigger))
                    return transition;

            return null;
        }

        private class Transition
        {
            private TTrigger _trigger;
            private TState _to;

            public TTrigger Trigger => _trigger;

            public TState To => _to;

            public Transition(TState to, TTrigger trigger)
            {
                _to = to;
                _trigger = trigger;
            }
        }        
    }

    public class PassiveStateMachine<TState> : IStateMachine<TState, PassiveStateMachine<TState>>
    {
        private TState _currentState;
        private readonly Dictionary<TState, List<Transition>> _transitions;                 // Dictionary of all 'from-to' transitions.
        private List<Transition> _anyTransitions;                                           // List of all 'from any-to' transitions.
        private List<Transition> _currentTransitions;                                       // List of all transitions related to _currentState.
        private readonly static List<Transition> _emptyTransitions = new List<Transition>(0);

        /// <summary>
        /// Fired whenever a state is entered.
        /// </summary>
        public event Action<TState> OnEnterState;

        /// <summary>
        /// Fired whenever a state is exited.
        /// </summary>
        public event Action<TState> OnExitState;

        public TState CurrentState => _currentState;

        public PassiveStateMachine()
        {
            _transitions = new Dictionary<TState, List<Transition>>();
            _anyTransitions = new List<Transition>();
            _currentTransitions = new List<Transition>();
        }

        /// <summary>
        /// When calling this method, chain several At(TState from),
        /// Permit(TState to, TTrigger trigger),
        /// PermitSelfEntry(params TTrigger[] triggers)
        /// and AtAnyPermit(TState to, TTrigger trigger) to configure states and transitions.
        /// </summary>
        public StateMachineConfigurer<TState, PassiveStateMachine<TState>> Configure() =>
            new StateMachineConfigurer<TState, PassiveStateMachine<TState>>(this);

        /// <summary>
        /// Fire a <paramref name="trigger"/>.
        /// <para>A transition will happen if the <see cref="PassiveStateMachine{TState, Type}"/> configuration allows it.</para>
        /// </summary>
        /// <param name="trigger"></param>
        public bool Fire(Type trigger)
        {
            Transition transition = GetTransition(trigger);
            if (transition is null)
                return false;

            SetState(transition.To);
            return true;
        }

        /// <summary>
        /// Explicityly set the state of the state machine.
        /// </summary>
        public void SetState(TState state)
        {
            if (state.Equals(_currentState)) // Just return if the new state is the same as the current state.
                return;

            OnExitState?.Invoke(_currentState);
            _currentState = state;

            _transitions.TryGetValue(_currentState, out _currentTransitions);

            if (_currentTransitions is null)
                _currentTransitions = _emptyTransitions;

            OnEnterState?.Invoke(state);
        }

        /// <summary>
        /// Add a transition between states based on a given condition.
        /// </summary>
        public void AddTransition(TState from, TState to, Type trigger)
        {
            if (_transitions.TryGetValue(from, out var transitions) is false)
            {
                transitions = new List<Transition>();
                _transitions[from] = transitions;
            }

            transitions.Add(new Transition(to, trigger));
        }

        /// <summary>
        /// Add a transition from any active state to another state based on a given condition.
        /// <para>This will change the current state no matter what the current state is.</para>
        /// </summary>
        public void AddAnyTransition(TState to, Type trigger) =>
            _anyTransitions.Add(new Transition(to, trigger));

        private Transition GetTransition(Type trigger)
        {
            foreach (Transition transition in _anyTransitions)
                if (transition.Trigger.Equals(trigger))
                    return transition;

            foreach (Transition transition in _currentTransitions)
                if (transition.Trigger.Equals(trigger))
                    return transition;

            return null;
        }

        private class Transition
        {
            private Type _trigger;
            private TState _to;

            public Type Trigger => _trigger;

            public TState To => _to;

            public Transition(TState to, Type trigger)
            {
                _to = to;
                _trigger = trigger;
            }
        }
    }
}