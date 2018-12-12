using UnityEngine;
using UnityEngine.UI;

public class BihotzTokia : MonoBehaviour {

    public Image icon;

    public void AddBihotza(Sprite irudia)
    {
        icon.sprite = irudia;
        icon.enabled = true;
    }
}
