using Code.GameEntyties.Player;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform _player;    // ссылка на игрока
    [SerializeField] private NavMeshAgent _agent;  // агент

    [Header("Параметры")]
    [SerializeField] private float _followDistance = 10f; // дистанция, с которой идём за игроком
    [SerializeField] private float _circleRadius = 3f;    // радиус движения вокруг игрока
    [SerializeField] private float _updateDelay = 1.5f;   // раз в сколько секунд менять цель

    private float _nextUpdateTime;

    private void Awake()
    {
        if (_agent == null) _agent = GetComponent<NavMeshAgent>();
        if (_player == null)
        {
            var tag = FindAnyObjectByType<PlayerTag>(); // если есть свой PlayerTag
            if (tag != null) _player = tag.transform;
            else Debug.LogWarning("EnemyAI: игрок не найден!");
        }
    }

    private void Update()
    {
        if (_player == null) return;

        float dist = Vector3.Distance(transform.position, _player.position);

        if (dist > _followDistance)
        {
            // идём к игроку напрямую
            _agent.SetDestination(_player.position);
        }
        else
        {
            // ходим рандомно вокруг игрока
            if (Time.time >= _nextUpdateTime)
            {
                _nextUpdateTime = Time.time + _updateDelay;
                Vector3 randomOffset = Random.insideUnitCircle * _circleRadius;
                Vector3 target = _player.position + new Vector3(randomOffset.x, 0, randomOffset.y);

                if (NavMesh.SamplePosition(target, out NavMeshHit hit, 10.0f, NavMesh.AllAreas))
                {
                    _agent.SetDestination(hit.position);
                }
            }
        }
    }
}
