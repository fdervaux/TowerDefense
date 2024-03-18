using DG.Tweening;
using UnityEngine;



public class EnemyController : MonoBehaviour
{
    

    private void Start()
    {

    }

    private void Update()
    {   
        transform.position += Vector3.right * Time.deltaTime * GameManager.Instance.GameTimeScale * 5;
    }
}