using UnityEngine;
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

    [SyncVar] private int _currentLevel;
    [SyncVar] private float _gameTime;
    [SyncVar] private float _levelTimer;
    [SyncVar] private bool _isGamePaused;

    public bool IsGamePaused => _isGamePaused;

    private List<PlayerController> _playerList = new List<PlayerController>();

    // BULLET VARS FROM DELETED SERVER MANAGER CS
    // [SerializeField] private Transform _bulletContainer;
    public List<BulletController> BulletList = new List<BulletController>(); // leaving uncommented so can compile. playercontroller references numerous times
    // PLAN TO TURN THESE INTO LISTS/DICT FOR EACH PLAYER (?)
    // [SerializeField] private int _magSize = 1;
    // [SerializeField] private float _bulletSize = 3f;
    // [SerializeField] private float _bulletSpread = 0f;
    // [SerializeField] private int _bulletBounces = 0;
    // [SerializeField] private float _shootCooldown = 0.25f;
    // [SerializeField] private BulletController _bulletPrefab;

    private void Start()
    {
        SetupBorders();
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
            else if (Input.GetKeyDown(KeyCode.R))
            {
                ResetAndPause();
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

    // DYSFUNCTIONAL BULLET LOGIC FROM DELETED SERVER MANAGER CS
    // private void CreateAndUpdateBullets()
    // {
    //     for (int i = 0; i < (_magSize - BulletList.Count); i++)
    //     {
    //         BulletController bulletController = Instantiate(_bulletPrefab, _bulletContainer);
    //         BulletList.Add(bulletController);
    //         NetworkServer.Spawn(bulletController.gameObject);
    //         bulletController.gameObject.SetActive(false);
    //     }

    //     foreach (BulletController bullet in BulletList)
    //     {
    //         bullet.transform.localScale = new Vector3(_bulletSize, _bulletSize, _bulletSize);
    //         bullet.bulletSpread =  _bulletSpread;
    //         bullet.bulletBounces = _bulletBounces;
    //     }

    //     // RecountAvailableBullets();
    // }
}
