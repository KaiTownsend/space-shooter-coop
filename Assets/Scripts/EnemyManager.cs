using UnityEngine;

// every interval / cooldown they move down one enemy length. 
// they also regenerate at a certain pace that accelerates as game time goes on (add a visual timer for that and set a limit so they don't go off screen)
// clamp ship movements to be within the enemies so you cant just escape entirely
// make main menu with same scene
// animate enemies
// if enemies collide with ship, then lose health, change sprite state, and eventually lose/disable.
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _enemyContainer;
    [SerializeField] private Transform _enemySpawnTransform;
    private GameObject[] _enemyList;
    private int _rowSize = 10;
    private float _offsetDist = 0.25f;
    
    private void Awake()
    {
        _enemyList = new GameObject[8*_rowSize];
    }

    private void Start()
    {
        CreateEnemies();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnEnemy();
        }
    }

    private void CreateEnemies()
    {
        for (int i = 0; i < _enemyList.Length; i++)
        {
            GameObject enemyPrefab = Instantiate(_enemyPrefab, _enemyContainer);
            _enemyList[i] = enemyPrefab;

            enemyPrefab.transform.position = GetSpawnPosition(i%_rowSize, i/_rowSize);
        }
    }

    private Vector3 GetSpawnPosition(int row, int column)
    {
        return new Vector3(_enemySpawnTransform.position.x + _offsetDist * row, _enemySpawnTransform.position.y + _offsetDist * column, _enemySpawnTransform.position.z);
    }

    private void RespawnEnemy()
    {
        for (int i = _enemyList.Length - 1; i >= 0; i--)
        {
            if (_enemyList[i].activeInHierarchy == false)
            {
                Vector3 spawnPosition = GetSpawnPosition(i%_rowSize, i/_rowSize);
                bool isSpawnAvailable = true;
                float enemyRadius = _enemyList[i].GetComponent<BoxCollider2D>().size.x;

                foreach (GameObject otherEnemy in _enemyList)
                {
                    if (Vector3.Distance(spawnPosition, otherEnemy.transform.position) < enemyRadius && otherEnemy.activeInHierarchy)
                    {
                        isSpawnAvailable = false;
                        break;
                    }
                }

                if (isSpawnAvailable == true)
                {
                    _enemyList[i].GetComponent<EnemyController>().Reactivate(spawnPosition);
                    Debug.Log("respawned");
                    break;
                }
            }
        }

        
    }
}
