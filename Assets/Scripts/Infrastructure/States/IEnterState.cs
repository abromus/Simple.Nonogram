namespace Simple.Nonogram.Infrastructure.States
{
    public interface IEnterState : IExitState
    {
        public void Enter();
    }

    public interface IEnterState<TPayload> : IExitState
    {
        public void Enter(TPayload payload);
    }
}
