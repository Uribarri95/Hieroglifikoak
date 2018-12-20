﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BizitzaPuntuakUI : MonoBehaviour {

    public Sprite bihotzikEz;
    public Sprite bihotzErdia;
    public Sprite bihotzOsoa;

    Inbentarioa inbentario;
    BihotzTokia[] bihotzak;

	// Use this for initialization
	void Start () {
        inbentario = Inbentarioa.instantzia;
        inbentario.itemJasoDeitu += BihotzUIEguneratu;
        bihotzak = GetComponentsInChildren<BihotzTokia>();
    }
	
	void BihotzUIEguneratu()
    {
        int bihotzOsoak = inbentario.bizitzaPuntuak / 2;
        bool bErdia = inbentario.bizitzaPuntuak % 2 > 0;
        for (int i = 1; i < (inbentario.bizitzaPuntuMax / 2) + 1; i++)
        {
            if (bErdia)
            {
                if(bihotzOsoak + 1 > i)
                {
                    bihotzak[i - 1].AddBihotza(bihotzOsoa);
                }
                else if(bihotzOsoak + 1 == i)
                {
                    bihotzak[i - 1].AddBihotza(bihotzErdia);
                }
                else
                {
                    bihotzak[i - 1].AddBihotza(bihotzikEz);
                }
            }
            else
            {
                if(bihotzOsoak >= i)
                {
                    bihotzak[i - 1].AddBihotza(bihotzOsoa);
                }
                else
                {
                    bihotzak[i - 1].AddBihotza(bihotzikEz);
                }
            }
        }
        // Edabea irudikatu
        if (inbentario.edabea != null)
        {
            bihotzak[inbentario.bizitzaPuntuMax / 2].AddBihotza(inbentario.edabea.irudia);
        }
    }

}