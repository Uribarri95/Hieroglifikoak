using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraMugimendua : MonoBehaviour {

    public MugKudeatzaile helburua;
    public Vector2 kameraGeldiTamaina;

    KameraGeldiGunea kameraGeldiGunea;

	// Use this for initialization
	void Start () {
        kameraGeldiGunea = new KameraGeldiGunea(helburua.bc2d.bounds, kameraGeldiTamaina);
    }

    private void LateUpdate()
    {
        kameraGeldiGunea.Update(helburua.bc2d.bounds);
        transform.position = (Vector3)kameraGeldiGunea.erdigunea + Vector3.forward * -10;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(kameraGeldiGunea.erdigunea, kameraGeldiTamaina);
    }

    struct KameraGeldiGunea
    {
        public Vector2 erdigunea;
        public Vector2 abiadura;
        float ezkerra, eskuma;
        float goian, behean;

        public KameraGeldiGunea(Bounds jokalariMugak, Vector2 kameraGeldiTamaina)
        {
            ezkerra = jokalariMugak.center.x - kameraGeldiTamaina.x / 2;
            eskuma = jokalariMugak.center.x + kameraGeldiTamaina.x / 2;
            behean = jokalariMugak.center.y - kameraGeldiTamaina.y / 2;
            goian = jokalariMugak.center.y + kameraGeldiTamaina.y / 2;

            erdigunea = new Vector2((ezkerra+eskuma)/2,(goian + behean)/2);
            abiadura = Vector2.zero;
        }

        public void Update(Bounds jokalariMugak)
        {
            float mugituX = 0;
            if (jokalariMugak.min.x < ezkerra)
            {
                mugituX = jokalariMugak.min.x - ezkerra;
            }
            else if (jokalariMugak.max.x > eskuma)
            {
                mugituX = jokalariMugak.max.x - eskuma;
            }
            ezkerra += mugituX;
            eskuma += mugituX;

            float mugituY = 0;
            if (jokalariMugak.min.y < behean)
            {
                mugituY = jokalariMugak.min.y - behean;
            }
            else if (jokalariMugak.max.y > goian)
            {
                mugituY = jokalariMugak.max.y - goian;
            }
            behean += mugituY;
            goian += mugituY;
            erdigunea = new Vector2((ezkerra + eskuma) / 2, (goian + behean) / 2);
            abiadura = new Vector2(mugituX, mugituY);
        }
    }
}
