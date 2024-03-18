using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class EnemyWave
{
    public EnemyData EnemyData;
    public float SpawnTimeAfterPrevious;
}



[CreateAssetMenu(fileName = "EnemyWaveData", menuName = "Enemy/EnemyWaveData", order = 2)]
public class EnemyWaveData : ScriptableObject
{
    public List<EnemyWave> EnemiesData = new List<EnemyWave>();
}

