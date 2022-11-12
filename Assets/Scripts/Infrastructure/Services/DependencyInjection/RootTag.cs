namespace Simple.Nonogram.Infrastructure.Services.DependencyInjection
{
    public sealed class RootTag : MonoTag
    {
#if UNITY_EDITOR
        protected override string InjectTag => CompositionTag.Root;
#endif
    }
}
