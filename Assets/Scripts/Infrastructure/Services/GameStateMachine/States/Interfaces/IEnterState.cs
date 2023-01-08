namespace Simple.Nonogram.Infrastructure.Services
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
