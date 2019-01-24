using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {

    public GameObject dialogCanvas;
    public float itzaroteDenbora = 1;
    public bool puzleaErakutsi;
    public int zenbakia;
    public Dialog dialog;

    DialogManager dialogManager;
    private bool exekutatu = false;

    private void Start()
    {
        dialogManager = DialogManager.instantzia;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!exekutatu)
            {
                exekutatu = true;
                StartCoroutine(TriggerDialog());
                // jokalaria gelditu ||jokoa gelditu
            }
        }
    }

    public IEnumerator TriggerDialog()
    {
        yield return new WaitForSeconds(itzaroteDenbora);
        dialogCanvas.SetActive(true);
        dialogManager.StartDialog(dialog, puzleaErakutsi, zenbakia);
    }
}
