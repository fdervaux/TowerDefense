using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{   
    public string enemyName = "Enemy";
    public int PV_Number = 100;
    public int speed = 1;
    public GameObject prefab;

}

