using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float _speed;
    private float _lifeTime;
    private int _damage;
    private float _timeLeft;
    private Transform _target;
    private Vector3 _targetDirection;

    public void SetBulletData(float speed, float lifeTime, int damage, Transform target)
    {
        _speed = speed;
        _lifeTime = lifeTime;
        _damage = damage;
        _target = target;
        transform.LookAt(target);
        _targetDirection =  (target.position - transform.position).normalized;
    }

    // Start is called before the first frame update
    void Start()
    {
        _timeLeft = _lifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timeLeft -= Time.deltaTime * GameManager.Instance.GameTimeScale;

        if(_target == null || _timeLeft <= 0)
        {
            Destroy(gameObject);
            return;
        }

        transform.position += _targetDirection * _speed * Time.deltaTime * GameManager.Instance.GameTimeScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.GetComponentInParent<EnemyController>().TakeDamage(_damage);
            Destroy(gameObject);
        }
    }

}
