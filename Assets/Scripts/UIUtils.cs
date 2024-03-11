using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIUtils
{
    public static void LoadSceneWithDelay(string sceneName, float delay)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.SetDelay(delay).OnComplete(
            () => SceneManager.LoadScene(sceneName)
            ).SetUpdate(true);
    }
    
}
