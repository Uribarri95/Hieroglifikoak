using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCam : MonoBehaviour {

    CinemachineVirtualCamera cam;
    CinemachineConfiner confiner;
    JokalariMug jokalaria;
    Transform target;
    PolygonCollider2D[] bounds;

    public GameObject cameraBounds;

    float lookAhead;
    float width;
    float hight;

	// Use this for initialization
	void Start () {
        cam = GetComponent<CinemachineVirtualCamera>();

        confiner = cam.GetComponent<CinemachineConfiner>();
        bounds = cameraBounds.GetComponentsInChildren<PolygonCollider2D>();

        lookAhead = cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime;
        width = cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth;
        hight = cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight;

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

    public void CameraConfinerKudeatu(Vector2 pos)
    {
        for (int i = 0; i < bounds.Length; i++)
        {
            if (bounds[i].bounds.Contains(pos))
            {
                confiner.m_BoundingShape2D = bounds[i];
                confiner.InvalidatePathCache();
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0;
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0;
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0;
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0;
                StartCoroutine(Itxaron());
            }
        }
    }

    IEnumerator Itxaron()
    {
        yield return new WaitForSeconds(.8f);
        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = width;
        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = hight;
        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 1;
        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1;
    }

    void SetFollow(Transform target)
    {
        cam.m_Follow = target;
    }
}
