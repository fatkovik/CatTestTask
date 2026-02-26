using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text livesCounter;
    [SerializeField] private int totalLives;
    [SerializeField] private CatSpotManager catSpotManager;

    private int _currentLives;

    void Start()
    {
        _currentLives = totalLives;
        catSpotManager.onWrongSpotClicked += OnLifeLost;
    }

    private void OnLifeLost()
    {
        _currentLives--;
        SetLivesText();

        if (_currentLives <= 0)
        {
            Debug.Log("Game Over!");
        }
    }

    private void SetLivesText()
    {
        livesCounter.text = $"{_currentLives}";
    }

    private void OnDisable()
    {
        catSpotManager.onWrongSpotClicked -= OnLifeLost;
    }
}
