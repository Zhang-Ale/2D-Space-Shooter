using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnManager : NetworkBehaviour
{
    public bool isThisSingle; 
    public GameObject _leftEnemyPrefab;
    public GameObject _rightEnemyPrefab;
    public GameObject _leftEnemyContainer;
    public GameObject _rightEnemyContainer;

    public GameObject[] singlePowerups; 
    public GameObject[] powerups;
    private bool _stopSpawning = false; 
    private bool _stopLeftSpawning = false;
    private bool _stopRightSpawning = false;
    GameTime gametime;

    public float spawnTime; 
    void Start()
    {
        gametime = GameObject.Find("TimeManager").GetComponent<GameTime>();
        if (isThisSingle)
        {
            StartCoroutine(SpawnRoutine());

            StartCoroutine(SpawnPowerUpRoutine());
        }
        else
        {
            StartCoroutine(LeftSpawnRoutine());
            StartCoroutine(RightSpawnRoutine());

            StartCoroutine(SpawnLeftPowerUpRoutine());
            StartCoroutine(SpawnRightPowerUpRoutine());
        }  
    }

    public override void OnStartServer()
    {
        if (isThisSingle)
        {
            if (_stopSpawning && gametime.gameOver)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (_stopLeftSpawning && _stopRightSpawning && gametime.gameOver)
            {
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (_stopSpawning == false && gametime.gameOver == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(2.5f, 20f), 5.6f, 0);
            GameObject rightEnemy = Instantiate(_rightEnemyPrefab, postToSpawn, Quaternion.identity);
            rightEnemy.transform.parent = _rightEnemyContainer.transform;
            NetworkServer.Spawn(rightEnemy);
            yield return new WaitForSeconds(spawnTime);
            //yield return null; //means: wait 1 frame
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (_stopSpawning == false && gametime.gameOver == false)
        {
            yield return new WaitForSeconds(1);
            Vector3 postToSpawn = new Vector3(Random.Range(1f, 22f), 5.6f, 0);
            int randomPwrUp = Random.Range(0, 2);
            GameObject rightPwrUp = Instantiate(singlePowerups[randomPwrUp], postToSpawn, Quaternion.identity);
            NetworkServer.Spawn(rightPwrUp);
            yield return new WaitForSeconds(Random.Range(4, 7));
        }
    }

    IEnumerator LeftSpawnRoutine()
    {
        while (_stopLeftSpawning == false && gametime.gameOver == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(1f, 7.7f), 5.6f, 0);
            GameObject leftEnemy = Instantiate(_leftEnemyPrefab, postToSpawn, Quaternion.identity);
            leftEnemy.transform.parent = _leftEnemyContainer.transform;
            NetworkServer.Spawn(leftEnemy);
            yield return new WaitForSeconds(spawnTime);
            //yield return null; //means: wait 1 frame
        }
    }

    IEnumerator RightSpawnRoutine()
    {
        while (_stopRightSpawning == false && gametime.gameOver == false)
        {
            Vector3 postToSpawn = new Vector3(Random.Range(11.5f, 22f), 5.6f, 0);
            GameObject rightEnemy = Instantiate(_rightEnemyPrefab, postToSpawn, Quaternion.identity);
            rightEnemy.transform.parent = _rightEnemyContainer.transform;
            NetworkServer.Spawn(rightEnemy);
            yield return new WaitForSeconds(spawnTime);
        }
    }

    IEnumerator SpawnLeftPowerUpRoutine()
    {
        while(_stopLeftSpawning == false && gametime.gameOver == false)
        {
            yield return new WaitForSeconds(1);
            Vector3 postToSpawn = new Vector3(Random.Range(1f, 7.7f), 5.6f, 0);
            int randomPwrUp = Random.Range(0, 2);
            GameObject leftPwrUp = Instantiate(powerups[randomPwrUp], postToSpawn, Quaternion.identity);
            NetworkServer.Spawn(leftPwrUp);
            yield return new WaitForSeconds(Random.Range(4, 7));
        }
    }

    IEnumerator SpawnRightPowerUpRoutine()
    {
        while (_stopRightSpawning == false && gametime.gameOver == false)
        {
            yield return new WaitForSeconds(1);
            Vector3 postToSpawn = new Vector3(Random.Range(11.5f, 22f), 5.6f, 0);
            int randomPwrUp = Random.Range(3, 5);
            GameObject rightPwrUp = Instantiate(powerups[randomPwrUp], postToSpawn, Quaternion.identity);
            NetworkServer.Spawn(rightPwrUp);
            yield return new WaitForSeconds(Random.Range(4, 7));
        }
    }

    public void OnLeftPlayerDeath()
    {
        _stopLeftSpawning = true;
    }

    public void OnRightPlayerDeath()
    {
        _stopRightSpawning = true;
    }
}
