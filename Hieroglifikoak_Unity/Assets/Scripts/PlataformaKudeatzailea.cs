using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaKudeatzailea : IzpiKudeaketa {

    public LayerMask bidaiariak;                            // zein objektu mugituko da platafomarekin
    public LayerMask jokalaria;                             // zein objektu mugituko du plataforma (erortzen direnak eta bidai luzeak)
    //Dictionary<Transform, MugKudeatzaile> hiztegia = new Dictionary<Transform, MugKudeatzaile>();
    List<Transform> hiztegia = new List<Transform>();

    public Vector3[] marraztuPuntuak;
    Vector3[] itxarotePuntuak;
    public float PlataformaAbiadura;
    public bool zikloa;
    [Range(0,4)]
    public float easing;                    // plataforma mugikorren abiadura aldatu egiten da bidaiaren egoeraren arabera 0.1-0.9 -> ertzetan azkarra, 1-etik gora ertzak moteldu eta 1 abiadura kte
    public float itxaroteDenbora;
    float hurrengoDenbora;
    int itxarotePosizioa;
    float bidaiKantitatea;

    public bool bidaiLuzea = false;
    public bool reset = false;
    bool alderantziz = false;
    public bool lurreraJauzi = false;

    AudioSource audioa;
    public bool mugimenduAudioa;
    public bool kolpeAudioa;

    public override void Start () {
        base.Start();

        audioa = GetComponent<AudioSource>();
        if(audioa == null && (mugimenduAudioa || kolpeAudioa))
        {
            Debug.Log("Audio pista jartzea ahaztu zaizu");
        }

        itxarotePuntuak = new Vector3[marraztuPuntuak.Length];
        for (int i = 0; i < marraztuPuntuak.Length; i++)
        {
            itxarotePuntuak[i] = marraztuPuntuak[i] + transform.position;
        }
        if (bidaiLuzea || lurreraJauzi)
        {
            TranpakKudeatu(true);
        }
	}
	
	void Update () {
        Vector2 abiadura = Vector2.zero;
        IzpiJatorriaEguneratu();
        if (reset)
        {
            Restart();
        }
        else
        {
            if (itxarotePuntuak.Length != 0)
            {
                abiadura = PlataformaMugitu();
                
                if (abiadura.y >= 0)
                {
                    MugituBidaiariak(abiadura);
                    transform.Translate(abiadura);
                }
                else
                {
                    transform.Translate(abiadura);
                    MugituBidaiariak(abiadura);
                }
            }
        }
    }

    public void TranpakKudeatu(bool hasieratu)
    {
        reset = hasieratu;
        if (alderantziz)
        {
            System.Array.Reverse(itxarotePuntuak);
            alderantziz = !alderantziz;
        }

        itxarotePosizioa = 0;
        bidaiKantitatea = 0;
        transform.position = itxarotePuntuak[0];
    }

    public void Gelditu()
    {
        if (lurreraJauzi)
        {
            TranpakKudeatu(true);
        }
        else
        {
            reset = true;
        }
    }

    public void Berrabiarazi()
    {
        reset = false;
    }

    // jokalaria gainean jarri arte itzaroten gelditu
    public void Restart()
    {
        if(transform.position == itxarotePuntuak[0])
        {
            float izpiLuzera = 2 * azalZabalera;
            for (int i = 0; i < izpiBertKop; i++)
            {
                Vector2 jatorriIzpia = izpiJatorria.topLeft + Vector2.right * (bertIzpiTartea * i);
                RaycastHit2D kolpatu = Physics2D.Raycast(jatorriIzpia, Vector2.up, izpiLuzera, jokalaria);
                if (kolpatu)
                {
                    Berrabiarazi();
                }
            }
        }
    }

    Vector3 PlataformaMugitu()
    {
        if (mugimenduAudioa)
        {
            if (GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
            {
                if (!audioa.isPlaying)
                {
                    audioa.Play();
                }
            }
            else
            {
                audioa.Stop();
            }
        }

        if (Time.time < hurrengoDenbora)
        {
            return Vector3.zero;
        }

        itxarotePosizioa %= itxarotePuntuak.Length;
        int hurrengoPosizoia = (itxarotePosizioa + 1) % itxarotePuntuak.Length;
        float PuntuenArtekoDistantzia = Vector3.Distance(itxarotePuntuak[itxarotePosizioa], itxarotePuntuak[hurrengoPosizoia]);
        bidaiKantitatea += Time.deltaTime * PlataformaAbiadura / PuntuenArtekoDistantzia;
        bidaiKantitatea = Mathf.Clamp01(bidaiKantitatea);
        float easeBidaiKantitatea = Ease(bidaiKantitatea);

        Vector3 posizioBerria = Vector3.Lerp(itxarotePuntuak[itxarotePosizioa], itxarotePuntuak[hurrengoPosizoia], easeBidaiKantitatea);

        if (bidaiKantitatea > .9f && bidaiKantitatea < .95f)
        {
            if (kolpeAudioa)
            {
                if (GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
                {
                    if (!audioa.isPlaying)
                    {
                        audioa.Play();
                    }
                }
            }
        }

        if (bidaiKantitatea >= 1)
        {
            bidaiKantitatea = 0;
            itxarotePosizioa++;
            if (transform.position != itxarotePuntuak[0] && lurreraJauzi)
            {
                //TranpakKudeatu(true);
                Gelditu();
            }
            if (!zikloa)
            {
                if (itxarotePosizioa >= itxarotePuntuak.Length - 1)
                {
                    itxarotePosizioa = 0;
                    System.Array.Reverse(itxarotePuntuak);
                    alderantziz = !alderantziz;
                }
            }
            hurrengoDenbora = Time.time + itxaroteDenbora;
        }
        return posizioBerria - transform.position;
    }

    float Ease(float x)
    {
        //return Mathf.Pow(x, 2) * (3 - 2 * x);
        return Mathf.Pow(x, easing) / (Mathf.Pow(x, easing) + Mathf.Pow(1 - x, easing));
    }

    void MugituBidaiariak(Vector2 abiadura)
    {
        HashSet<Transform> mugitutakoBidaiariak = new HashSet<Transform>();

        bool gainean = false;
        // jokalaria plataforma gainean
        float izpiLuzera = 2 * azalZabalera;
        for (int i = 0; i < izpiBertKop; i++)
        {
            Vector2 jatorriIzpia = izpiJatorria.topLeft + Vector2.right * (bertIzpiTartea * i);
            RaycastHit2D kolpatu = Physics2D.Raycast(jatorriIzpia, Vector2.up, izpiLuzera, bidaiariak);

            Debug.DrawRay(jatorriIzpia, Vector2.up, Color.red);
            if (kolpatu)
            {
                if(transform.tag == "Player")
                {
                    gainean = true;
                }
                if (!mugitutakoBidaiariak.Contains(kolpatu.transform))
                {
                    mugitutakoBidaiariak.Add(kolpatu.transform);
                    if (!hiztegia.Contains(kolpatu.transform))
                    {
                        hiztegia.Add(kolpatu.transform);
                    }
                    Mugitu(kolpatu.transform, new Vector2(abiadura.x, abiadura.y), true);
                }
            }
        }

        if (!gainean)
        {
            izpiLuzera = 1.4f + 2 * azalZabalera;
            for (int i = 0; i < izpiBertKop; i++)
            {
                Vector2 jatorriIzpia = izpiJatorria.topLeft + Vector2.right * (bertIzpiTartea * i);
                RaycastHit2D[] kolpatu = Physics2D.RaycastAll(jatorriIzpia, Vector2.up, izpiLuzera, bidaiariak);
                //RaycastHit2D kolpatu = Physics2D.Raycast(jatorriIzpia, Vector2.up, izpiLuzera, bidaiariak);
                
                for (int j = 0; j < kolpatu.Length; j++)
                {
                    if (kolpatu[j])
                    {
                        if (kolpatu[j].transform.tag == "Player")
                        {
                            if (kolpatu[j].transform.GetComponent<JokalariMug>().KutxaGaineanDago())
                            {
                                if (!mugitutakoBidaiariak.Contains(kolpatu[j].transform))
                                {
                                    mugitutakoBidaiariak.Add(kolpatu[j].transform);
                                    if (!hiztegia.Contains(kolpatu[j].transform))
                                    {
                                        hiztegia.Add(kolpatu[j].transform);
                                    }
                                    Mugitu(kolpatu[j].transform, new Vector2(abiadura.x, abiadura.y), true);
                                }
                            }
                        }
                    }
                }
            }
        }


        // kolpe horizontala plataformaren aurka
        if (abiadura.x != 0)
        {
            float xNoranzkoa = Mathf.Sign(abiadura.x);
            izpiLuzera = Mathf.Abs(abiadura.x) + azalZabalera;
            for (int i = 0; i < izpiHorKop; i++)
            {
                Vector2 jatorriIzpia = xNoranzkoa == -1 ? izpiJatorria.bottomLeft : izpiJatorria.bottomRight;
                jatorriIzpia += Vector2.up * (horIzpiTartea * i);
                RaycastHit2D kolpatu = Physics2D.Raycast(jatorriIzpia, Vector2.right * xNoranzkoa, izpiLuzera, bidaiariak);

                Debug.DrawRay(jatorriIzpia, Vector2.right * xNoranzkoa, Color.red);

                if (kolpatu && kolpatu.distance != 0)
                {
                    if (!mugitutakoBidaiariak.Contains(kolpatu.transform))
                    {
                        mugitutakoBidaiariak.Add(kolpatu.transform);
                        float bultzatuX = abiadura.x - (kolpatu.distance - azalZabalera) * xNoranzkoa;
                        float bultzatuY = -azalZabalera;

                        if (!hiztegia.Contains(kolpatu.transform))
                        {
                            hiztegia.Add(kolpatu.transform);
                        }
                        Mugitu(kolpatu.transform, new Vector2(bultzatuX, bultzatuY), false);
                    }
                }
            }
        }
    }

    void Mugitu(Transform bidaiaria, Vector2 abiadura, bool plataformaGainean)
    {
        MugKudeatzaile jokalaria = bidaiaria.GetComponent<MugKudeatzaile>();
        if (jokalaria != null)
        {
            jokalaria.Mugitu(abiadura, plataformaGainean);
        }

        KutxaMugKud kutxa = bidaiaria.GetComponent<KutxaMugKud>();
        if (kutxa != null)
        {
            kutxa.Mugitu(abiadura, plataformaGainean);
        }
    }

    private void OnDrawGizmos()
    {
        if(marraztuPuntuak != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;
            for (int i = 0; i < marraztuPuntuak.Length; i++)
            {
                Vector3 puntua = (Application.isPlaying) ? itxarotePuntuak[i] : marraztuPuntuak[i] + transform.position;
                Gizmos.DrawLine(puntua + Vector3.down * size, puntua + Vector3.up * size);
                Gizmos.DrawLine(puntua + Vector3.right * size, puntua + Vector3.left * size);
            }
        }
    }
}
