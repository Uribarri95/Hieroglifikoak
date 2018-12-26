using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableUI : MonoBehaviour {


    public void UIGaitu()
    {
        GetComponent<Image>().enabled = true;
        GetComponentInChildren<Text>().enabled = true;
    }
}
