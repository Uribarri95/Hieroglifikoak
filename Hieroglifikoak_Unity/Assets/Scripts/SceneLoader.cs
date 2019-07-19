using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public void LoadScene(int i)
    {
        print("loading scene");
        SceneManager.LoadScene(i);
    }

    public int GetCurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

}