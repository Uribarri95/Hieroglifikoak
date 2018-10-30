using UnityEngine;

public class ItemHartu : MonoBehaviour {

    public Item item;
    Animator anim;

    private void Start()
    {
        if (item.izena == "SuArgia")
        {
            anim = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            print(item.izena);
            bool tokiaDago = Inbentarioa.instantzia.Add(item);
            if (tokiaDago)
            {
                if(item.izena == "SuArgia")
                {
                    anim.SetBool("TakeTorch", true);
                    return;
                }
                Destroy(gameObject);
            }
        }
    }

}
