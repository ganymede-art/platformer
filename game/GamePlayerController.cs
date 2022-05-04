using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System;
using static Assets.Script.GameConstants;

public class GamePlayerController : MonoBehaviour
{
    private static GamePlayerController global;
    public static GamePlayerController Global
    {
        get
        {
            if (global == null)
            {
                global = GameMasterController.Global.playerController;
            }
            return global;
        }
    }

    // game over constants.

    const float GAME_OVER_TIMER_INTERVAL = 4F;

    // master.

    public GameMasterController master;
    private GameUserInterfaceController ui;

    // game over variables.

    float game_over_timer = 0F;

    // player variables.

    [NonSerialized] public int health = 6;
    [NonSerialized] public int maxHealth = 6;

    [NonSerialized] public int ammo = 10;
    [NonSerialized] public int maxAmmo = 10;

    [NonSerialized] public int money = 0;
    [NonSerialized] public int maxMoney = 99;

    [NonSerialized] public int oxygen = 10;
    [NonSerialized] public int maxOxygen = 10;

    [NonSerialized] public bool canAttack = false;
    [NonSerialized] public bool canHighJump = false;
    [NonSerialized] public bool canDive = false;
    [NonSerialized] public bool canSwim = false;
    [NonSerialized] public bool canWaterJump = false;
    [NonSerialized] public bool canFlutter = false;
    [NonSerialized] public bool canFireProjectile = false;
    [NonSerialized] public bool canSlam = false;

    void Start()
    {
        master = this.GetComponentInParent<GameMasterController>();
    }

    void Update()
    {

        if (master.gameState == GAME_STATE_GAME_OVER)
        {
            game_over_timer += Time.deltaTime;

            if(game_over_timer >= GAME_OVER_TIMER_INTERVAL)
            {
                // TODO USE LOAD LEVEL CONTROLLER.
                foreach (GameObject o in GameObject.FindObjectsOfType<GameObject>())
                    Destroy(o);
                SceneManager.LoadScene("menu_startup");
            }
        }
    }

    public void ModifyPlayerHealth(int playerHealthChange)
    {
        health += playerHealthChange;

        if (health > maxHealth)
            health = maxHealth;

        if(health <= 0)
        {
            SetGameOver();
        }
    }

    public void ModifyPlayerAmmo(int playerAmmoChange)
    {
        ammo += playerAmmoChange;

        if (ammo > maxAmmo)
            ammo = maxAmmo;

        if (ammo <= 0)
            ammo = 0;
    }

    public void ModifyPlayerMoney(int playerMoneyChange)
    {
        money += playerMoneyChange;

        if (money > maxMoney)
            money = maxMoney;

        if (money <= 0)
            money = 0;
    }

    public void ModifyPlayerOxygen(int playerOxygenChange)
    {
        oxygen += playerOxygenChange;

        if (oxygen > maxOxygen)
            oxygen = maxOxygen;

        if (oxygen <= 0)
        {
            oxygen = 0;
            GameMasterController.GlobalPlayerController.behaviourDamage.SimpleDamage(1);
        }
    }

    public void SetGameOver()
    {
        health = 0;
        GameMasterController.GlobalPlayerController.ChangePlayerState(PLAYER_STATE_DIE);
        GameMasterController.Global.ChangeState(GAME_STATE_GAME_OVER);
        game_over_timer = 0F;
    }
    
}
