using System.Collections;
using UnityEngine;

// every interval / cooldown they move down one enemy length. 
// they also regenerate at a certain pace that accelerates as game time goes on (add a visual timer for that and set a limit so they don't go off screen)
// clamp ship movements to be within the enemies so you cant just escape entirely
// make main menu with same scene
// animate enemies
// if enemies collide with ship, then lose health, change sprite state, and eventually lose/disable.

// CRUCIAL BUG FIX: when the enemies respawn they mirror the position of the eliminated enemies
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Transform _enemySpawnTransform;
    [SerializeField] private float _updateCooldown = 3f;
    [SerializeField] private float _movementDist = 0.25f;
    private GameObject[] _enemyList;
    private int _rowSize = 10;
    private int _colSize = 8;
    private float _offsetDist = 0.25f;
    private float _updateTimer;
    
    private void Awake()
    {
        _enemyList = new GameObject[_colSize*_rowSize];
    }

    private void Start()
    {
        CreateEnemies();
    }

    private void Update()
    {
        _updateTimer += Time.deltaTime;

        if (_updateTimer >= _updateCooldown)
        {
            MoveAll();
            RespawnTopRow();
            _updateTimer = 0f;
        }
    }

    private void CreateEnemies()
    {
        for (int i = 0; i < _enemyList.Length; i++)
        {
            GameObject enemyPrefab = Instantiate(_enemyPrefab, _enemyContainer);
            _enemyList[i] = enemyPrefab;

            enemyPrefab.transform.position = GetSpawnPosition(i/_rowSize, i%_rowSize);
        }
    }

    private Vector3 GetSpawnPosition(int row, int column)
    {
        return new Vector3(_enemySpawnTransform.position.x + _offsetDist * column, _enemySpawnTransform.position.y - _offsetDist * row, _enemySpawnTransform.position.z);
    }

    private void MoveAll()
    {
        foreach (GameObject enemyToMove in _enemyList)
        {
            enemyToMove.transform.position += new Vector3(0, -_movementDist, 0);
        }
    }


    private void RespawnTopRow()
    {
        int enemiesRespawned = 0;

        for (int j = 0; j < _enemyList.Length; j++)
        {
            if (enemiesRespawned >= _rowSize)
            {
                break;
            }

            if (_enemyList[j].activeInHierarchy == false)
            {
                Vector3 spawnPosition = GetSpawnPosition(0, enemiesRespawned);
                Debug.Log(spawnPosition);

                    _enemyList[j].GetComponent<EnemyController>().Reactivate(spawnPosition);
                    Debug.Log($"respawned enemy at index: {j}");
                    
                enemiesRespawned++;

            }
        }
    }
}
