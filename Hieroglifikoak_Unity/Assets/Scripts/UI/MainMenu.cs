using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public FadeManager fadeManager;
    public static int sceneToLoad = 1; // load fitxategia zenbaki hau aldatu

    public void Play()
    {
        fadeManager.FadeToScene(sceneToLoad);
    }

    public void Quit()
    {
        Debug.Log("Irteten");
        Application.Quit();
    }

    void Save()
    {

    }

    void Load()
    {

    }
}
