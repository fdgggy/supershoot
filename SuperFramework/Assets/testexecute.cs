using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testexecute : MonoBehaviour {

    bool update = false;
    bool lateupdate = false;
    private void OnEnable()
    {
        Debug.Log("OnEnable");
    }
    private void Awake()
    {
        Debug.Log("Awake");
    }
    // Use this for initialization
    void Start () {
        Debug.Log("Start");
    }
	
	// Update is called once per frame
	void Update () {
		if (update == false)
        {
            Debug.Log("Update");
            update = true;
        }
	}
    private void LateUpdate()
    {
        if (lateupdate == false)
        {
            Debug.Log("LateUpdate");
            lateupdate = true;
        }
    }
    public void Init()
    {
        Debug.Log("Init");
    }
}
