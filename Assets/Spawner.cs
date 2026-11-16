using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform basicEnemyPrefab;
    public Transform spawnPoint;
    public float timeBetween = 5f;
    private float countdown = 4.5f;

    private int waveNumber = 1;

    void Update()
    {
        if(countdown <= 0f)
        {
            NewWave();
            countdown = timeBetween;
        }

        countdown -= Time.deltaTime;
    }

    void SpawnBasicEnemy()
    {
        Instantiate(basicEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    void NewWave()
    {
        for (int i=0;i<waveNumber;i++)
        {
            SpawnBasicEnemy();
        }

        waveNumber++;
    }
}
