using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KakalardoBola : MonoBehaviour {

    public GameObject bolaParticle;
    public Transform obstacleCheck;
    public LayerMask oztopoak;

    Rigidbody2D rb;

    public float hormaDistantzia;
    public float abiadura;
    bool eskumaBegira;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        eskumaBegira = transform.rotation.y == 0 ? false : true;
	}
	
	// Update is called once per frame
	void Update () {
        // mugitu -> jokalaria kolpatu -> min eman, destroy
        //        -> horma kolpatu -> buelta eman -->> bukaerara heltzen -> destroy
        Vector2 mugimendua = rb.velocity;
        mugimendua.x = abiadura * (eskumaBegira ? 1 : -1);
        rb.velocity = mugimendua;
        if (HormaBilatu())
        {
            BueltaEman();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            // min eman
            bool eskuma = transform.position.x > collision.transform.position.x ? true : false;
            collision.transform.GetComponent<Eraso>().KolpeaJaso(eskuma);
            // suntsitu
            BolaSuntsitu();
        }
        else if(collision.transform.tag == "kutxa")
        {
            BolaSuntsitu();
        }
    }

    public void BolaSuntsitu()
    {
        Destroy(gameObject);
        Instantiate(bolaParticle, transform.position, transform.rotation);
    }

    private bool HormaBilatu()
    {
        return Physics2D.Raycast(obstacleCheck.position, eskumaBegira ? Vector2.right : Vector2.left, hormaDistantzia, oztopoak); ;
    }

    private void BueltaEman()
    {
        eskumaBegira = !eskumaBegira;
        transform.eulerAngles = new Vector3(0, eskumaBegira ? -180 : 0, 0);
    }
}
