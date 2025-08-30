using System;
namespace Code.GameEntyties.Player
{
    public interface ITanksEngine
    {
        public event Action<bool> ChangeEngineState;
        public event Action<float> ChangeRpm;
        public float EngineRpm { get; }
    }
}
