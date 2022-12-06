using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnSystem : MonoBehaviour
{
	public float timeBetweenWaves = 5f;
	private float countdown = 2f;

    public WaveSpawner[] spawners;
    private int spawnIndex = 0;

	public Text waveCountdownText;
	public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform spawn in transform)
        {
            Debug.Log(spawn.GetComponent<WaveSpawner>().finished);
            if (!spawn.GetComponent<WaveSpawner>().finished)
                return;
        }

        

		if (countdown <= 0f)
		{
            foreach(Transform spawn in transform)
            {
			    StartCoroutine(spawn.GetComponent<WaveSpawner>().SpawnWave());
            }
			countdown = timeBetweenWaves;
			return;
		}

		countdown -= Time.deltaTime;
		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        
		//waveCountdownText.text = string.Format("{0:00.00}", countdown);
    }
}
