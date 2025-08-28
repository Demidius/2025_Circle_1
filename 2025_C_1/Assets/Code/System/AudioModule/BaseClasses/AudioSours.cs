
using Code.UIModule.Controllers;
using CodeBase.System.GameSystems.AudioModule.BaseLogic;
using Zenject;


public abstract class AudioSours
{
    protected AudioTracksBase _audioTracksBase;
    protected AudioManager _audioManager;

    [Inject]
    void Construct(AudioManager audioManager, AudioTracksBase audioTracksBase)
    {
        _audioManager = audioManager;
        _audioTracksBase = audioTracksBase;
    }

}