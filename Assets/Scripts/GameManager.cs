using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Transform _topBorder;
    [SerializeField] private Transform _bottomBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _leftBorder;

    private void Start()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        _topBorder.localScale = new Vector3(_enemyManager.totalHorizDistance + _leftBorder.localScale.x, _topBorder.localScale.y, _topBorder.localScale.z);
    }
}
