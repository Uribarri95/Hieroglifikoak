using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraMugimendua : MonoBehaviour {

    public MugKudeatzaile jokalaria;
    public float offset;
    public float distantziaX;
    public float distantziaY;
    public Vector2 mugak;
    public bool marraztuMugak;

    KameraGeldiGunea kameraGeldiGunea;

    // !!! kamera bounds !!!
    // gelak -> kamera jokalaria erdian mantendu
    // gela aldatzean kamera aldera mugitu

	void Start () {
        kameraGeldiGunea = new KameraGeldiGunea(jokalaria.bc2d.bounds, distantziaX, distantziaY);
    }

    private void LateUpdate()
    {
        kameraGeldiGunea.Update(jokalaria.bc2d.bounds);
        Vector2 kameraPosizioa = kameraGeldiGunea.erdigunea + Vector2.up * offset;

        transform.position = (Vector3)kameraPosizioa + Vector3.forward * -10;
    }

    private void OnDrawGizmos()
    {
        if (marraztuMugak)
        {
            Gizmos.color = new Color(1, 0, 0, .5f);
            Gizmos.DrawCube(kameraGeldiGunea.erdigunea, new Vector2(distantziaX * 2, distantziaY * 2));
        }
    }

    struct KameraGeldiGunea
    {
        public Vector2 erdigunea;
        float ezkerra, eskuma;
        float goian, behean;

        public KameraGeldiGunea(Bounds jokalariMugak, float distX, float distY)
        {
            ezkerra = jokalariMugak.center.x - distX;
            eskuma = jokalariMugak.center.x + distX;
            behean = jokalariMugak.center.y - distY;
            goian = jokalariMugak.center.y + distY;

            erdigunea = new Vector2((ezkerra + eskuma) / 2, (goian + behean) / 2);
        }

        public void Update(Bounds jokalariMugak)
        {
            float mugituX = 0;
            if (jokalariMugak.min.x < ezkerra)
                mugituX = jokalariMugak.min.x - ezkerra;
            else if (jokalariMugak.max.x > eskuma)
                mugituX = jokalariMugak.max.x - eskuma;
            ezkerra += mugituX;
            eskuma += mugituX;

            float mugituY = 0;
            if (jokalariMugak.min.y < behean)
                mugituY = jokalariMugak.min.y - behean;
            else if (jokalariMugak.max.y > goian)
                mugituY = jokalariMugak.max.y - goian;
            behean += mugituY;
            goian += mugituY;
            erdigunea = new Vector2((ezkerra + eskuma) / 2, (goian + behean) / 2);
        }
    }
}
