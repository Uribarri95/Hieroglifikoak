using UnityEngine;
using UnityEngine.UI;

public class ItemTokia : MonoBehaviour {

    public Image icon;
    public Image holder;
    public Text geziak;

    public void AddItem(Sprite irudia)
    {
        GetComponent<Image>().enabled = true;
        if (GetComponentInChildren<EnableUI>())
        {
            GetComponentInChildren<EnableUI>().UIGaitu();
        }
        icon.sprite = irudia;
        if (!icon.enabled)
        {
            icon.enabled = true;
        }
    }

    public void ErakutsiGeziKop(int geziKop)
    {
        holder.enabled = true;
        geziak.text = geziKop.ToString();
        if(geziKop == 0)
        {
            geziak.color = Color.red;
        }
        geziak.enabled = true;
    }

    public void EzkutatuGeziKop()
    {
        holder.enabled = false;
        geziak.enabled = false;
    }
}
