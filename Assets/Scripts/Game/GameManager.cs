using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    [Header("Game Ui Panel")]
    [SerializeField] private CanvasGroup _startGameBackgroundPanel;
    [SerializeField] private RectTransform _startGameOverPanel;
    [SerializeField] private List<EnemyWaveData> _enemyWaveData = new List<EnemyWaveData>();
    [SerializeField] private PauseMenuController _pauseMenuController;
    [SerializeField] private List<Transform> _enemyPath;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _goldText;

    private List<EnemyController> _spawnedEnemies = new List<EnemyController>();

    private float _gameTimeScale = 1;

    private int _health = 3;
    private int _currentWaveIndex = 0;

    private int _gold = 200;

    public int Gold
    {
        get => _gold;
    }

    public void UpdateGold(int amount)
    {
        _gold += amount;

        if (_gold < 0)
        {
            _gold = 0;
        }

        _goldText.text = _gold.ToString();
    }

    public float GameTimeScale
    {
        get => _gameTimeScale;
    }


    public EnemyController GetNearestEnemiesInZone(Vector3 position, float radius)
    {
        foreach (var enemy in _spawnedEnemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, position);
            if (distance < radius)
            {
                return enemy;
            }
        }

        return null;
    }

    public void PauseGame()
    {
        _gameTimeScale = 0;
    }

    public void ResumeGame()
    {
        _gameTimeScale = 1;
    }

    public void ShowTextOverPanel(string text, Action OnEnd)
    {
        Sequence sequence = DOTween.Sequence();
        _startGameOverPanel.anchoredPosition = new Vector2(-1920, 0);
        _startGameBackgroundPanel.interactable = true;
        _startGameBackgroundPanel.blocksRaycasts = true;
        _gameTimeScale = 0;

        _pauseMenuController.CanPause = false;

        sequence.AppendCallback(() =>
        {
            _startGameOverPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
        });
        sequence.Append(_startGameBackgroundPanel.DOFade(1, 0.5f).From(0));
        sequence.Append(_startGameOverPanel.DOAnchorPosX(0, 0.3f).SetEase(Ease.OutBack));
        sequence.AppendInterval(0.3f);

        sequence.Append(_startGameOverPanel.DOAnchorPosX(1920, 0.5f).SetEase(Ease.InBack));
        sequence.Append(_startGameBackgroundPanel.DOFade(0, 0.3f));
        sequence.OnComplete(() =>
        {
            _startGameBackgroundPanel.interactable = false;
            _startGameBackgroundPanel.blocksRaycasts = false;
            _gameTimeScale = 1;
            _pauseMenuController.CanPause = true;
            OnEnd?.Invoke();
        });
    }


    public void StartWave()
    {
        ShowTextOverPanel("Wave " + (_currentWaveIndex + 1) + " Start !", () =>
        {
            StartCoroutine(SpawnEnemyWave(_enemyWaveData[_currentWaveIndex++]));
        });
    }

    private void RemoveEnemy(EnemyController enemy)
    {
        _spawnedEnemies.Remove(enemy);
        enemy.OnDeath.RemoveListener(OnEnemyDeath);
        enemy.OnPathEnd.RemoveListener(OnPathEnd);

        if (_spawnedEnemies.Count <= 0)
        {
            if (_currentWaveIndex < _enemyWaveData.Count)
            {
                StartWave();
                return;
            }

            if (_health <= 0)
            {
                ShowTextOverPanel("Game Over !", () =>
                {
                    UIUtils.LoadSceneWithDelay("MainMenu", 0.2f);
                });
                return;
            }

            ShowTextOverPanel("You Win !", () =>
            {
                UIUtils.LoadSceneWithDelay("MainMenu", 0.2f);
            });
        }
    }

    private void LoseHealth(int damage)
    {
        _health -= damage;
        _healthText.text = _health.ToString();

        if(_health <= 0)
        {
            ShowTextOverPanel("Game Over !", () =>
            {
                UIUtils.LoadSceneWithDelay("MainMenu", 0.2f);
                
            });
        }
    }

    public void OnEnemyDeath(EnemyController enemy)
    {
        UpdateGold(enemy.Reward);
        RemoveEnemy(enemy);
    }

    public void OnPathEnd(EnemyController enemy)
    {
        RemoveEnemy(enemy);
        LoseHealth(1);
    }

    public void CreateEnemy(EnemyData enemyData, List<Transform> path)
    {
        GameObject enemyObject = Instantiate(enemyData.prefab, path[0].position, Quaternion.identity);
        EnemyController enemy = enemyObject.GetComponent<EnemyController>();
        enemy.SetupEnemy(enemyData, path);
        enemy.OnDeath.AddListener(OnEnemyDeath);
        enemy.OnPathEnd.AddListener(OnPathEnd);
        _spawnedEnemies.Add(enemy);
    }


    public IEnumerator SpawnEnemyWave(EnemyWaveData enemyWaveData)
    {
        foreach (var enemyData in enemyWaveData.EnemiesData)
        {
            CreateEnemy(enemyData.EnemyData, _enemyPath);
            yield return new WaitForSeconds(enemyData.SpawnTimeAfterPrevious);
        }
    }

    public void Start()
    {
        _healthText.text = _health.ToString();
        _goldText.text = _gold.ToString();
        StartWave();
    }


    public void Update()
    {
        DOTween.ManualUpdate(Time.deltaTime * _gameTimeScale, Time.unscaledDeltaTime * _gameTimeScale);
    }


}

