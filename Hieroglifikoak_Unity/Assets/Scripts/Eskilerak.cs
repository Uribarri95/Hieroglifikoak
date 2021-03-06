﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eskilerak : MonoBehaviour {

    JokalariMug jokalaria;
    public float yAbiadura = 3;
    public float xAbiadura = 3;
    bool eskileran = false;

    void Start () {
        jokalaria = FindObjectOfType<JokalariMug>();
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!Input.GetButton("Jump") && eskileran)
            {
                jokalaria.SetAbiadura(new Vector2(0, -.4f));
            }
            eskileran = false;
            jokalaria.SetEskaileran(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Ekintzak.instantzia.GetEskilerakIgo())
        {
            if (collision.tag == "Player")
            {
                // eskileratik salto egin
                float aginduHorizontala = Input.GetAxisRaw("Horizontal");
                if (aginduHorizontala != 0 && eskileran)
                {
                    jokalaria.SetAbiadura(new Vector2(aginduHorizontala * xAbiadura, 0));
                    eskileran = false;
                    jokalaria.SetEskaileran(false);
                }

                // eskilera igo/jeitsi
                float aginduBertikala = Input.GetAxisRaw("Vertical");
                if (aginduBertikala != 0)
                {
                    // beheko topea
                    if (aginduBertikala < 0 && jokalaria.GetLurrean())
                    {
                        eskileran = false;
                        jokalaria.SetEskaileran(false);
                    }
                    // goiko topea
                    else if (aginduBertikala > 0 && jokalaria.transform.position.y > gameObject.transform.position.y && jokalaria.GetLurrean())
                    {
                        eskileran = false;
                        jokalaria.SetEskaileran(false);
                    }
                    // eskileratik mugitu
                    else
                    {
                        if (!eskileran)
                        {
                            eskileran = true;
                            float posX = gameObject.transform.position.x;
                            jokalaria.transform.position = new Vector2(posX, jokalaria.transform.position.y);
                        }
                        jokalaria.SetEskaileran(true);
                        jokalaria.SetAbiadura(new Vector2(0, aginduBertikala * yAbiadura));
                    }
                }
                else
                {
                    if (jokalaria.GetLurrean())
                    {
                        eskileran = false;
                        jokalaria.SetEskaileran(false);
                    }
                    // eskileran geldi
                    else if (eskileran)
                    {
                        jokalaria.SetAbiadura(new Vector2(0, 0));
                    }
                }
            }
        }
    }
}
