using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Obsolete]

public class SpawnEnemies : MonoBehaviour
{
    //public int numberToSpawn;
    public UIManager UI;
    public GameObject quad;

    public GameObject[] _enemies;
    public List<GameObject> _enemyList;
    //public GameObject Enemy1;
    //public GameObject Enemy2;
    //public GameObject Enemy3;
    //public GameObject Enemy4;
    //public GameObject Enemy5;

    private int _waveCount = 0;
    private int _enemyCount = 4;
    public static int _enemyCountUpdate = 1;
    private float _waitTime = 0.2f;
    private bool _StopSpawning = false;
    int[] _enemySpawnProb = new int[5];

    public static float shotIntervalMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("PhaseManagement");
        //InvokeRepeating("Spawn", 0f, 3f);
    }

    IEnumerator PhaseManagement()
    {
        yield return new WaitForSeconds(2f);
        while (_StopSpawning == false)
        {
            yield return Spawncycle();
            yield return new WaitWhile(EnemyIsAlive);
            Debug.Log("Wave " + _waveCount + " cleared");
            yield return new WaitForSeconds(1.5f);
            UI.AddWave();
        }
    }

    IEnumerator Spawncycle()
    {
        _waveCount++;
        _enemyCount = 4 + (_waveCount - 1) * _enemyCountUpdate;
        AssignProbability();
        for (int i = 0; i < _enemyCount; i++)
        {
            Spawn();
            yield return new WaitForSeconds(_waitTime);
        }

        //for (float n = 0; ; n++)
        //{
        //    if (n < 15)
        //    {
        //        Spawn(Enemy1, 2);
        //        yield return new WaitForSeconds(2.5f);
        //    } 
        //    else if (n >= 15 && n < 30)
        //    {
        //        Spawn(Enemy1, 3);
        //        yield return new WaitForSeconds(2.25f);
        //    } 
        //    else
        //    {
        //        Spawn(Enemy1, 4);
        //        yield return new WaitForSeconds(2f);
        //    }
        //}
    }

    public void Spawn(/*GameObject toSpawn, int n*/)
    {
        MeshCollider c = quad.GetComponent<MeshCollider>();

        float x, y;
        Vector2 pos;

        int randomEnemy = Random.Range(1, 101);
        int low;
        int high = 0;

        for (int i = 0; i < _enemies.Length; i++)
        {
            low = high;
            high += _enemySpawnProb[i];

            if (randomEnemy >= low && randomEnemy < high)
            {
                GameObject toSpawn = _enemies[i];
                float width = toSpawn.GetComponent<SpriteRenderer>().bounds.size.x;
                float height = toSpawn.GetComponent<SpriteRenderer>().bounds.size.y;

                x = Random.Range(c.bounds.min.x + width, c.bounds.max.x - width);
                y = Random.Range(c.bounds.min.y + height, c.bounds.max.y - height);
                pos = new Vector2(x, y);

                GameObject enemy = Instantiate(toSpawn, pos, toSpawn.transform.rotation);
                settingsEnemy(enemy, i);
                _enemyList.Add(enemy);
            }
        }
    }

    private void settingsEnemy(GameObject enemy, int enemyType)
    {
        float setShotInterval = 0;
        float setBulletForce = 0;
        switch (enemyType)
        {
            case 0:
                setShotInterval = 2f;
                setBulletForce = 10f;
                break;
            case 1:
                setShotInterval = 4f;
                setBulletForce = 20f;
                break;
            case 2:
                setShotInterval = 2f;
                setBulletForce = 10f;
                enemy.GetComponent<EnemyMovement>().moveSpeed = 3f;
                break;
            case 3:
                setShotInterval = 1.7f;
                setBulletForce = 10f;
                enemy.GetComponent<EnemyMovement>().moveSpeed = 2f;
                break;
            case 4:
                break;
        }

        enemy.GetComponent<EnemyShooting>().shotInterval = setShotInterval / shotIntervalMultiplier;
        enemy.GetComponent<EnemyShooting>().bulletForce = setBulletForce;
    }
    private bool EnemyIsAlive()
    {
        _enemyList = _enemyList.Where(e => e != null).ToList();
        UI.updateWave(_enemyList.Count, _enemyCount);
        return _enemyList.Count > 0;
    }

    private void AssignProbability()
    {
        switch (_waveCount)
        {
            case 1:
                _enemySpawnProb[0] = 100;
                _enemySpawnProb[1] = 0;
                _enemySpawnProb[2] = 0;
                _enemySpawnProb[3] = 0;
                _enemySpawnProb[4] = 0;
                //_enemyCount = 4;
                break;
            case 3:
                _enemySpawnProb[0] = 75;
                _enemySpawnProb[1] = 25;
                _enemySpawnProb[2] = 0;
                _enemySpawnProb[3] = 0;
                _enemySpawnProb[4] = 0;
                break;
            case 5:
                _enemySpawnProb[0] = 50;
                _enemySpawnProb[1] = 30;
                _enemySpawnProb[2] = 20;
                _enemySpawnProb[3] = 0;
                _enemySpawnProb[4] = 0;
                break;
            case 7:
                _enemySpawnProb[0] = 10;
                _enemySpawnProb[1] = 20;
                _enemySpawnProb[2] = 40;
                _enemySpawnProb[3] = 30;
                _enemySpawnProb[4] = 0;
                break;
        }
    }
}
