using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Transform _enemySpawnTransform;
    [SerializeField] private AudioManager _audioManager;

    [Header("Enemy Group Config")]
    public float updateCooldown = 3f;
    [SerializeField] private float _movementDist = 0.25f;
    [SerializeField] private int _rowSize = 10;
    [SerializeField] private int _colSize = 8;
    
    [HideInInspector] public float totalHorizDistance;
    [HideInInspector] public float updateTimer;
    private GameManager _gameManager;
    private EnemyController[] _enemyList;
    private float _offsetDist = 0.25f; // this is also the dist from enemy spawn to center of L/R border
    
    private void Awake()
    {
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _enemyList = new EnemyController[_colSize*_rowSize];
        _enemySpawnTransform.transform.position = new Vector3(_offsetDist/2f * (-_rowSize + 1f), 2.85f, 0f);
        totalHorizDistance = Mathf.Abs(2*(_enemySpawnTransform.transform.position.x - _offsetDist));
    }

    private void Start()
    {
        CreateEnemies();
    }

    private void Update()
    {
        if (_gameManager.IsGamePaused)
        {
            return;
        }

        updateTimer += Time.deltaTime;

        if (updateTimer >= updateCooldown)
        {
            MoveAll();
            RespawnTopRow();
            updateTimer = 0f;
        }
    }

    public void ResetEnemies()
    {
        for (int i = 0; i < _enemyList.Length; i++)
        {
            _enemyList[i].gameObject.SetActive(true);
            _enemyList[i].transform.position = GetSpawnPosition(i/_rowSize, i%_rowSize);
        }
    }

    public void AddMaxHealth(float healthToAdd)
    {
        foreach (EnemyController enemy in _enemyList)
        {
            enemy.maxHealth += healthToAdd;
        }
    }

    private void CreateEnemies()
    {
        for (int i = 0; i < _enemyList.Length; i++)
        {
            EnemyController enemy = Instantiate(_enemyPrefab, _enemyContainer);
            _enemyList[i] = enemy;

            enemy.SetAudioManager(_audioManager);
            enemy.transform.position = GetSpawnPosition(i/_rowSize, i%_rowSize);
        }
    }

    private Vector3 GetSpawnPosition(int row, int column)
    {
        return new Vector3(_enemySpawnTransform.position.x + _offsetDist * column, _enemySpawnTransform.position.y - _offsetDist * row, _enemySpawnTransform.position.z);
    }

    private void MoveAll()
    {
        foreach (EnemyController enemy in _enemyList)
        {
            enemy.transform.position += new Vector3(0, -_movementDist, 0);
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

            if (_enemyList[i].gameObject.activeInHierarchy == false)
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
