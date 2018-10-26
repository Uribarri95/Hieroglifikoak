using UnityEngine;

public class ItemHartu : MonoBehaviour {

    public Item item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            print(item.izena);
            bool tokiaDago = Inbentarioa.instantzia.Add(item);
            if (tokiaDago)
            {
                Destroy(gameObject);
            }
        }
    }

}
