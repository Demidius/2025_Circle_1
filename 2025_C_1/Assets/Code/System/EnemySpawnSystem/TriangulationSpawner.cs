using System.Collections;
using Code.GameEntyties.Player;
using Code.GameEntyties.Target;
using CodeBase.System.GameSystems.Pools;
using CodeBase.System.Services.Utilities.Coroutines.CoroutinRuner;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Code.System.EnemySpawnSystem
{
    public class TriangulationSpawner : MonoBehaviour
    {
        [Inject] IPoolController _pool;
        [Inject] ICoroutineRunner _coroutineRunner;
        
        [Header("Refs")]
        [SerializeField] private Transform _player; // игрок (для проверки дистанции)
        [SerializeField] private GameObject _enemyPrefab; // префаб врага

        [Header("NavMesh")]
        [SerializeField] private int _areaMask = NavMesh.AllAreas;
        [SerializeField] private float _snap = 0.15f;

        [Header("Лимиты")]
        [SerializeField] private float _minDistanceFromPlayer = 1f;
        [SerializeField] private float _maxDistanceFromPlayer = 10f;
        [SerializeField] private int _tries = 500;

        [Header("Включение")]
        [SerializeField] private bool On = false;
        
        // кэш триангуляции
        private NavMeshTriangulation _tri;
        private float[] _cdf;
        private float _total;

        private void Awake() => RebuildTriangulation();
        private void OnEnable()
        {
            MaybeRefreshIfEmpty();

            if (_player == null)
            {
                var playerComp = FindAnyObjectByType<PlayerTag>(FindObjectsInactive.Include);
                if (playerComp != null) _player = playerComp.transform;
            }
        }

        private void Update()
        {
            // Плюс на верхней строке (обычно '=') и на NumPad
            // if (UnityEngine.Input.GetKeyDown(KeyCode.KeypadPlus) || UnityEngine.Input.GetKeyDown(KeyCode.Equals))
            //     TrySpawn();

            if (UnityEngine.Input.GetKeyDown(KeyCode.KeypadPlus) || UnityEngine.Input.GetKeyDown(KeyCode.Equals))
            {
                On = !On;
                if(On) _coroutineRunner.StartCoroutine(ClickSpawn());
            }
        }

        private IEnumerator ClickSpawn()
        {
            while (On)
            {
                TrySpawn();
                yield return new WaitForSeconds(0.1f);
            }
        }
        

        [ContextMenu("Rebuild Triangulation")]
        public void RebuildTriangulation()
        {
            _tri = NavMesh.CalculateTriangulation();
            BuildAreasCDF();
        }

        private void MaybeRefreshIfEmpty()
        {
            if (_tri.indices == null || _tri.indices.Length < 3)
                RebuildTriangulation();
        }

        private void BuildAreasCDF()
        {
            if (_tri.indices == null || _tri.indices.Length < 3)
            {
                _cdf = null;
                _total = 0f;
                return;
            }

            int triCount = _tri.indices.Length / 3;
            _cdf = new float[triCount];
            _total = 0f;

            for (int t = 0; t < triCount; t++)
            {
                var a = _tri.vertices[_tri.indices[t * 3 + 0]];
                var b = _tri.vertices[_tri.indices[t * 3 + 1]];
                var c = _tri.vertices[_tri.indices[t * 3 + 2]];

                // площадь треугольника
                float area = Vector3.Cross(b - a, c - a).magnitude * 0.5f;
                _total += Mathf.Max(area, 1e-6f);
                _cdf[t] = _total;
            }
        }

        public bool TrySpawn()
        {
            Debug.Log("TrySpawn");
            if (_enemyPrefab == null || _player == null || _cdf == null || _cdf.Length == 0)
                return false;

            for (int i = 0; i < _tries; i++)
            {
                if (!TryPickPointOnNavMesh(out var pos)) continue;

                // проверка расстояния по XZ
                if (HorizontalDistance(pos, _player.position) < _minDistanceFromPlayer)
                    continue;

                if (HorizontalDistance(pos, _player.position) > _maxDistanceFromPlayer)
                    continue;
             
                var go = _pool.GetPool<Target>().GetElement();
                   
                if (go.TryGetComponent<NavMeshAgent>(out var agent))
                    agent.Warp(pos);
                else
                    go.transform.position = pos;

                return true;
            }

            return false;
        }

        private bool TryPickPointOnNavMesh(out Vector3 pos)
        {
            pos = default;

            // Выбор треугольника пропорционально площади
            float r = Random.value * _total;
            int t = global::System.Array.FindIndex(_cdf, v => r <= v);
            if (t < 0) t = _cdf.Length - 1;

            int i0 = _tri.indices[t * 3 + 0];
            int i1 = _tri.indices[t * 3 + 1];
            int i2 = _tri.indices[t * 3 + 2];

            var a = _tri.vertices[i0];
            var b = _tri.vertices[i1];
            var c = _tri.vertices[i2];

            // равномерная по площади барицентрическая точка
            float r1 = Mathf.Sqrt(Random.value);
            float r2 = Random.value;
            Vector3 p = a * (1 - r1) + b * (r1 * (1 - r2)) + c * (r1 * r2);

            // «прилипание» к навмешу маленьким радиусом (чтобы не утянуло далеко)
            if (!NavMesh.SamplePosition(p, out var hit, _snap, _areaMask))
                return false;

            pos = hit.position + Vector3.up * 0.05f; // слегка приподнять от пола
            return true;
        }

        private static float HorizontalDistance(Vector3 a, Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }
    }
}
