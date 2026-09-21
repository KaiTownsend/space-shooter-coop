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
    
    private void Awake()
    {
        _enemyList = new GameObject[80];
    }

    private void Start()
    {
        CreateEnemies();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _enemyList[_enemyList.Length/2].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            EnableEnemies();
        }
    }

    private void CreateEnemies()
    {
        Vector3 spawnPosition = _enemySpawnTransform.position;
        for (int i = 0; i < _enemyList.Length/10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject enemyPrefab = Instantiate(_enemyPrefab, _enemyContainer);
                enemyPrefab.transform.position = spawnPosition;
                spawnPosition += new Vector3(0.25f, 0, 0);

                _enemyList[j + i*10] = enemyPrefab;
            }

            spawnPosition = new Vector3(_enemySpawnTransform.position.x, spawnPosition.y + 0.25f, _enemySpawnTransform.position.z);
        }
    }

    private void EnableEnemies()
    {
        foreach(GameObject enemyPrefab in _enemyList)
        {
            if (enemyPrefab.activeInHierarchy == false)
            {
                enemyPrefab.SetActive(true);
            }
        }
    }
}
