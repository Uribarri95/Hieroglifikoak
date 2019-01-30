using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour {

    int level = 1;
    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        FadeIn();
	}

    public void FadeOutMenu(int sceneZenbakia) // parametro bezala scene, bestela deia egiten duen script-a aldatzea
    {
        level = sceneZenbakia;
        anim.SetTrigger("fadeOutMenu");
    }

    public void FadeOutBukatuta()
    {
        SceneManager.LoadScene(level);
    }

    public void FadeOut()
    {
        anim.SetTrigger("fadeOut");
    }

    public void FadeIn()
    {
        anim.SetTrigger("fadeIn");
    }
}
