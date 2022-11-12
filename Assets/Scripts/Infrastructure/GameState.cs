using Simple.Nonogram.Infrastructure.Services.Audio;
using UniRx;

namespace Simple.Nonogram.Infrastructure
{
    public class GameState
    {
        internal readonly ReactiveProperty<AudioState> audioState = new ReactiveProperty<AudioState>(AudioState.AllEnabled);
    }
}
