using System;
using UnityEngine;
namespace Code.GameEntyties.Player
{

    public class TrackVelocity : MonoBehaviour, ITrackVelocity
    {

        [Tooltip("Скорость изменения тяги на гусенице (ед/с)")]
        [SerializeField] private float changeSpeed = 1.5f;


        //params

        private float _leftTrackVelocity;
        private float _rightTrackVelocity;
        private float _leftVelTarget;
        private float _rightVelTarget;

        public float LeftTrackVelocity => _leftTrackVelocity;
        public float RightTrackVelocity => _rightTrackVelocity;

        private void Update()
        {
            LeftTrack();
            RightTrack();
        }

        private void LeftTrack()
        {
            _leftVelTarget = (Input.GetKey(KeyCode.Q) ? 1f : 0f) + (Input.GetKey(KeyCode.A) ? -1f : 0f);

            _leftTrackVelocity = Mathf.MoveTowards(_leftTrackVelocity, _leftVelTarget, changeSpeed * Time.deltaTime);
        }

        private void RightTrack()
        {
            _rightVelTarget = (Input.GetKey(KeyCode.E) ? 1f : 0f) + (Input.GetKey(KeyCode.D) ? -1f : 0f);

            _rightTrackVelocity = Mathf.MoveTowards(_rightTrackVelocity, _rightVelTarget, changeSpeed * Time.deltaTime);
        }

    }

}
