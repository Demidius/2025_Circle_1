using Code.UIModule.Controllers;
using FMODUnity;
using UnityEngine;
using Zenject;
namespace CodeBase.System.GameSystems.AudioModule.BaseLogic
{
    public class AudioTracksBase : MonoBehaviour
    {
        private AudioManager _audioManager;
      
        

        [Inject]
        private void Construct(AudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        // Музыка
        public EventReference MusicGameplay;
        public EventReference MusicMenu;
        //
        // // Атмосфера / окружение
        public EventReference LevelEmbient;
        public EventReference MenuEmbient;
        //
        // // Игровые звуки
        public EventReference Engine;
        public EventReference StartEngine;
        public EventReference StopEngine;
        public EventReference Force1;
        public EventReference Force2;
        public EventReference StartForceEngine;
        public EventReference StopForceEngine;
        public EventReference TransmissionChanger;
        public EventReference TrackSound;
       
        // Effects
        public EventReference Explosion;
        public EventReference GunShoot;
        
        
        // public EventReference EnemyShoot;
        // public EventReference Spawn;
        // public EventReference SpiderStep;
        // public EventReference Step;
        // public EventReference Explosion;
        // public EventReference DustSound;
        // public EventReference WaveStart;
        // public EventReference HitRock;
        // public EventReference CantTake;
        // public EventReference TakeBullet;
        // public EventReference TakeCrystal;
        //
        // // Интерфейс (UI)
        public EventReference SystemUnderButton;
        public EventReference MenuWindowChange;
        public EventReference BottonDownClick;
        public EventReference TextInputError;

        private void Start()
        {
            InitializePools();
        }

        private void InitializePools()
        {
            // Музыка
            _audioManager.InitializeSoundPool(MusicGameplay, 1);
            _audioManager.InitializeSoundPool(MusicMenu, 1);
            //
            // // Атмосфера / окружение
            _audioManager.InitializeSoundPool(LevelEmbient, 5);
            _audioManager.InitializeSoundPool(MenuEmbient, 5);
            //
            // // Игровые звуки
            _audioManager.InitializeSoundPool(Engine, 1);
            _audioManager.InitializeSoundPool(StartEngine, 5);
            _audioManager.InitializeSoundPool(StopEngine, 5);
            _audioManager.InitializeSoundPool(Force1, 1);
            _audioManager.InitializeSoundPool(Force2, 1);
            _audioManager.InitializeSoundPool(StartForceEngine, 3);
            _audioManager.InitializeSoundPool(StopForceEngine, 3);
            _audioManager.InitializeSoundPool(TransmissionChanger, 3);
            _audioManager.InitializeSoundPool(TrackSound, 3);
           
            _audioManager.InitializeSoundPool(Explosion, 30);
            _audioManager.InitializeSoundPool(GunShoot, 15);
            // _audioManager.InitializeSoundPool(EnemyShoot, 10);
            // _audioManager.InitializeSoundPool(Spawn, 5);
            // _audioManager.InitializeSoundPool(SpiderStep, 5);
            // _audioManager.InitializeSoundPool(Step, 10);
            // _audioManager.InitializeSoundPool(Explosion, 5);
            // _audioManager.InitializeSoundPool(DustSound, 5);
            // _audioManager.InitializeSoundPool(WaveStart, 5);
            // _audioManager.InitializeSoundPool(HitRock, 5);
            // _audioManager.InitializeSoundPool(CantTake, 5);
            // _audioManager.InitializeSoundPool(TakeBullet, 5);
            // _audioManager.InitializeSoundPool(TakeCrystal, 5);
            //
            // // Интерфейс (UI)
            _audioManager.InitializeSoundPool(SystemUnderButton, 5);
            _audioManager.InitializeSoundPool(MenuWindowChange, 5);
            _audioManager.InitializeSoundPool(BottonDownClick, 5);
            _audioManager.InitializeSoundPool(TextInputError, 5);
        }
    }

}
