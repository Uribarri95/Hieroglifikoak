using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class IzpiKudeaketa : MonoBehaviour {

    //kolpeGainazalak geruzan dauden gorputzekin bakarrik egingo dugu talka
    [HideInInspector]
    public BoxCollider2D bc2d;

    public const float azalZabalera = .015f;

    const float izpiDistantzia = .1f;
    [HideInInspector]
    public int izpiHorKop;
    [HideInInspector]
    public int izpiBertKop;
    [HideInInspector]
    public float horIzpiTartea;
    [HideInInspector]
    public float bertIzpiTartea;

    public IzpiJatorria izpiJatorria;

    // start baino lehe exekutatzen da
    private void Awake()
    {
        bc2d = GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        IzpiTarteakKalkulatu();
    }

    // ertz batetik bestera jaurti behar diren izpi kopurua erabakitako izpiDistantzia kontuan hartuta
    public void IzpiTarteakKalkulatu()
    {
        Bounds mugak = bc2d.bounds;
        mugak.Expand(azalZabalera * -2);

        float altuera = mugak.size.y;
        float luzera = mugak.size.x;

        izpiHorKop = Mathf.RoundToInt(altuera / izpiDistantzia);
        izpiBertKop = Mathf.RoundToInt(luzera / izpiDistantzia);

        horIzpiTartea = mugak.size.y / (izpiHorKop - 1);
        bertIzpiTartea = mugak.size.x / (izpiBertKop - 1);
    }

    // jokalariaren posizio berria kontuan hartuta izpi berrien igorpen puntuak kalkulatzen dira
    public void IzpiJatorriaEguneratu()
    {
        Bounds mugak = bc2d.bounds;
        mugak.Expand(azalZabalera * -2);

        izpiJatorria.bottomLeft = new Vector2(mugak.min.x, mugak.min.y);
        izpiJatorria.bottomRight = new Vector2(mugak.max.x, mugak.min.y);
        izpiJatorria.topLeft = new Vector2(mugak.min.x, mugak.max.y);
        izpiJatorria.topRight = new Vector2(mugak.max.x, mugak.max.y);
    }

    // talkak kudeatzeko izpiak
    public struct IzpiJatorria
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
