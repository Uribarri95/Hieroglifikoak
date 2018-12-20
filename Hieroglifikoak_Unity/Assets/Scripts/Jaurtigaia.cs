using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jaurtigaia : MonoBehaviour {

    public GameObject destroyParticle;      // objektua suntsitzean agertarazten den partikula
    public float desagertuDenbora;          // objektua ezer ez badu kolpatzen, denbora pasatu ostean suntsitzen da
    public float abiadura;                  // objektuaren abiadura

    Rigidbody2D rb;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * -1 * abiadura;
        Destroy(gameObject, desagertuDenbora);
	}

    // jokalaria edo pareta kolpatzean objektua suntsitu
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            bool eskuma = transform.position.x > collision.transform.position.x ? true : false;
            collision.transform.GetComponent<Eraso>().KolpeaJaso(eskuma);
            Destroy(gameObject);
            Instantiate(destroyParticle, transform.position, transform.rotation);
        }
        else if (collision.tag == "Horma")
        {
            Destroy(gameObject);
            Instantiate(destroyParticle, transform.position, transform.rotation);
        }
    }
}
