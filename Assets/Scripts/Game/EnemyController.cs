using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Declare any variables or properties here

    private void Start()
    {
        transform.DOMoveX(transform.position.x + 100, 20).SetEase(Ease.Linear).SetUpdate(UpdateType.Manual);

        // Initialization code goes here
    }

    private void Update()
    {
        GameManager gameManager = GameManager.Instance;
        //transform.Translate(Vector3.right * Time.deltaTime * gameManager.GameTimeScale);

        

        // Update code goes here
    }
}