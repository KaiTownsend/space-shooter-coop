using TMPro;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("TextMeshPro")]
    [SerializeField] private TMP_Text _gameStatusText;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _magText;

    [Header("Upgrades")]
    [SerializeField] private PlayerUpgradableData[] _playerUpgradableData;
    [SerializeField] private Button[] _upgradeButtons;
    private int[] _chosenUpgradeIndexes;

    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _chosenUpgradeIndexes = new int[3];
        EnableLevelUpScreen();
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

    public void EnableLevelUpScreen()
    {
        SelectRandomUpgrades();
        // display upgrades on screen by looping through the scriptableobject array and showing 3 random upgrades as buttons
    }

    public void ResumeGameAndLevelUpPlayer()
    {
        
    }

    private void SelectRandomUpgrades()
    {   
        _chosenUpgradeIndexes = GetUniqueRandomNumbersShuffle(0, 2, 3).ToArray();
        for (int i = 0; i < 3; i++)
        {
            _upgradeButtons[i].GetComponentInChildren<TMP_Text>().text = _playerUpgradableData[_chosenUpgradeIndexes[i]].UpgradeName;
        }
    }

    private static readonly System.Random _random = new System.Random();

    public static List<int> GetUniqueRandomNumbersShuffle(int min, int max, int count) // come back and try to understand this later !!!!
    {
        int rangeSize = max - min + 1;
        if (count > rangeSize)
        {
            throw new ArgumentException("Count cannot be greater than the available range of numbers.");
        }

        // 1. Populate the pool with all numbers in the range
        List<int> numberPool = Enumerable.Range(min, rangeSize).ToList();

        // 2. Fisher-Yates Shuffle algorithm
        for (int i = numberPool.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            // Swap elements
            int temp = numberPool[i];
            numberPool[i] = numberPool[j];
            numberPool[j] = temp;
        }

        // 3. Take the first 'count' numbers from the shuffled list
        return numberPool.Take(count).ToList();
    }
}
