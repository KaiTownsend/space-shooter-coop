using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
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
        _healthText.text = $"Health: {_playerController.health}/{maxHealth}";
    }

    public void UpdateMagText(int bulletCount, int magSize)
    {
        _magText.text = $"Mag: {bulletCount}/{magSize}";
    }
}
