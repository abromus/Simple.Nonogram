﻿namespace Simple.Nonogram.Infrastructure.Services
{
    public interface IGameStateMachine : IService
    {
        public void Enter<Tstate>() where Tstate : class, IEnterState;
        public void Enter<Tstate, TPayload>(TPayload payload) where Tstate : class, IEnterState<TPayload>;
    }
}
