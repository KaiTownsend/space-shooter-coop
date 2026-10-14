using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

public class GameManager : NetworkBehaviour
{
    [HideInInspector] public List<PlayerController> PlayerList = new List<PlayerController>();
    [HideInInspector] public List<BulletController> BulletList = new List<BulletController>();

    [Header("Main")]
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private UIManager _uiManager;

    [Header("Border Transforms")]
    [SerializeField] private Transform _topBorder;
    [SerializeField] private Transform _bottomBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _leftBorder;

    [Header("Time/Level Variables")]
    [SerializeField] private float _levelTimeInterval = 30f;
    [SerializeField] private int _levelsToWin = 20;

    [SyncVar] private int _currentLevel;
    [SyncVar] private float _gameTime;
    [SyncVar] private float _levelTimer;
    [SyncVar] private bool _isGamePaused = true;
    public bool IsGamePaused => _isGamePaused;

    [SyncVar] private float _countdownTimer = 3f;

    private void Start()
    {
        SetupBorders();
        StartCoroutine(WaitForCountdown());
    }

    private void Update()
    {
        _uiManager.UpdateTimerText(_gameTime);

        if (!isServer) return; // cant just make the whole update server because UI needs to update on client before

        if (isClient)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                ResumeGame();
            }
        }

        if (_isGamePaused) return;

        _gameTime += Time.deltaTime;
        _levelTimer += Time.deltaTime;

        if (_levelTimer >= _levelTimeInterval)
        {
            NextLevel();
        }
    }

    private IEnumerator WaitForCountdown()
    {
        while (_countdownTimer > 0)
        {
            _countdownTimer -= Time.deltaTime;
            _uiManager.UpdateCountdownText(_countdownTimer);
            yield return null;
        }

        _isGamePaused = false;
        _uiManager.UpdateCountdownText(_countdownTimer);
        _uiManager.UpdateGameStatusText(_isGamePaused);
    }

    [Server]
    public void UpdateGameState()
    {
        foreach (PlayerController player in PlayerList)
        {
            if (player.Health <= 0)
            {
                PauseGame();
            }
        }
    }

    [Server]
    public void AddPlayerToList(PlayerController playerToAdd)
    {
        PlayerList.Add(playerToAdd);
    }

    [Server]
    public void PauseGame()
    {
        _isGamePaused = true;

        RpcUpdateGameStatusText(true);
    }

    [Server]
    public void ResumeGame()
    {
        _isGamePaused = false;
        RpcUpdateGameStatusText(false);
    }

    [Server]
    private void ResetAndPause()
    {
        PauseGame();

        foreach (PlayerController player in PlayerList)
        {
            player.ResetPlayerAndBullets();
        }

        _enemyManager.ResetEnemies();
        _enemyManager.updateTimer = 0f;
    }

    [Server]
    private void NextLevel()
    {
        _currentLevel++;

        _enemyManager.updateCooldown -= 0.3f;
        _enemyManager.AddMaxHealth(50f);

        _levelTimer = 0;

        ResetAndPause();
        _isGamePaused = true;
        RpcEnableLevelUpScreen();
    }

    private void SetupBorders()
    {
        _topBorder.localScale = new Vector3(_enemyManager.totalHorizDistance - _leftBorder.localScale.x, _topBorder.localScale.y);
        _bottomBorder.localScale = new Vector3(_enemyManager.totalHorizDistance - _leftBorder.localScale.x, _topBorder.localScale.y);
        _rightBorder.localPosition = new Vector3(_enemyManager.totalHorizDistance / 2, _rightBorder.localPosition.y);
        _leftBorder.localPosition = new Vector3(-_enemyManager.totalHorizDistance / 2, _rightBorder.localPosition.y);
    }

    [ClientRpc]
    private void RpcEnableLevelUpScreen()
    {
        _uiManager.EnableLevelUpScreen();
    }

    [ClientRpc]
    private void RpcUpdateGameStatusText(bool isPaused) // for some reason this only synced up properly when I literally passed in isPaused instead of just using _isGamePaused like before.
    {
        _uiManager.UpdateGameStatusText(isPaused);
    }
}
