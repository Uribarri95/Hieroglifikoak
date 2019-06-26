using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {

    public GameObject dialogCanvas;
    public float itxaroteDenbora = 1;
    public bool puzleaErakutsi;
    public int zenbakia;
    public Dialog dialog;

    public bool pasahitzak;
    public int pasahitzKopurua;
    private float denbora = 2;
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            print("pasahitzKop: " + Inbentarioa.instantzia.GetPasahitzKop());
        }
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
        yield return new WaitForSeconds(itxaroteDenbora);
        dialogCanvas.SetActive(true);
        if (pasahitzak)
        {
            if(pasahitzKopurua != Inbentarioa.instantzia.GetPasahitzKop())
            {
                pasahitzakFaltan.esaldiak = new string[] { pasahitzKopurua - Inbentarioa.instantzia.GetPasahitzKop() + " papiro hartzea falta zaizu." };
                dialogManager.StartDialog(pasahitzakFaltan, false, 0);
                yield return new WaitForSeconds(denbora);
                exekutatu = false;
            }
            else
            {
                Inbentarioa.instantzia.PasahitzakReset();
                dialogManager.StartDialog(dialog, puzleaErakutsi, zenbakia);
                gameObject.SetActive(false);
            }
        }
        else
        {
            dialogManager.StartDialog(dialog, puzleaErakutsi, zenbakia);
            gameObject.SetActive(false);
        }
    }
}
