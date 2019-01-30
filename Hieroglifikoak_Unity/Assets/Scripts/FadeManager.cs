using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour {

    public Image blackImage;
    public float fadeOutDenbora;
    public float fadeInDenbora;
    public float speed = .8f;

    int newScene;
    bool fadeToScene;

	// Use this for initialization
	void Start () {
        StartCoroutine(FadeIn());
	}

    public void FadeToScene(int scene)
    {
        newScene = scene;
        fadeToScene = true;
        StartCoroutine(FadeOut());
    }

    public void Argitu(float t = 0)
    {
        StartCoroutine(FadeIn(t));
    }

    public void Ilundu()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn(float denbora = 0)
    {
        yield return new WaitForSeconds(denbora);
        float t = 1f;
        blackImage.color = new Color(0f, 0f, 0f, t);
        yield return new WaitForSeconds(fadeInDenbora);
        while (t > 0f)
        {
            t -= Time.deltaTime * speed;
            blackImage.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }
    }

    public IEnumerator FadeOut()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            blackImage.color = new Color(0f, 0f, 0f, t);
            yield return 0;
        }
        yield return new WaitForSeconds(fadeOutDenbora);
        if (fadeToScene)
        {
            fadeToScene = false;
            SceneManager.LoadScene(newScene);
        }
    }
}
