using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;

public class GameManager : NetworkBehaviour
{
    [Header("Main")]
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private UIManager _uiManager;
    
    [Header("Border Transforms")]
    [SerializeField] private Transform _topBorder;
    [SerializeField] private Transform _bottomBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _leftBorder;

    [Header("Level Variables")]
    [SerializeField] private float _levelTimeInterval = 30f;
    [SerializeField] private int _levelsToWin = 20;

    [SyncVar] private int _currentLevel; // need a way to "get" this despite syncvar making me make this private.
    [SyncVar] private float _gameTime;
    [SyncVar] private float _levelTimer;
    [SyncVar] private bool _isGamePaused;

    public bool IsGamePaused => _isGamePaused;

    private List<PlayerController> _playerList = new List<PlayerController>();

    private void Start()
    {
        SetupBorders();
    }

    private void Update()
    {
        //if and else if to be deleted once I have a button setup for a menu or such.
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     PauseGame();
        // }
        // else if (Input.GetKeyDown(KeyCode.O))
        // {
        //     ResumeGame();
        // }
        // else if (Input.GetKeyDown(KeyCode.R))
        // {
        //     ResetAndPause();
        // }

        _uiManager.UpdateTimerText(_gameTime);

        if (!isServer) return; // cant just make the whole update server because UI needs to update on client before

        Debug.Log("Player count" + _playerList.Count);

        if (_isGamePaused) return;

        _gameTime += Time.deltaTime;
        _levelTimer += Time.deltaTime;

        if (_levelTimer >= _levelTimeInterval)
        {
            NextLevel();
        }
    }

    [Server]
    public void UpdateGameState()
    {
        foreach (PlayerController player in _playerList)
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
        _playerList.Add(playerToAdd);
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

    // private IEnumerator WaitForPlayer()
    // {
    //     while (_playerController == null)
    //     {
    //         GameObject player = GameObject.FindWithTag("Player");
    //         if (player != null)
    //         {
    //             _playerController = player.GetComponent<PlayerController>();
    //         }
    //         yield return null;
    //     }
    // }
    
    [Server]
    private void ResetAndPause()
    {
        PauseGame();

        foreach (PlayerController player in _playerList)
        {
            player.ResetPlayerAndBullets();
            player.ReloadTimer = 0f;
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

    private void SetupBorders()
    {
        _topBorder.localScale = new Vector3(_enemyManager.totalHorizDistance - _leftBorder.localScale.x, _topBorder.localScale.y);
        _bottomBorder.localScale = new Vector3(_enemyManager.totalHorizDistance - _leftBorder.localScale.x, _topBorder.localScale.y);
        _rightBorder.localPosition = new Vector3(_enemyManager.totalHorizDistance/2, _rightBorder.localPosition.y);
        _leftBorder.localPosition = new Vector3(-_enemyManager.totalHorizDistance/2, _rightBorder.localPosition.y);
    }
}
