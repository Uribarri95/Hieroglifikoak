using UnityEngine;

public class JokalariaHil : MonoBehaviour {

    public bool zuzeneanHil = false;
    private JokalariKudetzailea jokalariKudetzailea;

	// Use this for initialization
	void Start () {
		jokalariKudetzailea = FindObjectOfType<JokalariKudetzailea>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (zuzeneanHil)
            {
                jokalariKudetzailea.JokalariaHil();
            }
            else
            {
                collision.transform.GetComponent<Eraso>().KolpeaJaso(Random.value > .5f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (zuzeneanHil)
            {
                jokalariKudetzailea.JokalariaHil();
            }
            else
            {
                collision.transform.GetComponent<Eraso>().KolpeaJaso(Random.value > .5f);
            }
        }
    }
}
