using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("TextMeshPro")]
    [SerializeField] private TMP_Text _gameStatusText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _magText;
    

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    public void UpdateTimerText(float currentTime)
    {
        float minutes = (int)currentTime/60;
        float seconds = (int)currentTime%60;

        _timerText.text = $"{minutes}m {seconds}s";
    }

    public void UpdateHealthText(float maxHealth)
    {
        _healthText.text = $"HP: {_playerController.health}/{maxHealth}";
    }

    public void UpdateMagText(int bulletCount, int magSize)
    {
        _magText.text = $"Mag: {bulletCount}/{magSize}";
    }

    public void UpdateGameStatusText(bool isGamePaused)
    {
        if (isGamePaused)
        {
            _gameStatusText.text = "Game Paused \n- Press O to Resume";
        }
        else if (!isGamePaused)
        {
            _gameStatusText.text = "Game Running \n- Press P to Pause";
        }
    }
}
