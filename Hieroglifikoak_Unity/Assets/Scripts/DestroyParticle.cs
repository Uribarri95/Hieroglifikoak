using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour {

    public float particleDestroyTime;
    public AudioSource audioa;

	void Start () {
        if (GetComponentInChildren<SoinuGunea>() != null && GetComponentInChildren<SoinuGunea>().EntzunDaiteke())
        {
            audioa.Play();
        }
        Destroy(gameObject, particleDestroyTime);
    }
}
