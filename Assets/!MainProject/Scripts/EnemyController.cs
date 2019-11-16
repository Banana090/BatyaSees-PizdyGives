using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    private Stack<Enemy> enemies;

    private void Start()
    {
        enemies = new Stack<Enemy>();
        StartCoroutine(SpawnEnemies(3));
    }

    private IEnumerator SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            enemies.Push(Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity).GetComponent<Enemy>());
            yield return new WaitForSeconds(Random.Range(0.2f, 1.1f));
        }

        StartCoroutine(StopAttack());
    }

    private IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(6f);

        int iterations = enemies.Count;

        for (int i = 0; i < iterations; i++)
            enemies.Pop().StopWorkingGoAway();
    }
}
