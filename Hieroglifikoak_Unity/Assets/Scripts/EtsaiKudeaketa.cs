using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtsaiKudeaketa : MonoBehaviour {

    // !!! destroy beharrean setactivefalse?
    // !!! DISEINUA: etsaiak[] prefab-etik hartu -->> ordena mantendu!!!
    public GameObject[] etsaiak;
    float etsaiKop;
    float[] etsaiRotation;
    Vector2[] etsaiPos;
    public bool reset;

	// Use this for initialization
	void Start () {
        etsaiKop = etsaiak.Length;
        if(etsaiKop != 0)
        {
            etsaiPos = new Vector2[etsaiak.Length];
            etsaiRotation = new float[etsaiak.Length];
            for (int i = 0; i < etsaiak.Length; i++)
            {
                etsaiPos[i] = transform.GetChild(i).position;
                etsaiRotation[i] = transform.GetChild(i).transform.rotation.y;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (reset)
        {
            reset = false;
            EtsaiakReset();
        }
	}

    public void EtsaiakReset()
    {
        int childs = transform.childCount;
        for (int i = 0; i < childs; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < etsaiak.Length; i++)
        {
            //GameObject etsaia =  Instantiate(etsaiak[i], etsaiPos[i], transform.rotation);
            GameObject etsaia = Instantiate(etsaiak[i], etsaiPos[i], new Quaternion(0, etsaiRotation[i], 0, 0));
            etsaia.transform.parent = transform;
        }
    }
}
