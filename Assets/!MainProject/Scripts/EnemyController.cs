using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private Stack<Enemy> enemies;
    private PlayerMovement playerMovement;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        enemies = new Stack<Enemy>();
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public IEnumerator SpawnEnemies(int count, float time)
    {
        playerMovement.GoSleep();
        yield return new WaitForSeconds(3.5f);
        float currentTime = Time.time;
        for (int i = 0; i < count; i++)
        {
            enemies.Push(Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, transform).GetComponent<Enemy>());
            yield return new WaitForSeconds(Random.Range(0.1f, 0.75f));
        }

        yield return new WaitForSeconds(time - (Time.time - currentTime));

        int iterations = enemies.Count;

        for (int i = 0; i < iterations; i++)
            enemies.Pop().StopWorkingGoAway();

        GameManager.StartGameTimer();
    }
}
