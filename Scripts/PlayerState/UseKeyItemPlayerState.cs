using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Constants;
using static PlayerConstants;

public class UseKeyItemPlayerState : MonoBehaviour, IState<Player, PlayerStateId>
{
    // Consts.
    private static readonly Vector3 CARD_START_OFFSET = new Vector3(0.0F, 0.0F, 0.0F);
    private static readonly Vector3 CARD_FINISH_OFFSET = new Vector3(0.0F, 0.4F, 0.0F);
    private static readonly Vector3 CARD_START_SCALE = new Vector3(0.0F, 0.0F, 0.0F);
    private static readonly Vector3 CARD_FINISH_SCALE = new Vector3(0.25F, 0.25F, 0.25F);

    // Public properties.
    public PlayerStateId StateId => PlayerStateId.UseKeyItem;

    public void BeginState(Player c, Dictionary<string, object> args = null)
    {
        c.playerAnimator.ResetAllAnimatorTriggers();
        c.playerAnimator.SetTrigger(ANIMATION_TRIGGER_EMOTE_REACT_KEY_ITEM);

        c.playerRigidBody.velocity = Vector3.zero;
        c.playerRigidBody.AddForce(Vector3.up * ATTACK_UP_FORCE_MULT, ForceMode.VelocityChange);

        string keyItemId = PlayerHighLogic.G.SelectedKeyItemId;

        if(keyItemId == null)
        {
            c.useKeyItemCardSpriteRenderer.sprite = null;
        }
        else
        {
            string iconId = $"KeyItem{keyItemId}Icon";
            string icon = TextsHighLogic.G.GetText(iconId) ?? keyItemId;

            var sprite = AssetsHighLogic.G.KeyItemSprites.FirstOrDefault(x => x.name == icon);
            c.useKeyItemCardSpriteRenderer.sprite = sprite;
        }

        c.useKeyItemCardObject.SetActive(true);

        c.useKeyItemAudioSource.PlayPitchedOneShot
            (c.useKeyItemAudioSource.clip
            , SettingsHighLogic.G.PlayerVolume
            , SFX_MIN_PT
            , SFX_MAX_PT);
    }

    public void FixedUpdateState(Player c)
    {
        
    }

    public void UpdateState(Player c)
    {
        if(c.StateTimer > USE_KEY_ITEM_INTERVAL)
        {
            c.ChangeState(PlayerStateId.Default);
            return;
        }

        // Card.

        float moveProgress = Mathf.InverseLerp(0, USE_KEY_ITEM_CARD_INTERVAL, c.StateTimer);
        float moveLerp = Mathf.SmoothStep(0.0F, 1.0F, moveProgress);

        c.useKeyItemCardObject.transform.position = Vector3.Lerp
            ( c.transform.position + CARD_START_OFFSET
            , c.transform.position + CARD_FINISH_OFFSET
            , moveLerp);

        c.useKeyItemCardObject.transform.localScale = Vector3.Lerp
            ( CARD_START_SCALE
            , CARD_FINISH_SCALE
            , moveLerp);

        // Renderer.

        if (c.KeyItemUse.IsKeyItemUsableInRange)
        {
            Vector3 directionToKeyItemUsable
            = (c.KeyItemUse.KeyItemUsableInRange.KeyItemUsableGameObject.transform.position - transform.position).normalized;
            directionToKeyItemUsable.y = 0.0F;

            PlayerStatics.UpdateInternalDirection(ActiveSceneHighLogic.G.CachedPlayer, directionToKeyItemUsable);
            PlayerStatics.UpdateRendererDirection(ActiveSceneHighLogic.G.CachedPlayer, directionToKeyItemUsable);
        }
    }

    public void EndState(Player c)
    {
        c.useKeyItemCardObject.SetActive(false);

        // If state ran to full interval, use
        // the key item.
        if(c.StateTimer >= USE_KEY_ITEM_INTERVAL 
            && c.KeyItemUse.IsKeyItemUsableInRange
            && PlayerHighLogic.G.SelectedKeyItemId != null)
        {
            c.KeyItemUse.KeyItemUsableInRange.OnKeyItemUse(PlayerHighLogic.G.SelectedKeyItemId);
        }
    }
}
