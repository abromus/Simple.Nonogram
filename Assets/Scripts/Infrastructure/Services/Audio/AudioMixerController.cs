using System;
using Simple.Nonogram.Extensions;
using Simple.Nonogram.GameCore;
using Simple.Nonogram.Infrastructure.Services.Application;
using Simple.Nonogram.Infrastructure.Services.DependencyInjection;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

namespace Simple.Nonogram.Infrastructure.Services.Audio
{
    public class AudioMixerController : MonoBehaviour, IService
    {
        private const int Mute = -80;
        private const int NormalVolume = 0;

        private const string AllVolumeKey = "AllVolume";
        private const string MusicVolumeKey = "MusicVolume";
        private const string SoundsVolumeKey = "SoundsVolume";

        [SerializeField] private CoreSceneController _coreSceneController;
        [SerializeField] private AudioMixer _mixer;

        private GameWorld _world;
        private IDisposable _initSubscription;
        private IDisposable _stateChangeSubscription;

        private bool _isInitialized;

        public bool IsInitialized => _isInitialized;

        private void Start()
        {
            var compositionRoot = DI.GetCompositionRoot(CompositionTag.Root);
            compositionRoot.Add(this);

            _initSubscription = _coreSceneController.OnGameInitialized.Subscribe(_ =>
            {
                RxUtils.SafeUnsubscribe(ref _initSubscription);
                var gameProvider = compositionRoot.Get<IGameProvider>();
                _world = gameProvider.World;

                OnInitalizeComplete();
            });
        }

        private void OnDestroy()
        {
            RxUtils.SafeUnsubscribe(ref _stateChangeSubscription);
        }

        public void Initialize()
        {
            _isInitialized = true;
        }

        public void SetAllEnabled()
        {
            _mixer.SetFloat(AllVolumeKey, NormalVolume);
            _mixer.SetFloat(MusicVolumeKey, NormalVolume);
            _mixer.SetFloat(SoundsVolumeKey, NormalVolume);
        }

        public void SetMusicOnly()
        {
            _mixer.SetFloat(AllVolumeKey, NormalVolume);
            _mixer.SetFloat(MusicVolumeKey, NormalVolume);
            _mixer.SetFloat(SoundsVolumeKey, Mute);
        }

        public void SetSoundOnly()
        {
            _mixer.SetFloat(AllVolumeKey, NormalVolume);
            _mixer.SetFloat(MusicVolumeKey, Mute);
            _mixer.SetFloat(SoundsVolumeKey, NormalVolume);
        }

        public void SetMute()
        {
            _mixer.SetFloat(AllVolumeKey, Mute);
            _mixer.SetFloat(MusicVolumeKey, NormalVolume);
            _mixer.SetFloat(SoundsVolumeKey, NormalVolume);
        }

        public void SetMusicHalfVolume()
        {
            _mixer.SetFloat(AllVolumeKey, NormalVolume);
            _mixer.SetFloat(MusicVolumeKey, NormalVolume);
            _mixer.SetFloat(SoundsVolumeKey, NormalVolume);
        }

        private void AudioStateChanged(AudioState state)
        {
            switch (state)
            {
                case AudioState.AllEnabled:
                    SetAllEnabled();
                    break;
                case AudioState.MusicOnly:
                    SetMusicOnly();
                    break;
                case AudioState.SoundOnly:
                    SetSoundOnly();
                    break;
                case AudioState.Mute:
                    SetMute();
                    break;
                default:
                    SetAllEnabled();
                    break;
            }
        }

        private void OnInitalizeComplete()
        {
            _stateChangeSubscription = _world.GameState.audioState.Subscribe(AudioStateChanged);

            AudioStateChanged(_world.GameState.audioState.Value);
        }
    }
}
