using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.script;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;

public class GamePlayerController : MonoBehaviour
{
    // game over constants.

    const float GAME_OVER_TIMER_INTERVAL = 4f;

    // master.

    public GameMasterController master;
    private GameUserInterfaceController ui;

    // game over variables.

    float game_over_timer = 0f;

    // player variables.

    [NonSerialized] public int health = 3;
    [NonSerialized] public int maxHealth = 3;

    [NonSerialized] public int ammo = 0;
    [NonSerialized] public int maxAmmo = 0;

    [NonSerialized] public bool canAttack = false;
    [NonSerialized] public bool canCrouchJump = false;
    [NonSerialized] public bool canDive = false;
    [NonSerialized] public bool canWaterDive = false;
    [NonSerialized] public bool canWaterJump = false;

    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();
    }

    void Update()
    {
        if (master.gameState == GameState.Game || master.gameState == GameState.GameCutscene)
        {
            // game over.

            if (health <= 0 && master.gameState != GameState.GameOver)
            {
                master.ChangeState(GameState.GameOver);
                game_over_timer = 0f;
            }
        }
        else if(master.gameState == GameState.GameOver)
        {
            game_over_timer += Time.deltaTime;

            if(game_over_timer >= GAME_OVER_TIMER_INTERVAL)
            {
                // TODO USE LOAD LEVEL CONTROLLER.
                foreach (GameObject o in GameObject.FindObjectsOfType<GameObject>())
                    Destroy(o);
                SceneManager.LoadScene("scene_title");
            }
        }
    }
}
