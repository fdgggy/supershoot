using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testevent : MonoBehaviour {

    vp_PlayerEventHandler eventHandler = null;
    // Use this for initialization
    void Awake () {
        eventHandler = this.gameObject.GetComponent<vp_PlayerEventHandler>();

    }
    private void Start()
    {
        testexecute ex = this.gameObject.AddComponent<testexecute>();
        ex.Init();
    }
    private void OnEnable()
    {
        //eventHandler.Register(this);

        //eventHandler.CurrentWeaponType.Set(2);
    }

    //public int OnValue_CurrentWeaponType
    //{
    //    get; set;
    //}

    // Update is called once per frame
    void Update () {
        //int gg = eventHandler.CurrentWeaponType.Get();
        //eventHandler.CurrentWeaponType.Set(2);
        //Debug.Log("gg=" + gg);
    }
    private void LateUpdate()
    {
        Debug.Log("xxxxxxxxxxxx LateUpdate");
    }
}
