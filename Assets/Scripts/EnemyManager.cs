using System.Collections.Generic;
using Unity.Collections.Tests.CoreCLR.TestJobs;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Transform _enemySpawnTransform;
    [SerializeField] private float _updateCooldown = 3f;
    [SerializeField] private float _movementDist = 0.25f;
    private GameObject[] _enemyList;
    private int _rowSize = 20;
    private int _colSize = 8;
    private float _offsetDist = 0.25f; // this is also the dist from enemy spawn to center of L/R border
    private float _updateTimer;
    
    private void Awake()
    {
        _enemyList = new GameObject[_colSize*_rowSize];
        _enemySpawnTransform.transform.position = new Vector3(_offsetDist/2f * (-_rowSize + 1f), 2.85f, 0f);
        // float totalHorizDistance = _offsetDist*(_rowSize+1) + _enemyPrefab.GetComponent<BoxCollider2D>().size.x;
        // Debug.Log(totalHorizDistance);
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
        List<int> randomNumsList = new List<int>();
        for (int i = 0; i <= _rowSize-1; i++)
        {
            randomNumsList.Add(i);
        }

        int enemiesRespawned = 0;
        for (int i = 0; i < _enemyList.Length; i++)
        {
            if (enemiesRespawned >= _rowSize)
            {
                break;
            }

            if (_enemyList[i].activeInHierarchy == false)
            {
                int randomIndex = Random.Range(0, randomNumsList.Count);
                int uniqueNum = randomNumsList[randomIndex];
                randomNumsList.RemoveAt(randomIndex);
                
                Vector3 spawnPosition = GetSpawnPosition(0, uniqueNum);
                _enemyList[i].GetComponent<EnemyController>().Reactivate(spawnPosition);
                    
                enemiesRespawned++;
            }
        }
    }
}
