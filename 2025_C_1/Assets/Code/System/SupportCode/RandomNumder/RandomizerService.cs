using UnityEngine;
namespace CodeBase.System.Services.Utilities.RandomNumder
{
    public class RandomizerService : IRandomizerService
    {
        public int GetRandomValue(int min, int max)
        {
            return UnityEngine.Random.Range(min, max); // max не включается
        }

        public float GetRandomValue(float min, float max)
        {
            return UnityEngine.Random.Range(min, max); // max включается
        }

        public Vector2 GetRandomValue(Vector2 min, Vector2 max)
        {
            return new Vector2(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y)
            );
        }

        public Vector3 GetRandomValue(Vector3 min, Vector3 max)
        {
            return new Vector3(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z)
            );
        }
    }
}
