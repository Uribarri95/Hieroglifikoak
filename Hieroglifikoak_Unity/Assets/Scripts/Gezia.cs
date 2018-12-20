using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gezia : MonoBehaviour {

    public GameObject geziPartikula;        // apurtutako gezi irudia
    Rigidbody2D rb;

    public float desagertuDenbora = 5f;     // gezia ezer kolpatzen ez badu desagertzeko denbora
    public float abiadura = 30f;            // geziaren abiadura
    float arrowDamage;                      // geziaren min puntuak

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * abiadura;
        Destroy(gameObject, desagertuDenbora);
    }

    // etsaia edo pareta bat kolpatzean suntsitu
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Etsaia")
        {
            collision.GetComponent<Etsaia>().KolpeaJaso(arrowDamage);
            collision.GetComponent<Etsaia>().SetKolpea(false);
            Destroy(gameObject);
            Instantiate(geziPartikula, transform.position, transform.rotation);
        }
        else if (collision.tag == "Horma")
        {
            Destroy(gameObject);
            Instantiate(geziPartikula, transform.position, transform.rotation);
        }
    }

    // geziaren min puntuak Eraso script-an aldatzeko
    public void SetArrowDamage(float damage)
    {
        arrowDamage = damage;
    }
}
