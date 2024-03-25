using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;



public class EnemyController : MonoBehaviour
{
    private float _speed = 1;
    private int _health = 1;
    private string _name = "default";
    private List<Transform> _path;
    private Rigidbody _rigidbody;
    private int _currentPathIndex;
    private int _reward;

    private UnityEvent<EnemyController> _onPathEnd = new UnityEvent<EnemyController>();
    private UnityEvent<EnemyController> _onDeath = new UnityEvent<EnemyController>();
    public UnityEvent<EnemyController> OnDeath => _onDeath;
    public UnityEvent<EnemyController> OnPathEnd => _onPathEnd;

    [SerializeField] private TextMeshPro _healthText;
    [SerializeField] private Transform _bodyTarget;

    public int Reward => _reward;

    public void SetupEnemy(EnemyData enemyData, List<Transform> path)
    {
        _speed = enemyData.speed;
        _health = enemyData.health;
        _name = enemyData.enemyName;
        _path = path;
        _currentPathIndex = 0;
        _reward = enemyData.reward;
    }

    public Transform GetBodyTarget()
    {
        return _bodyTarget;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _healthText.text = _health.ToString();
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _healthText.text = _health.ToString();
        if(_health <= 0)
        {
            _onDeath.Invoke(this);
            Destroy(gameObject);
        }
    }

    private void Update()
    {       
        if(Vector3.Distance(transform.position, _path[_currentPathIndex].position) < 0.05f)
        {
            _currentPathIndex++;
        }

        if(_currentPathIndex >= _path.Count)
        {
            Destroy(gameObject);
            _onPathEnd.Invoke(this);
            return;
        }

        Vector3 currentTarget = _path[_currentPathIndex].position;
        Vector3 direction = (currentTarget - transform.position).normalized;
        _rigidbody.velocity = direction * _speed * GameManager.Instance.GameTimeScale;
    }
}