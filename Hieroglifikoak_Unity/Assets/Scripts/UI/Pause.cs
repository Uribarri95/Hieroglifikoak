using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    public GameObject pauseUI;
    public FadeManager fademanager;
    public GameObject pausePanel;
    public GameObject hiztegiPanel;

    public static bool jokuaGeldituta = false;
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
	}

    public void TogglePause()
    {
        pauseUI.SetActive(!pauseUI.activeSelf);
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        jokuaGeldituta = !jokuaGeldituta;
    }

    public void Jarraitu()
    {
        TogglePause();
    }

    public void Hiztegia()
    {
        hiztegiPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void Menu()
    {
        TogglePause();
        fademanager.FadeToScene(0);
    }
}
