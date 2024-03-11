using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _menuTransform;
    [SerializeField] private float _animationDuration = 0.5f;

    private bool _isPaused = false;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        EventSystem.current.SetSelectedGameObject(null);

        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        OnCancel();
    }

    public void OnCancel()
    {
        if (_isPaused)
        {
            ResumeGame();
            return;
        }
        ShowMenu();
    }

    public void ShowMenu()
    {
        if (_isPaused) return;

        _isPaused = true;

        _canvasGroup.DOFade(1, _animationDuration)
            .SetUpdate(true);

        _menuTransform.DOAnchorPos(Vector2.zero, _animationDuration)
            .From(new Vector2(-1920, 0))
            .SetDelay(_animationDuration)
            .SetEase(Ease.InOutExpo)
            .SetUpdate(true);

        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        EventSystem.current.SetSelectedGameObject(_menuTransform.gameObject);
    }

    public void HideMenu()
    {
        if (!_isPaused) return;

        _isPaused = false;

        _canvasGroup.DOFade(0, _animationDuration)
        .SetDelay(_animationDuration)
        .SetUpdate(true);

        _menuTransform.DOAnchorPos(new Vector2(+1920, 0), _animationDuration)
            .From(Vector2.zero)
            .SetEase(Ease.InOutExpo)
            .SetUpdate(true);

        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ResumeGame()
    {
        HideMenu();
    }

    public void QuitGame()
    {
        UIUtils.LoadSceneWithDelay("MainMenu", 0.1f);
    }

    public void RestartGame()
    {
        UIUtils.LoadSceneWithDelay("MainGame", 0.1f);
    }


}
