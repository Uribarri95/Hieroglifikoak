using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eskilerak : MonoBehaviour {

    JokalariMug jokalaria;
    public float yAbiadura = 3;
    public float xAbiadura = 3;

    void Start () {
        jokalaria = FindObjectOfType<JokalariMug>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            jokalaria.eskileran = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!Input.GetButton("Jump") && jokalaria.eskileraIgotzen)
                jokalaria.SetAbiadura(new Vector2(0, -.4f));
            jokalaria.eskileran = false;
            jokalaria.eskileraIgotzen = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && jokalaria.eskileran)
        {
            // eskileratik salto egin
            float aginduHorizontala = Input.GetAxisRaw("Horizontal");
            if (aginduHorizontala != 0 && jokalaria.eskileraIgotzen)
            {
                jokalaria.SetAbiadura(new Vector2(aginduHorizontala * xAbiadura, 0));
                jokalaria.eskileraIgotzen = false;
            }

            // eskilera igo/jeitsi
            float aginduBertikala = Input.GetAxisRaw("Vertical");
            if(aginduBertikala != 0)
            {
                // beheko topea
                if (aginduBertikala < 0 && jokalaria.GetLurrean())
                    jokalaria.eskileraIgotzen = false;
                // goiko topea
                else if (aginduBertikala > 0 && jokalaria.transform.position.y > gameObject.transform.position.y && jokalaria.GetLurrean())
                    jokalaria.eskileraIgotzen = false;
                else
                {
                    jokalaria.eskileraIgotzen = true;
                    float posX = gameObject.transform.position.x;
                    jokalaria.transform.position = new Vector2(posX, jokalaria.transform.position.y);
                    jokalaria.SetAbiadura(new Vector2(0, aginduBertikala * yAbiadura));
                }
            }
            else
            {
                if (jokalaria.GetLurrean())
                    jokalaria.eskileraIgotzen = false;
                // eskileran geldi
                else if (jokalaria.eskileraIgotzen)
                    jokalaria.SetAbiadura(new Vector2(0, 0));
            }
        }
    }
}
