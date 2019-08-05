using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDialogTrigger : MonoBehaviour {

    public GameObject dialogCanvas;
    public float itzaroteDenbora;
    public Dialog dialog;

    private bool exekutatu = false;
    DialogManager dialogManager;

    private void Start()
    {
        dialogManager = DialogManager.instantzia;
    }

    private void Update()
    {
        if (gameObject.activeSelf && !exekutatu)
        {
            exekutatu = true;
            StartCoroutine(TriggerDialog());
        }
    }

    public IEnumerator TriggerDialog()
    {
        yield return new WaitForSeconds(itzaroteDenbora);
        dialogCanvas.SetActive(true);
        dialogManager.StartDialog(dialog, false, -1);
    }

    public void ArgibideakJarri()
    {
        exekutatu = !exekutatu;
        //exekutatu = false;
    }
}
