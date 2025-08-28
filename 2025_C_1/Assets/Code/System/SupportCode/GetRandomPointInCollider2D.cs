using UnityEngine;
namespace CodeBase.System.Services.Utilities
{
    public class GetRandomPointInCollider2D
    {
        public Vector2 GetPoint(Collider2D _areaCollider)
        {
            int maxAttempts = 100;
            for (int i = 0; i < maxAttempts; i++)
            {
                Bounds bounds = _areaCollider.bounds;
                float randomX = Random.Range(bounds.min.x, bounds.max.x);
                float randomY = Random.Range(bounds.min.y, bounds.max.y);
                Vector2 point = new Vector2(randomX, randomY);

                if (_areaCollider.OverlapPoint(point))
                {
                    return point;
                }
            }
            return _areaCollider.bounds.center;
        }
    }
}
