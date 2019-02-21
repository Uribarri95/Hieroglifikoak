using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public FadeManager fadeManager;
    public Animator backAnim;
    public Animator menuAnim;
    Data playerData;
    Data.PlayerData datuak;

    private void Start()
    {
        playerData = Data.instantzia;
        fadeManager.Argitu();
    }

    public void Jolastu()
    {
        datuak = playerData.Kargatu();
        int sceneToLoad;
        menuAnim.SetTrigger("play");
        if (datuak.datuakGordeta)
        {
            print("datuak gordeta");
            menuAnim.SetBool("partidaGordeta",true);
        }
        else
        {
            print("ez duzu daturik gordeta");
            menuAnim.SetBool("partidaGordeta", false);
            sceneToLoad = datuak.Eszenatokia;
            backAnim.SetTrigger("start");
            fadeManager.FadeToScene(sceneToLoad);
        }
        
    }

    public void Irten()
    {
        Debug.Log("Irteten");
        Application.Quit();
    }

    public void Jarraitu()
    {
        menuAnim.SetTrigger("jarraituhasi");
        int sceneToLoad = datuak.Eszenatokia;
        backAnim.SetTrigger("start");
        fadeManager.FadeToScene(sceneToLoad);
    }

    public void Hasi()
    {
        menuAnim.SetTrigger("jarraituhasi");
        datuak = playerData.DatuBerriak();
        int sceneToLoad = datuak.Eszenatokia;
        backAnim.SetTrigger("start");
        fadeManager.FadeToScene(sceneToLoad);
    }
}
