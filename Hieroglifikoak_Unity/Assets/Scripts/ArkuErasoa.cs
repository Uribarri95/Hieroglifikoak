using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkuErasoa : MonoBehaviour {

    private float coolDown;
    public float denboraTartea;

    public Transform erasoPuntua;
    public GameObject gezia;

    JokalariMug jokalaria;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        jokalaria = FindObjectOfType<JokalariMug>();
    }

    // Update is called once per frame
    void Update () {
        if (coolDown <= 0)
        {
            if (Input.GetKeyDown(KeyCode.RightControl))
            {
                if (!jokalaria.GetMakurtu())
                {
                    anim.SetBool("eraso", true);
                    coolDown = denboraTartea;
                }
            }
        }
        else
        {
            coolDown -= Time.deltaTime;
        }
	}

    //animazioko event jaurtitzen du metodoa
    public void GeziaJaurti()
    {
        Instantiate(gezia, erasoPuntua.position, erasoPuntua.rotation);
    }
}
