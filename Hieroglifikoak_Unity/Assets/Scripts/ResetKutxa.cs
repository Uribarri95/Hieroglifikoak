using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetKutxa : MonoBehaviour {

    int kutxaKop;
    Vector2[] kutxaPos;

	// Use this for initialization
	void Start () {
        kutxaKop = transform.childCount;
        if(kutxaKop != 0)
        {
            kutxaPos = new Vector2[kutxaKop];
            for (int i = 0; i < kutxaKop; i++)
            {
                kutxaPos[i] = transform.GetChild(i).position;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GetComponent<Animator>().SetBool("gainean", true);
            for (int i = 0; i < kutxaKop; i++)
            {
                transform.GetChild(i).position = kutxaPos[i];
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GetComponent<Animator>().SetBool("gainean", false);
        }
    }
}
