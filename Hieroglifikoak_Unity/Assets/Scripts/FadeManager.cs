using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour {

    public SceneLoader sceneLoader;
    public Image blackImage;
    public float fadeOutDenbora;
    public float fadeInDenbora;
    public float speed = .6f;

    int newScene;
    bool fadeToScene;

	// Use this for initialization
	void Start () {
        
	}

    public void FadeToScene(int scene)
    {
        newScene = scene;
        fadeToScene = true;
        StartCoroutine(FadeOut(.8f));
    }

    // Desaktibatutako objektuak ezin dute coroutine hasi
    public void Argitu(float t = 0) // ilundu argitu kendu? !!! erabiltzen duen bakarra gelaaldatu da eta fadein deitu dezake zuzenean
    {
        StartCoroutine(FadeIn(t));
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

    public void Ilundu()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut(float denbora = 0)
    {
        yield return new WaitForSeconds(denbora);
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
            sceneLoader.LoadScene(newScene);
        }
    }

    public void BukaeraEszenatokia()
    {
        fadeToScene = true;
        StartCoroutine(Bukaera(2f));
    }

    IEnumerator Bukaera(float denbora = 0)
    {
        yield return new WaitForSeconds(denbora);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            blackImage.color = new Color(1f, 1f, 1f, t);
            yield return 0;
        }
        yield return new WaitForSeconds(fadeOutDenbora);
        if (fadeToScene)
        {
            fadeToScene = false;
            sceneLoader.LoadScene(2);
        }
    }

    public IEnumerator BukaeraFadeIn(float denbora = 0)
    {
        yield return new WaitForSeconds(denbora);
        float t = 1f;
        blackImage.color = new Color(1f, 1f, 1f, t);
        yield return new WaitForSeconds(fadeInDenbora);
        while (t > 0f)
        {
            t -= Time.deltaTime * speed;
            blackImage.color = new Color(1f, 1f, 1f, t);
            yield return 0;
        }
    }
}
