using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jaurtigaia : MonoBehaviour {

    public GameObject destroyParticle;

    Rigidbody2D rb;

    public float desagertuDenbora;
    public float abiadura;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * -1 * abiadura;
        Destroy(gameObject, desagertuDenbora);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Jokalaria kolpatuta, mina eman");
            bool eskuma = transform.position.x > collision.transform.position.x ? true : false;
            collision.transform.GetComponent<Eraso>().KolpeaJaso(eskuma);
            Destroy(gameObject);
            Instantiate(destroyParticle, transform.position, transform.rotation);
        }
    }
}
