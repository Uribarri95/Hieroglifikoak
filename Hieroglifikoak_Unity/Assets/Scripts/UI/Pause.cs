using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public GameObject pauseUI;
    public FadeManager fademanager;
    public GameObject pausePanel;
    public GameObject hiztegiPanel;
    public GameObject menuPanel;

    public Sprite soinua;
    public Sprite mute;
    public Image botoia;

    public static bool jokuaGeldituta = false;

    bool irten = false;
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!irten)
            {
                TogglePause();
            }
        }
	}

    public void TogglePause()
    {
        AudioManager.instantzia.Play("Menua");
        
        pauseUI.SetActive(!pauseUI.activeSelf);
        if (!pauseUI.activeSelf)
        {
            //AudioListener.pause = false;
            //Mute();

            pausePanel.SetActive(true);
            hiztegiPanel.SetActive(false);
            menuPanel.SetActive(false);
        }
        /*else
        {
            //AudioListener.pause = true;
            Mute();
        }*/
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;
        jokuaGeldituta = !jokuaGeldituta;
    }

    public void Mute()
    {
        AudioListener.pause = !AudioListener.pause;
        if (AudioListener.pause)
        {
            botoia.sprite = mute;
        }
        else
        {
            botoia.sprite = soinua;
        }
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
        menuPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void Irten()
    {
        print("irten");
        TogglePause();
        irten = true;
        fademanager.FadeToScene(0);
    }
}
