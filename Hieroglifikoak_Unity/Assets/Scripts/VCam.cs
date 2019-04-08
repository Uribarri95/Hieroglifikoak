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

    bool aldaketak = false;
    float lookAhead;
    float width;
    float hight;
    float xDamp;
    float yDamp;

	// Use this for initialization
	void Start () {
        cam = GetComponent<CinemachineVirtualCamera>();

        confiner = cam.GetComponent<CinemachineConfiner>();
        bounds = cameraBounds.GetComponentsInChildren<PolygonCollider2D>();

        lookAhead = cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime;
        width = cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth;
        hight = cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight;
        xDamp = cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping;
        yDamp = cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping;

        target = cam.m_Follow;
        if (target.GetComponent<JokalariMug>())
        {
            jokalaria = cam.m_Follow.GetComponent<JokalariMug>();
            //CameraConfinerKudeatu(jokalaria.transform.position);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (jokalaria.GetHiltzen())
        {
            cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = 0;
        }
        else if(jokalaria.berpizten && !aldaketak)
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
                aldaketak = true;
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = 0;
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0;
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0;
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = 0;
                cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0;
                StartCoroutine(Itxaron());
                break;
            }
        }
    }

    IEnumerator Itxaron()
    {
        yield return new WaitForSeconds(2f);
        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_LookaheadTime = lookAhead;
        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = width;
        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = hight;
        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = xDamp;
        cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = yDamp;
        aldaketak = false;
    }

    void SetFollow(Transform target)
    {
        cam.m_Follow = target;
    }
}
