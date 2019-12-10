using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MotionDriver : MonoBehaviour
{
    public SteamVR_Action_Boolean moveSag1;
    public SteamVR_Action_Boolean moveSag2;
    public SteamVR_Action_Boolean moveCor1;
    public SteamVR_Action_Boolean moveCor2;
    public SteamVR_Action_Boolean moveUp;
    public SteamVR_Action_Boolean moveDown;
    public SteamVR_Action_Boolean moveForward;
    public SteamVR_Action_Boolean moveBackward;
    public SteamVR_Action_Boolean moveRight;
    public SteamVR_Action_Boolean moveLeft;
    public SteamVR_Action_Boolean displayText;
    public GameObject text;
    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;
    public Camera vrCamera;
    public float verticalVelocity;
    public float horizontalVelocity;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        moveSag1.AddOnStateDownListener(XDown, leftHand);
        moveSag2.AddOnStateDownListener(YDown, leftHand);
        moveDown.AddOnStateDownListener(LTrigDown, leftHand);
        moveCor1.AddOnStateDownListener(BDown, rightHand);
        moveCor2.AddOnStateDownListener(ADown, rightHand);
        moveUp.AddOnStateDownListener(RTrigDown, rightHand);
        moveForward.AddOnStateDownListener(JoystickNorth, rightHand);
        moveRight.AddOnStateDownListener(JoystickEast, rightHand);
        moveLeft.AddOnStateDownListener(JoystickWest, rightHand);
        moveBackward.AddOnStateDownListener(JoystickSouth, rightHand);
        displayText.AddOnStateDownListener(TriggerDown, rightHand);
    }

    public void XDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        player.transform.position = new Vector3(-150, 0, 0);
        player.transform.rotation = transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
    }

    public void YDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        player.transform.position = new Vector3(150, 0, 0);
        player.transform.rotation = transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
    }

    public void BDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        player.transform.position = new Vector3(0, 0, -150);
        player.transform.rotation = transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void ADown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        player.transform.position = new Vector3(0, 0, 150);
        player.transform.rotation = transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
    }

    public void RTrigDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Vector3 loc = player.transform.position;
        loc.y += verticalVelocity;
        player.transform.position = loc;
    }

    public void LTrigDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Vector3 loc = player.transform.position;
        loc.y -= verticalVelocity;
        player.transform.position = loc;
    }

    public void JoystickNorth(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        float y = player.transform.position.y;
        Vector3 loc = player.transform.position + (vrCamera.transform.forward * horizontalVelocity);
        Debug.Log(loc);
        loc.y = y;
        player.transform.position = loc;
    }

    public void JoystickSouth(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        float y = player.transform.position.y;
        Vector3 loc = player.transform.position + (vrCamera.transform.forward * -1 * horizontalVelocity);
        Debug.Log(loc);
        loc.y = y;
        player.transform.position = loc;
    }

    public void JoystickWest(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        float y = player.transform.position.y;
        Vector3 loc = player.transform.position + (vrCamera.transform.right * -1 * horizontalVelocity);
        Debug.Log(loc);
        loc.y = y;
        player.transform.position = loc;
    }

    public void JoystickEast(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        float y = player.transform.position.y;
        Vector3 loc = player.transform.position + (vrCamera.transform.right * horizontalVelocity);
        Debug.Log(loc);
        loc.y = y;
        player.transform.position = loc;
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        text.SetActive(!text.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

