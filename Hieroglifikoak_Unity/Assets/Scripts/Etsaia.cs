using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Etsaia : MonoBehaviour {

    public float bizitzaPuntuak;        // etsaiaren bizitza puntuak
    public GameObject hilPartikula;     // etsaia hiltzeam agertzen den efektua

    bool kolpatuta = false;             // jokalariak kolpe bat eraso bakoitzeko
    bool knockBack = false;             // etsaiak kolpea jaso duen efektua lortzeko
    bool bizirik = true;                // jokalaria bizirik jarraitzen duen

    // etsaiari min puntuak kendu
    public void KolpeaJaso(float minPuntuak)
    {
        if (!kolpatuta)
        {
            knockBack = true;
            bizitzaPuntuak -= minPuntuak;
            if (bizitzaPuntuak <= 0)
            {
                bizirik = false;
                Hil();
            }
        }
    }

    // min puntuak 0 direnean etsaia hil, mumia denean bere IA barruan egingo da
    public void Hil()
    {
        if(!transform.name.Contains("Mummy"))
        {
            Destroy(gameObject);
        }
        Instantiate(hilPartikula, transform.position, Quaternion.identity); // !!! parent berdinarekin
    }

    // etsaia eraso bakoitzean bakarrik behin kolpatzeko
    public void SetKolpea(bool kolpea)
    {
        kolpatuta = kolpea;
    }

    // etsaia bizirik jarraitzen duen jakiteko, mumiaren IA bere burua hiltzeko
    public bool GetBizirik()
    {
        return bizirik;
    }

    // kolpea jasotzean IA denak knockback efektua egin dezaten
    public bool GetKnockBack()
    {
        return knockBack;
    }

    // IA denak knockback efektua egin ondoren 'entzuten' geratzen da berriro
    public void KnockBackErreseteatu()
    {
        knockBack = false;
    }
}
