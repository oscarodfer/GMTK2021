using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Controls
{

    private static bool spaceUp, spaceDown, spacePressed;

    private static float horizontal, vertical;
    private static bool scapeUp, scapeDown, scapePressed;

    public static bool SpaceUp { get => spaceUp; }
    public static bool SpaceDown { get => spaceDown; }
    public static bool SpacePressed { get => spacePressed; }


    public static float Horizontal { get => horizontal; }
    public static float Vertical { get => vertical; }
    public static bool EscapeUp { get => scapeUp; }
    public static bool EscapeDown { get => scapeDown; }
    public static bool EscapePressed { get => scapePressed; }

    public static void CaptureControls()
    {
        GetKeys();
        GetMouseButtons();
    }


    private static void GetKeys()
    {
        GetJump();
        GetAxis();
        GetAction();
        GetMenu();
    }



    private static void GetMouseButtons()
    {
        //Not for this project
    }


    private static void GetJump()
    {
        spaceDown = Input.GetKeyDown("space");
        spaceUp = Input.GetKeyUp("space");
        spacePressed = Input.GetKey("space");
    }

    private static void GetAxis()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private static void GetAction()
    {
        //Not for this project
    }

    private static void GetMenu()
    {
        scapeDown = Input.GetKeyDown(KeyCode.Escape);
        scapeUp = Input.GetKeyUp(KeyCode.Escape);
        scapePressed = Input.GetKey(KeyCode.Escape);
    }
}
