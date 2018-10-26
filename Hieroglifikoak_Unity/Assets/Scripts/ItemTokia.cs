using UnityEngine;
using UnityEngine.UI;

public class ItemTokia : MonoBehaviour {

    public Image icon;

    public void AddItem(Sprite irudia)
    {
        icon.sprite = irudia;
        icon.enabled = true;
    }
}
