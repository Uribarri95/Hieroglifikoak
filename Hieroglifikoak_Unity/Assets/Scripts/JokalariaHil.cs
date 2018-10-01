using UnityEngine;

public class JokalariaHil : MonoBehaviour {

    private JokalariKudetzailea jokalariKudetzailea;

	// Use this for initialization
	void Start () {
		jokalariKudetzailea = FindObjectOfType<JokalariKudetzailea>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            jokalariKudetzailea.JokalariaHil();
        }
    }
}
