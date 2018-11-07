using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCam : MonoBehaviour {

    CinemachineVirtualCamera cam;
    JokalariMug jokalaria;
    Transform target;
    float lookAhead;

	// Use this for initialization
	void Start () {
        cam = GetComponent<CinemachineVirtualCamera>();
        lookAhead = cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime;
        target = cam.m_Follow;
        if (target.GetComponent<JokalariMug>())
        {
            jokalaria = cam.m_Follow.GetComponent<JokalariMug>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (jokalaria.GetHiltzen())
        {
            LookAheadKudeatu(true);
        }
        else
        {
            LookAheadKudeatu(false);
        }
	}

    void LookAheadKudeatu(bool kendu)
    {
        if (kendu)
        {
            cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = 0;
        }
        else
        {
            cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = lookAhead;
        }
    }

    void SetFollow(Transform target)
    {
        cam.m_Follow = target;
    }
}
