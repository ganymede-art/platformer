using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets.Script.GameConstants;

public class GameStartupController : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("menu_main");
    }

    void Update()
    {
        
    }
}
