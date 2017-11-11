using UnityEngine;

[CreateAssetMenu(fileName = "Levels", menuName = "Thesis/Levels")]
public class Levels : ScriptableObject
{
    [SerializeField] float[] requiredExperience;

    public float MaxLevel
    {
        get { return requiredExperience.Length; }
    }

    public float GetNextLevelRequirement(int level)
    {
        if (level < MaxLevel)
            return requiredExperience[level];
        else
            return float.PositiveInfinity;
    }
}