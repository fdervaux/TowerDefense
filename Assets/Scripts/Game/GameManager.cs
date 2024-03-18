using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Ui Panel")]
    [SerializeField] private CanvasGroup _startGameBackgroundPanel;
    [SerializeField] private RectTransform _startGameOverPanel;

    private float _gameTimeScale = 1;
    public float GameTimeScale  {
        get => _gameTimeScale;
    }


    public void StartGame()
    {
        Sequence sequence = DOTween.Sequence();

        _startGameOverPanel.anchoredPosition = new Vector2(-1920, 0);
        _startGameBackgroundPanel.interactable = true;
        _startGameBackgroundPanel.blocksRaycasts = true;

        
        _gameTimeScale = 0;
        

        sequence.AppendCallback(() => {
            _startGameOverPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Get Ready";
        });
        sequence.Append(_startGameBackgroundPanel.DOFade(1, 0.5f).From(0));
        sequence.Append(_startGameOverPanel.DOAnchorPosX(0, 0.3f).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.3f);

        sequence.Append(_startGameOverPanel.DOAnchorPosX(1920, 0.5f).SetEase(Ease.InBack));
        sequence.Append(_startGameBackgroundPanel.DOFade(0, 0.3f));
        sequence.OnComplete(() => {
            _startGameBackgroundPanel.interactable = false;
            _startGameBackgroundPanel.blocksRaycasts = false;
            _gameTimeScale = 1;
        });


    }

    public void Start()
    {
        StartGame();
    }


    public void Update()
    {
        DOTween.ManualUpdate(Time.deltaTime * _gameTimeScale, Time.unscaledDeltaTime * _gameTimeScale);
    }


}

