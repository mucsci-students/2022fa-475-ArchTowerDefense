using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnSystem : MonoBehaviour
{
	public float timeBetweenWaves = 60f;
	private float countdown = 2f;

	public TMP_Text waveCountdownText;
    public TMP_Text skipBuildText;
    public TMP_Text gunPrompt;
	public GameManager gameManager;

    public bool inBuildPeriod = false;

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
            inBuildPeriod = false;
            waveCountdownText.enabled = false;
            skipBuildText.enabled = false;
            gunPrompt.enabled = false;
            foreach(Transform spawn in transform)
            {
			    StartCoroutine(spawn.GetComponent<WaveSpawner>().SpawnWave());
            }
			countdown = timeBetweenWaves;
			return;
		}

        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Skip Build Period");
            countdown = 0;
            return;
        }

		countdown -= Time.deltaTime;
		countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);
        
        inBuildPeriod = true;
        waveCountdownText.enabled = true;
        skipBuildText.enabled = true;
        gunPrompt.enabled = true;
        waveCountdownText.text = "NEXT WAVE: " + string.Format("{0:0}", countdown);

    }
}
