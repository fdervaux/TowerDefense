using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private TowerData towerData;
    private float timeLeftForNextAttack;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private bool _isActive = false;

    public void SetActive(bool isActive)
    {
        _isActive = isActive;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Attack(EnemyController enemy)
    {
        timeLeftForNextAttack -= Time.deltaTime * GameManager.Instance.GameTimeScale;

        if(timeLeftForNextAttack > 0)
        {
            return;
        }

        timeLeftForNextAttack = towerData.attackSpeed;

        //enemy.TakeDamage(towerData.damage);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.SetBulletData(towerData.bulletSpeed, towerData.bulletLifeTime, towerData.damage, enemy.GetBodyTarget());

        //GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isActive)
        {
            return;
        }

        EnemyController enemy = GameManager.Instance.GetNearestEnemiesInZone(transform.position, towerData.attackRange);

        if(enemy == null)
        {
            return;
        }

        transform.LookAt(enemy.transform);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        Attack(enemy);
    }
}
