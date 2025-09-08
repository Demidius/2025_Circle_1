using UnityEngine;

public class TankTurret : MonoBehaviour
{
    [SerializeField] private Transform _turret; // сама башня
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] Camera _mainCamera;

    void Update()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = _turret.position.y;

            Vector3 dir = (targetPos - _turret.position).normalized;
            Quaternion targetRot = Quaternion.LookRotation(dir);

            _turret.rotation = Quaternion.Slerp(
                _turret.rotation, targetRot,
                _rotationSpeed * Time.deltaTime);
        }
    }
}
