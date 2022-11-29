using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject infoScreen;
    public GameObject settingsScreen;
    private bool lowerInfo = false;
    private bool lowerSettings = false;
    private bool raiseInfo = false;
    private bool raiseSettings = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lowerInfo || lowerSettings)
        {
            RectTransform screenRect = null;
            if (lowerInfo)
                screenRect = infoScreen.GetComponent<RectTransform>();
            else
                screenRect = settingsScreen.GetComponent<RectTransform>();

            screenRect.localPosition += Vector3.down * 25f;

            if (screenRect.localPosition.y <= 0)
            {
                lowerInfo = false;
                lowerSettings = false;
            }
        }
        else if (raiseInfo || raiseSettings)
        {
            RectTransform screenRect = null;
            if (raiseInfo)
                screenRect = infoScreen.GetComponent<RectTransform>();
            else
                screenRect = settingsScreen.GetComponent<RectTransform>();

            screenRect.localPosition += Vector3.up * 25f;

            if (screenRect.localPosition.y >= 1200)
            {
                raiseInfo = false;
                raiseSettings = false;
            }
        }
    }

    public void TransitionToGame()
    {
        SceneManager.LoadScene("ArchTowerDefense");
    }

    public void InfoScreenTrans(int val)
    {
        if (val == 0)
        {
            raiseInfo = false;
            lowerInfo = true;
        }
        else
        {
            lowerInfo = false;
            raiseInfo = true;
        }
    }

    public void SettingsScreenTrans(int val)
    {
        if (val == 0)
        {
            raiseSettings = false;
            lowerSettings = true;
        }
        else
        {
            lowerSettings = false;
            raiseSettings = true;
        }
    }
}
