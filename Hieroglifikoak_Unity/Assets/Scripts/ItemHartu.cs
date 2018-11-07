using UnityEngine;

public class ItemHartu : MonoBehaviour {

    public Item item;
    Animator torchAnim;

    private void Start()
    {
        if (item.izena == "SuArgia")
        {
            torchAnim = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            bool tokiaDago = Inbentarioa.instantzia.Add(item);
            if (tokiaDago)
            {
                // itemHartu animazioa? !!!
                if(item.izena == "SuArgia")
                {
                    torchAnim.SetBool("TakeTorch", true);
                    GetComponentInChildren<SpriteMask>().enabled = false;
                    return;
                }
                Destroy(gameObject);
            }
        }
    }

}
