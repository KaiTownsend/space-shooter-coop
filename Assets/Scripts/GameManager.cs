using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private UIManager _uiManager;
    
    [Header("Border Transforms")]
    [SerializeField] private Transform _topBorder;
    [SerializeField] private Transform _bottomBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _leftBorder;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isGamePaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            isGamePaused = false;
        }

        if (!isGamePaused)
        {
            _gameTime += Time.deltaTime;
            _uiManager.UpdateTimerText(_gameTime);
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
            // pause game / game over?
        }

        if (!isGamePaused)
        {
            PauseGame();
            
        }
    }

    private void PauseGame()
    {
        isGamePaused = true;
    }
}
