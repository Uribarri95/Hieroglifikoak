﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour {

    public float particleDestroyTime;

	void Start () {
        Destroy(gameObject, particleDestroyTime);
    }
}
