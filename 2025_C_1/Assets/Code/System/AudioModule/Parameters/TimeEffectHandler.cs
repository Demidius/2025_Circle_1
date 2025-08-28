using CodeBase.System.Core.Consts;
namespace CodeBase.System.GameSystems.AudioModule.Parameters
{
    public class TimeEffectHandler
    {
        public void SetTimeAudioEffect(float timeModifier)
        {
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Const.TimeEffect, timeModifier);
        }

    }
}
