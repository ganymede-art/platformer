using Assets.script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTintController : MonoBehaviour
{
    bool isUnderwater = false;

    Texture2D underwaterTexture;
    Color underwaterColour = new Color(0.2f, 0.4f, 1.0f, 0.3f);

    Rect screen_rectangle = new Rect(0, 0, 4000, 4000);//Screen.width, Screen.height);

    private void Start()
    {
        underwaterTexture = new Texture2D(1, 1);
        underwaterTexture.SetPixel(0, 0, underwaterColour);
        underwaterTexture.Apply();
    }

    private void OnGUI()
    {
        if(isUnderwater)
            GUI.DrawTexture(screen_rectangle, underwaterTexture);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameConstants.TAG_WATER)
            isUnderwater = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == GameConstants.TAG_WATER)
            isUnderwater = false;
    }

}
