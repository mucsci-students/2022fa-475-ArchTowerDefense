using UnityEngine;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour {

	public static bool GameIsOver;

	public GameObject arch;
	public GameObject gameOverUI;
	public GameObject completeLevelUI;
	public int totalTurrets = 15;
	public int turretCount = 0;
	public TextMeshProUGUI turretText;

	void Start ()
	{
		GameIsOver = false;
	}

	// Update is called once per frame
	void Update () {
		turretText.SetText(turretCount + " / " + totalTurrets);

		if (GameIsOver)
		{
			Time.timeScale = 0;
			return;
		}

		if (arch == null)
		{
			EndGame();
		}
	}

	void EndGame ()
	{
		GameIsOver = true;
		gameOverUI.SetActive(true);
	}

	public void WinLevel ()
	{
		GameIsOver = true;
		completeLevelUI.SetActive(true);
	}
}
