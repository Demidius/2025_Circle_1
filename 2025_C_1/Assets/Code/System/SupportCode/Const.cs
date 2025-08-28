using UnityEngine;
namespace CodeBase.System.Core.Consts
{
    
    public class Const 
    {
        public static float timeHoldRoutine = 0.5f;

        public static string playerName = "Test";

        public const string LevelScene = nameof(LevelScene);
               

        public const float DampingValue = 200;
                      
      
        public const string Run = nameof(Run);
        public const string Active = nameof(Active);

       
        
        private const float Epsilon = 0.0001f;
        
        public const int MaxValieBulletsOnScene = 7;
        public static string GameScene = "Assets/Scenes/GameScene.unity";
        

        public static bool AreFloatsEqual(float a, float b)
        {
            return Mathf.Abs(a - b) < Epsilon;
        }
        
        //Audio
        public const string Effectvolume = "EffectVolume";
        public const string Musicvolume = "MusicVolume";
        public const string TimeEffect = "TimeEffect";

        public static int EnemiesStepSouses = 0;
        public static bool EnableToAddStepSources
        {
            get
            {
                if (EnemiesStepSouses < MaxEnemiesStepSources)
                    return true;
                return false;
            }
        }
        public const int MaxEnemiesStepSources = 5;

        public static readonly Vector3 ExplosionSizeBig = new Vector3(4f, 4f, 4f);
        public static readonly Vector3 ExplosionSizeNormal = new Vector3(1f, 1f, 1f);

        // UI Button Animation
        public const float ButtonHoverScaleMultiplier = 1.1f;
        public const float ButtonClickScaleMultiplier = 1.2f;
        public const float ButtonHoverAnimationDuration = 0.3f;
        public const float ButtonClickAnimationDuration = 0.1f;


        //SroreURL
        public const string FeedbackUrl =
            "https://script.google.com/macros/s/AKfycbxz5c_QTZ81wE5fcBWHmI4vaz_vSV95zRMDBMLzGrCvQW3MswqCUwGOz1oud9j29MX7/exec";
        public const string SroreURL =
            "https://script.google.com/macros/s/AKfycbxEYDfA364pZoA3clPmblO49K5CxlxKXM8ui_8Q6-zqibWOWX-JuIWWBhTImazOE9gSIw/exec";
       
        
        //To Google:

        public const float SlowdownRateModifier = 0.15f;
      

        public const float MeleeAttackDuration = 0.1f;
        public const float MeleeAttackEnergyCost = 5f;
        public const float PriceIncreaseRate = 1.2f;





    }
}
