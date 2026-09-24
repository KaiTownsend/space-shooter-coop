using UnityEngine;

public class GameManager : MonoBehaviour
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
    private int _currentLevel;

    [HideInInspector] public bool isGamePaused;
    private PlayerController _playerController;
    private float _gameTime;
    

    private void Awake()
    {
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    

    private void Start()
    {
        SetupBorders();
        isGamePaused = true;
    }

    private void Update()
    {
        //if and else if to be deleted once I have a button setup for a menu or such.
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

        if (isGamePaused) return;

        _gameTime += Time.deltaTime;
        _uiManager.UpdateTimerText(_gameTime);

        if (_gameTime % 30 == 0 && _gameTime != 0)
        {
            _currentLevel++;
            ResetAndPause();
            _uiManager.EnableLevelUpScreen();
            isGamePaused = true;
        }
    }

    private void SetupBorders()
    {
        _topBorder.localScale = new Vector3(_enemyManager.totalHorizDistance - _leftBorder.localScale.x, _topBorder.localScale.y);
        _bottomBorder.localScale = new Vector3(_enemyManager.totalHorizDistance - _leftBorder.localScale.x, _topBorder.localScale.y);
        _rightBorder.localPosition = new Vector3(_enemyManager.totalHorizDistance/2, _rightBorder.localPosition.y);
        _leftBorder.localPosition = new Vector3(-_enemyManager.totalHorizDistance/2, _rightBorder.localPosition.y);
    }

    public void UpdateGameState()
    {
        if (_playerController.health <= 0)
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isGamePaused = true;

        _uiManager.UpdateGameStatusText(isGamePaused);
    }

    private void ResumeGame()
    {
        isGamePaused = false;

        _uiManager.UpdateGameStatusText(isGamePaused);
    }

    private void ResetAndPause()
    {
        PauseGame();

        _playerController.ResetPlayerAndBullets();
        _playerController.ReloadTimer = 0f;

        _enemyManager.ResetEnemies();
        _enemyManager.updateTimer = 0f;
    }
}
