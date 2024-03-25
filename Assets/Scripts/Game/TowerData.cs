using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Tower/TowerData", order = 1)]
public class TowerData : ScriptableObject
{
    public string towerName = "Tower";
    public int damage = 10;
    public float attackSpeed = 1;
    public float attackRange = 1;

    public float bulletSpeed = 10;
    public float bulletLifeTime = 2;

    public int cost = 100;
}
