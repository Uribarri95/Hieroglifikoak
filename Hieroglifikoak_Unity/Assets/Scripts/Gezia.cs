using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gezia : MonoBehaviour {

    public GameObject geziPartikula;
    public Rigidbody2D rb;
    float desagertuDenbora = 3f;
    public float abiadura = 30f;
    public int damage;

    // Use this for initialization
    void Start () {
        rb.velocity = transform.right * abiadura;
        Destroy(gameObject, desagertuDenbora);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Etsaia")
        {
            collision.GetComponent<Etsaia>().KolpeaJaso(damage);
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
}
