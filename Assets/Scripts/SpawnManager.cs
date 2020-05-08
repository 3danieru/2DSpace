using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{ public GameObject[] powerUpPrefabs;
    public GameObject enemyContainer;
    public GameObject enemyPrefab;
    private Vector3 spawnPosition;
    [SerializeField]
    private float secondsBetweenEnemies = 5f;
    [SerializeField]
    private float secondsBetweenPowerUps = 5f;
    GameObject newPowerUp;

    private bool stopSpawning;
    //spawn game objects every 5 seconds

    private void Start()
    {
        // As it is an infinite loop, calling it on start keeps it going
        
    }

    public void StartSpawning()
    {
            stopSpawning = false;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerUpRoutine());

    }

    IEnumerator SpawnEnemyRoutine()
    {
        // while loop
        yield return new WaitForSeconds(secondsBetweenEnemies);
        while (stopSpawning == false)
        {
            spawnPosition = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(secondsBetweenEnemies);
        }  
    }
    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(secondsBetweenPowerUps);
        while (stopSpawning == false)
        {
            secondsBetweenPowerUps = Random.Range(3, 8);
            spawnPosition = new Vector3(Random.Range(-8f, 8f), 7, 0);

            float percentaje = Random.Range(0,10f);
            
            if(percentaje <= 3) //Ammo (30% chance)
            {
               newPowerUp = Instantiate(powerUpPrefabs[0], spawnPosition, Quaternion.identity);
            }
            else if (percentaje > 3 && percentaje <= 9f) //Any powerup but ammo and MEGA(60% chance)
            {
                int prefabID = Random.Range(1, powerUpPrefabs.Length -1 );
                newPowerUp = Instantiate(powerUpPrefabs[prefabID], spawnPosition, Quaternion.identity);
            }
            else if (percentaje > 9) //MEGA POWERUP (10% chance)
            {  
                newPowerUp = Instantiate(powerUpPrefabs[5], spawnPosition, Quaternion.identity);
            }          
            yield return new WaitForSeconds(secondsBetweenPowerUps);
        }
    }

    public void OnPlayerDeath()

    {
        stopSpawning = true;
    }
}
