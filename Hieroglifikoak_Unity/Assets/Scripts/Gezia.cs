using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gezia : MonoBehaviour {

    public GameObject geziPartikula;
    Rigidbody2D rb;

    float desagertuDenbora = 5f;
    float abiadura = 30f;
    float arrowDamage;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * abiadura;
        Destroy(gameObject, desagertuDenbora);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Etsaia")
        {
            collision.GetComponent<Etsaia>().KolpeaJaso(arrowDamage);
            collision.GetComponent<Etsaia>().SetKolpea(false);
            Debug.Log(collision.name);
            Destroy(gameObject);
            Instantiate(geziPartikula, transform.position, transform.rotation);
        }
        else if (collision.tag == "Horma")
        {
            Debug.Log(collision.name);
            Destroy(gameObject);
            Instantiate(geziPartikula, transform.position, transform.rotation);
        }
    }

    public void SetArrowDamage(float damage)
    {
        arrowDamage = damage;
    }
}
