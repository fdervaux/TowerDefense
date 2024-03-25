using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{   
    public string enemyName = "Enemy";
    public int health = 100;
    public int speed = 1;
    public GameObject prefab;

    public int reward = 100;

}

