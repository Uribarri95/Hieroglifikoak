using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OntziaAukeratu : MonoBehaviour {

    public bool pitxerra;
    Animator anim;

	void Start () {
        anim = GetComponent<Animator>();
        if (pitxerra)
        {
            anim.SetBool("pitxerra", true);
        }
        else
        {
            anim.SetBool("pitxerra", false);
        }
	}

}
