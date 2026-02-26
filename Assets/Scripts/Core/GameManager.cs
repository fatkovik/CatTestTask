using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameLostPanel;

    [SerializeField] private TMP_Text livesCounter;
    [SerializeField] private int totalLives;
    [SerializeField] private CatSpotManager catSpotManager;

    private int _currentLives;

    void Start()
    {
        _currentLives = totalLives;
        catSpotManager.onWrongSpotClicked += OnLifeLost;
        catSpotManager.onAllCatsClicked += OnGameWin;
    }

    private void OnGameWin()
    {
        if (_currentLives > 0)
        {
            gameWinPanel.SetActive(true);
        }
    }

    private void OnLifeLost()
    {
        _currentLives--;
        SetLivesText();

        if (_currentLives <= 0)
        {
            gameLostPanel.SetActive(true);
        }
    }

    private void SetLivesText()
    {
        livesCounter.text = $"{_currentLives}";
    }

    private void OnDisable()
    {
        catSpotManager.onWrongSpotClicked -= OnLifeLost;
        catSpotManager.onAllCatsClicked -= OnGameWin;
    }
}
