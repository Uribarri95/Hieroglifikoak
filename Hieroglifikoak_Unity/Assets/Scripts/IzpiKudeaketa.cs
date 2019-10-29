using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class IzpiKudeaketa : MonoBehaviour {

    [HideInInspector]
    public BoxCollider2D bc2d;                              // kolpeak hautemateko kutxa formako geruza
    public const float azalZabalera = .015f;                // gorputzak beste baten barruan ez sartzeko badaezpadako distantzia

    const float izpiDistantzia = .1f;                       // izpien arteko distantzia gutxi gora behera
    [HideInInspector]
    public int izpiHorKop;                                  // izpi horizontal kopurua
    [HideInInspector]
    public int izpiBertKop;                                 // izpi bertikal kopurua
    [HideInInspector]
    public float horIzpiTartea;                             // izpi horizontalen arteko tartea
    [HideInInspector]
    public float bertIzpiTartea;                            // izpi bertikalen arteko tartea

    public IzpiJatorria izpiJatorria;                       // boxcollider-aren lau erpinak

    // start baino lehen exekutatzen da
    private void Awake()
    {
        bc2d = GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        IzpiTarteakKalkulatu();
    }

    // ertz batetik bestera jaurti behar diren izpi kopurua eta tartea izpiDistantzia kontuan hartuta
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

    // boxCollider-aren lau erpinak
    public struct IzpiJatorria
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
