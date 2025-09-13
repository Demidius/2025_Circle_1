using UnityEngine;
namespace Code.TODO
{
    public sealed class KeyboardTankInputProvider : ITankInputProvider
    {
        public float LeftTrackInput =>
            (Input.GetKey(KeyCode.Q) ? 1f : 0f) + (Input.GetKey(KeyCode.A) ? -1f : 0f);

        public float RightTrackInput =>
            (Input.GetKey(KeyCode.E) ? 1f : 0f) + (Input.GetKey(KeyCode.D) ? -1f : 0f);

        public bool ToggleEnginePressed => Input.GetKeyDown(KeyCode.I);
    }
}
