using UnityEngine;

//[System.Serializable]
[CreateAssetMenu(fileName = "Item berria", menuName = "Inbentarioa/Item")]
public class Item : ScriptableObject {

    public string izena;
    public Sprite irudia;
    public bool erbileraBakarra = false;

}
