using UnityEngine;
namespace CodeBase.System.Core.Consts
{
    public class PlayerSettingsService
    {

        private const string MusicVolumeKey = "MusicVolume";
        private const string EffectVolumeKey = "EffectVolume";
        private const string ScaleCursor = "ScaleCursor";

        public float MusicVolume
        {
            get => PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
            set
            {
                PlayerPrefs.SetFloat(MusicVolumeKey, Mathf.Clamp01(value));
                PlayerPrefs.Save();
            }
        }

        public float EffectVolume
        {
            get => PlayerPrefs.GetFloat(EffectVolumeKey, 0.5f);
            set
            {
                PlayerPrefs.SetFloat(EffectVolumeKey, Mathf.Clamp01(value));
                PlayerPrefs.Save();
            }
        }
        public float CursorSacale
        {
            get => PlayerPrefs.GetFloat(ScaleCursor, 0.5f);
            set
            {
                PlayerPrefs.SetFloat(ScaleCursor, Mathf.Clamp01(value));
                PlayerPrefs.Save();
            }
        }

    }
}
