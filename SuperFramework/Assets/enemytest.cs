using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemytest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Shoot());
	}

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(2f);

        vp_FPPlayerEventHandler m_FPPlayer = transform.root.GetComponentInChildren<vp_FPPlayerEventHandler>();

        m_FPPlayer.Crouch.TryStart();
        //m_FPPlayer.Interact.TryStart();
        //m_FPPlayer.Jump.TryStart();
        //m_FPPlayer.Run.TryStart();
        //m_FPPlayer.Attack.TryStart();
        //yield return null;
        //m_FPPlayer.Attack.TryStop();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
