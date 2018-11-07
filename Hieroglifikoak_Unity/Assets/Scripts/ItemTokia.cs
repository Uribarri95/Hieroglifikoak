using UnityEngine;
using UnityEngine.UI;

public class ItemTokia : MonoBehaviour {

    public Image icon;
    public Image holder;
    public Text geziak;

    public void AddItem(Sprite irudia)
    {
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
        geziak.enabled = true;
    }

    public void EzkutatuGeziKop()
    {
        holder.enabled = false;
        geziak.enabled = false;
    }
}
