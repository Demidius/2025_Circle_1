using System;

using CodeBase.System.GameSystems.StateMachine.Core;
using CodeBase.System.GameSystems.StateMachine.States;
using FMOD.Studio;
using FMODUnity;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;
namespace CodeBase.System.GameSystems.AudioModule.AudioPlayrs
{
    public class MusicMixer : AudioSours, IInitializable, IDisposable
    {
        private GameStateMachine _gameStateMachine;
        private EventInstance _sourseMusic;
        private EventInstance _sourseAmbiant;
        private bool _onGameState;


        public MusicMixer(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        public void Initialize()
        {
            _gameStateMachine.OnStateChanged += PlayMusic;
            _onGameState = true;
        }
        public void Dispose()
        {
            _gameStateMachine.OnStateChanged -= PlayMusic;
        }

        private void PlayMusic(GameState state)
        {
            // Debug.Log("Playing Music");
            switch (state)
            {
                case MenuState:
                    if (_onGameState)
                    {
                        _onGameState = false;
                        PlayMusic(_audioTracksBase.MusicMenu, _audioTracksBase.MenuEmbient);
                    }
                    break;

                case GameplayState:
                    if (!_onGameState)
                    {
                        _onGameState = true;
                        PlayMusic(_audioTracksBase.MusicGameplay, _audioTracksBase.LevelEmbient);
                    }
                    break;
            }
        }

        private void PlayMusic(EventReference trackMusic, EventReference trackAmbiant )
        {
            Stop();
            _sourseMusic = _audioManager.PlaySoundWithInstance(trackMusic, useInstance: true);
            _sourseAmbiant = _audioManager.PlaySoundWithInstance(trackAmbiant, useInstance: true);
        }

        public void Stop()
        {
            if (_sourseMusic.isValid() == false)
                return;

            _sourseMusic.stop(STOP_MODE.ALLOWFADEOUT);
            
            if (_sourseAmbiant.isValid() == false)
                return;

            _sourseAmbiant.stop(STOP_MODE.ALLOWFADEOUT);
        }

    }
}
