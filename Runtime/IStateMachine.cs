using System;

namespace Hybel.StateMachines
{
    public interface IStateMachine<TState, TTrigger, TStateMachine>
        where TStateMachine : IStateMachine<TState, TTrigger, TStateMachine>
    {
        TState CurrentState { get; }

        void SetState(TState state);
        StateMachineConfigurer<TState, TTrigger, TStateMachine> Configure();
        void AddTransition(TState from, TState to, TTrigger trigger);
        void AddAnyTransition(TState state, TTrigger trigger);
    }

    public interface IStateMachine<TState, TStateMachine>
        where TStateMachine : IStateMachine<TState, TStateMachine>
    {
        TState CurrentState { get; }

        void SetState(TState state);
        StateMachineConfigurer<TState, TStateMachine> Configure();
        void AddTransition(TState from, TState to, Type trigger);
        void AddAnyTransition(TState state, Type trigger);
    }
}