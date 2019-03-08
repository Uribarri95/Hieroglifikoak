using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public FadeManager fadeManager;
    public Animator backAnim;
    public Animator menuAnim;
    Data data;
    Data.PlayerData jokalariDatuak;

    private void Start()
    {
        data = Data.instantzia;
        fadeManager.Argitu();
    }

    public void Jolastu()
    {
        jokalariDatuak = data.JokalariDatuakKargatu();
        menuAnim.SetTrigger("play");
        print("jolastu -> datuak gordeta? " + jokalariDatuak.datuakGordeta);
        if (jokalariDatuak.datuakGordeta)
        {
            print("datuak gordeta");
            menuAnim.SetBool("partidaGordeta",true);
        }
        else
        {
            print("ez duzu daturik gordeta");
            menuAnim.SetBool("partidaGordeta", false);
            int sceneToLoad = jokalariDatuak.Eszenatokia;
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
        jokalariDatuak = data.JokalariDatuakKargatu();
        int sceneToLoad = jokalariDatuak.Eszenatokia;
        backAnim.SetTrigger("start");
        fadeManager.FadeToScene(sceneToLoad);
    }

    public void Hasi()
    {
        data.DatuakEzabatu();
        menuAnim.SetTrigger("jarraituhasi");
        jokalariDatuak = data.JokalariDatuakKargatu();
        jokalariDatuak = data.JokalariDatuBerriak();
        int sceneToLoad = jokalariDatuak.Eszenatokia;
        backAnim.SetTrigger("start");
        fadeManager.FadeToScene(sceneToLoad);
    }
}
