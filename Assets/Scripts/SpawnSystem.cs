using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnSystem : MonoBehaviour
{
	public float timeBetweenWaves = 5f;
	private float countdown = 2f;

	public TMP_Text waveCountdownText;
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
            if (!spawn.GetComponent<WaveSpawner>().finished)
                return;
        }

		if (countdown <= 0f)
		{
            waveCountdownText.enabled = false;
            foreach(Transform spawn in transform)
            {
			    StartCoroutine(spawn.GetComponent<WaveSpawner>().SpawnWave());
            }
			countdown = timeBetweenWaves;
			return;
		}

		countdown -= Time.deltaTime;
		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        
        waveCountdownText.enabled = true;
        waveCountdownText.text = "NEXT WAVE: " + string.Format("{0:00.00}", countdown);
    }
}
