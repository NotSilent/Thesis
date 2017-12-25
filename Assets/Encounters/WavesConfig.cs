using UnityEngine;

public class WavesConfig : ScriptableObject
{
    [System.Serializable]
    public class Wave
    {
        public int numberOfEnemies;
        public float speed;
        public int damage;
        public float rateOfFire;
    }

    public Wave[] waves;
}