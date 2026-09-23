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
        SetupBorders();
    }

    private void SetupBorders()
    {
        _topBorder.localScale = new Vector3(_enemyManager.totalHorizDistance - _leftBorder.localScale.x, _topBorder.localScale.y);
        _bottomBorder.localScale = new Vector3(_enemyManager.totalHorizDistance - _leftBorder.localScale.x, _topBorder.localScale.y);
        _rightBorder.localPosition = new Vector3(_enemyManager.totalHorizDistance/2, _rightBorder.localPosition.y);
        _leftBorder.localPosition = new Vector3(-_enemyManager.totalHorizDistance/2, _rightBorder.localPosition.y);
    }
}
