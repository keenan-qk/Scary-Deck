using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] spawnableEnemies;
    
    private bool enemyStatus;
    private GameObject enemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy != null)
        {
            if (enemy.activeSelf == false)
            {
                enemyStatus = false;
            }
        }
    }

    public bool GetStatus()
    {
        return enemyStatus;
    }

    public void SpawnEnemy()
    {
        int randNum = Random.Range(0, spawnableEnemies.Length);
        enemy = Instantiate(spawnableEnemies[randNum], transform.position, transform.rotation);
    }
}
