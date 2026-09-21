using System;
using System.Linq;
using Unity.Collections.Tests.CoreCLR.TestJobs;
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
    [SerializeField] private Transform _enemyTransform;
    private GameObject[] enemyList;
    
    private void Awake()
    {
        enemyList = new GameObject[50];
    }

    private void Start()
    {
        CreateEnemies();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            enemyList[enemyList.Length/2].SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            EnableEnemies();
        }
    }

    private void CreateEnemies()
    {
        Vector3 spawnPosition = _enemyTransform.position;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject enemyPrefab = Instantiate(_enemyPrefab, _enemyContainer);
                enemyPrefab.transform.position = spawnPosition;
                spawnPosition += new Vector3(0.25f, 0, 0);

                enemyList[j + i*10] = enemyPrefab;
            }

            spawnPosition = new Vector3(_enemyTransform.position.x, spawnPosition.y + 0.25f, _enemyTransform.position.z);
        }
    }

    private void EnableEnemies()
    {
        foreach(GameObject enemyPrefab in enemyList)
        {
            if (enemyPrefab.activeInHierarchy == false)
            {
                enemyPrefab.SetActive(true);
            }
        }
    }
}
