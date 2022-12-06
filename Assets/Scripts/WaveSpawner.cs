using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour {

	public bool finished = true;

	public Wave[] waves;
	public GameObject waypoints;

	public Transform spawnPoint;

	public GameManager gameManager;

	private int waveIndex = 0;

	void Update ()
	{
		if (transform.childCount != 0)
		{
			return;
		}

		finished = true;

		if (waveIndex == waves.Length)
		{
			gameManager.WinLevel();
			this.enabled = false;
		}
	}

	public IEnumerator SpawnWave ()
	{
		finished = false;
		PlayerStats.Rounds++;

		Wave wave = waves[waveIndex];

		for (int i = 0; i < wave.count; i++)
		{
			SpawnEnemy(wave.enemy);
			yield return new WaitForSeconds(1f / wave.rate);
		}

		waveIndex++;
	}

	void SpawnEnemy (GameObject enemy)
	{
		var enemyInst = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
		enemyInst.GetComponent<EnemyMovement>().waypoints = waypoints;
		enemyInst.transform.SetParent(transform);
	}

}
