using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Sagittal2 : MonoBehaviour
{
    public SteamVR_Action_Boolean moveSag1;
    public SteamVR_Input_Sources handType;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        moveSag1.AddOnStateDownListener(TriggerDown, handType);
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        player.transform.position = new Vector3(150, 0, 0);
        player.transform.rotation = transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
