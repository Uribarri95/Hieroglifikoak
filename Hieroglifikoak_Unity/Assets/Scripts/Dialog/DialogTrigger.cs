using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {

    public GameObject dialogCanvas;
    public float itzaroteDenbora = 1;
    public bool puzleaErakutsi;
    public int zenbakia;
    public Dialog dialog;

    public bool pasahitzak;
    public int pasahitzKopurua;
    Dialog pasahitzakFaltan = new Dialog();

    DialogManager dialogManager;
    private bool exekutatu = false;

    private void Start()
    {
        dialogManager = DialogManager.instantzia;
        /*if (pasahitzak)
        {
            pasahitzakFaltan.esaldiak = new string[] { pasahitzKopurua - Inbentarioa.instantzia.GetPasahitzKop() + " papiro hartzea falta zaigu."};
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!exekutatu)
            {
                exekutatu = true;
                StartCoroutine(TriggerDialog());
            }
        }
    }

    public IEnumerator TriggerDialog()
    {
        print("beharrezkoak: " + pasahitzKopurua);
        print("hartutakoak: " + Inbentarioa.instantzia.GetPasahitzKop());

        yield return new WaitForSeconds(itzaroteDenbora);
        dialogCanvas.SetActive(true);
        if (pasahitzak && pasahitzKopurua != Inbentarioa.instantzia.GetPasahitzKop())
        {
            pasahitzakFaltan.esaldiak = new string[] { pasahitzKopurua - Inbentarioa.instantzia.GetPasahitzKop() + " papiro hartzea falta zaigu." };
            dialogManager.StartDialog(pasahitzakFaltan, false, 0);
            exekutatu = false;
        }
        else
        {
            Inbentarioa.instantzia.PasahitzakReset();
            dialogManager.StartDialog(dialog, puzleaErakutsi, zenbakia);
            gameObject.SetActive(false);
        }
    }
}
