using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMastermindSingleton : MonoBehaviour
{
    private static EnemyMastermindSingleton _instance;
    public static EnemyMastermindSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = new GameObject();
                _instance = obj.AddComponent<EnemyMastermindSingleton>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private float _spawnEnemyDelay = 10f;
    private float _spawnEnemyDelayTimer;

    [SerializeField]
    private int _enemySpawnCountMin = 1;
    [SerializeField]
    private int _enemySpawnCountMax = 2;

    [SerializeField]
    private GameObject _eliteEnemyPrefab;
    [SerializeField]
    private float _eliteSpawnChance = 10;

    [SerializeField]
    private float _spawnArea = 20;

    [SerializeField]
    private float _spawnCountMultiplerRatePerSecond = 0.01f;
    private float _spawnCountMultipler = 1;

    [SerializeField]
    private int _enemyAttackLoad = 3;
    private float _currentEnemyAttackWeight;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Update()
    {
        _spawnEnemyDelayTimer += Time.deltaTime;
        _spawnCountMultipler += Time.deltaTime * _spawnCountMultiplerRatePerSecond;
        if (_spawnEnemyDelayTimer >= _spawnEnemyDelay)
        {
            var spawnCount = Random.Range(_enemySpawnCountMin, _enemySpawnCountMax) * _spawnCountMultipler;
            for (int i = 0; i < spawnCount; i++)
            {
                var enemyToSpawn = _enemyPrefab;
                if (Random.Range(0f, 100f) <= _eliteSpawnChance) 
                {
                    enemyToSpawn = _eliteEnemyPrefab;
                }
                var randomVector = Random.insideUnitCircle;
                var spawnPosition = new Vector3(randomVector.x, 5, randomVector.y) * _spawnArea;
                Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
            }
            _spawnEnemyDelayTimer = 0;
        }
    }

    //This is a simple attack weight & load system, this will dissallow too much enemy to attack at the same time
    //Load is the max amount of weight we can allow, if we have 3 load, and enemy attacks with 1 weight, that means 3 enemy can attack at the same time
    //If there is a heavy enemy with 2 weight, that means only 1 heavy & 1 light enemy can attack
    public bool RequestToAttack(int weight)
    {
        var isAllowed = _currentEnemyAttackWeight + weight <= _enemyAttackLoad;
        if (isAllowed)
        {
            _currentEnemyAttackWeight += weight;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FinishAttack(int weight)
    {

        _currentEnemyAttackWeight -= weight;
    }

}
