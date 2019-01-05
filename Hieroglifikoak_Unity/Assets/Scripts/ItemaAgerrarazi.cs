using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemaAgerrarazi : MonoBehaviour {

    public GameObject bihotza;
    public GameObject geziak;
    //public GameObject txanpona;       // !!! txanponak jarri?
    public float itemEhunekoa;

    public void ItemAtera()
    {
        float zenbakia = Random.Range(0, 100);
        if(zenbakia < itemEhunekoa)
        {
            if(Random.Range(0,2) == 0)
            {
                Instantiate(bihotza, transform.position, transform.rotation);
            }
            else
            {
                Instantiate(geziak, transform.position, transform.rotation);
            }
        }
    }
}
