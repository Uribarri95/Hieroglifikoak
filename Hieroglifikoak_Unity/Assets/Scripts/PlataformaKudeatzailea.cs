using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaKudeatzailea : IzpiKudeaketa {

    public LayerMask bidaiariak;
    Dictionary<Transform, MugKudeatzaile> hiztegia = new Dictionary<Transform, MugKudeatzaile>();
    //MugKudeatzaile mugKudeatzailea = FindObjectOfType<MugKudeatzaile>();

    public Vector3[] marraztuPuntuak;
    Vector3[] itzarotePuntuak;
    public float PlataformaAbiadura;
    public bool zikloa;
    [Range(0,3)]
    public float easing;
    int itzarotePosizioa;
    float bidaiKantitatea;

    public override void Start () {
        base.Start();
        itzarotePuntuak = new Vector3[marraztuPuntuak.Length];
        for (int i = 0; i < marraztuPuntuak.Length; i++)
        {
            itzarotePuntuak[i] = marraztuPuntuak[i] + transform.position;
        }
	}
	
	void Update () {
        Vector2 abiadura = Vector2.zero;
        if(itzarotePuntuak.Length != 0)
        {
            IzpiJatorriaEguneratu();
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

    float Ease(float x)
    {
        float a = easing + 1;
        //return Mathf.Pow(x, 2) * (3 - 2 * x);
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    Vector3 PlataformaMugitu()
    {
        itzarotePosizioa %= itzarotePuntuak.Length;
        int hurrengoPosizoia = (itzarotePosizioa + 1) % itzarotePuntuak.Length;
        float PuntuenArtekoDistantzia = Vector3.Distance(itzarotePuntuak[itzarotePosizioa], itzarotePuntuak[hurrengoPosizoia]);
        bidaiKantitatea += Time.deltaTime * PlataformaAbiadura / PuntuenArtekoDistantzia;
        bidaiKantitatea = Mathf.Clamp01(bidaiKantitatea);
        float easeBidaiKantitatea = Ease(bidaiKantitatea);

        Vector3 posizioBerria = Vector3.Lerp(itzarotePuntuak[itzarotePosizioa], itzarotePuntuak[hurrengoPosizoia], easeBidaiKantitatea);
        if (bidaiKantitatea >= 1)
        {
            bidaiKantitatea = 0;
            itzarotePosizioa++;
            if (!zikloa)
            {
                if (itzarotePosizioa >= itzarotePuntuak.Length - 1)
                {
                    itzarotePosizioa = 0;
                    System.Array.Reverse(itzarotePuntuak);
                }
            }
        }
        return posizioBerria - transform.position;
    }

    void MugituBidaiariak(Vector2 abiadura)
    {
        HashSet<Transform> mugitutakoBidaiariak = new HashSet<Transform>();
        float xNoranzkoa = Mathf.Sign(abiadura.x);

        // jokalaria plataforma gainean
        float izpiLuzera = 2 * azalZabalera;
        for (int i = 0; i < izpiBertKop; i++)
        {
            Vector2 jatorriIzpia = izpiJatorria.topLeft + Vector2.right * (bertIzpiTartea * i);
            RaycastHit2D kolpatu = Physics2D.Raycast(jatorriIzpia, Vector2.up, izpiLuzera, bidaiariak);

            Debug.DrawRay(jatorriIzpia, Vector2.up, Color.red);
            if (kolpatu)
            {
                /// !!! kutxa mugitzeko konponketak
                //if (kolpatu.transform.tag == "Player")
                if (!mugitutakoBidaiariak.Contains(kolpatu.transform))
                {
                    mugitutakoBidaiariak.Add(kolpatu.transform);
                    float bultzatuX = abiadura.x;
                    float bultzatuY = abiadura.y;
                    
                    if (!hiztegia.ContainsKey(kolpatu.transform))
                    {
                        hiztegia.Add(kolpatu.transform, kolpatu.transform.GetComponent<MugKudeatzaile>());
                    }
                    hiztegia[kolpatu.transform].Mugitu(new Vector2(bultzatuX, bultzatuY), plataformaGainean: true);
                }
            }
        }

        // kolpe horizontala plataformaren aurka
        if (abiadura.x != 0)
        {
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
                        
                        if (!hiztegia.ContainsKey(kolpatu.transform))
                        {
                            hiztegia.Add(kolpatu.transform, kolpatu.transform.GetComponent<MugKudeatzaile>());
                        }
                        hiztegia[kolpatu.transform].Mugitu(new Vector2(bultzatuX, bultzatuY), plataformaGainean: false);
                    }
                }
            }
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
                Vector3 puntua = (Application.isPlaying) ? itzarotePuntuak[i] : marraztuPuntuak[i] + transform.position;
                Gizmos.DrawLine(puntua + Vector3.down * size, puntua + Vector3.up * size);
                Gizmos.DrawLine(puntua + Vector3.right * size, puntua + Vector3.left * size);
            }
        }
    }
}
