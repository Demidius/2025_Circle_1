using UnityEngine;
namespace CodeBase.System.Services.Utilities.RandomNumder
{
    public interface IRandomizerService
    {
        int     GetRandomValue(int min, int max);
        float   GetRandomValue(float min, float max);
        Vector2 GetRandomValue(Vector2 min, Vector2 max);
        Vector3 GetRandomValue(Vector3 min, Vector3 max);
    }
}
