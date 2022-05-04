using Assets.Script;
using Assets.Script.ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceTransitionController : MonoBehaviour
{
    const float MASK_WIDTH_MAX = 1000;
    const float MASK_WIDTH_MIN = 0;
    const float DEFAULT_TRANSITION_INTERVAL = 1.0F;

    // root.

    GameObject uiObject;
    GameObject uiOverlayObject;
    RectTransform uiOverlayRect;
    Image uiOverlayImage;
    Color uiOverlayColour;

    GameObject uiMaskObject;
    RectTransform uiMaskRect;

    GameObject uiBackgroundObject;
    Image uiBackgroundImage;

    bool isActive;

    float transitionTimer = 0.0F;
    float transitionInterval = 1.0F;
    float transitionPercentage = 0.0F;

    float maskWidth = 0.0F;

    private AudioSource audioSource;
    private AudioClip transitionSound;

    // public fields.

    public Sprite defaultOverlaySprite;
    public Color defaultOverlayColour;
    public AudioClip defaultTransitionSound; 

    void Start()
    {
        uiObject = this.gameObject;

        uiOverlayObject = uiObject.transform.Find("ui_overlay").gameObject;
        uiOverlayImage = uiOverlayObject.GetComponent<Image>();
        uiOverlayRect = uiOverlayObject.GetComponent<RectTransform>();

        uiMaskObject = uiOverlayObject.transform.Find("ui_mask").gameObject;
        uiMaskRect = uiMaskObject.GetComponent<RectTransform>();

        uiBackgroundObject = uiMaskObject.transform.Find("ui_background").gameObject;
        uiBackgroundImage = uiBackgroundObject.GetComponent<Image>();

        audioSource = gameObject.AddComponent<AudioSource>();

        uiObject.SetActive(false);
    }
    
    void Update()
    {
        if (!uiObject.activeSelf)
            return;

        if (isActive && transitionTimer < transitionInterval)
            transitionTimer += Time.deltaTime;

        if (!isActive && transitionTimer > 0)
            transitionTimer -= Time.deltaTime;

        transitionPercentage = Mathf.InverseLerp(0, transitionInterval, transitionTimer);

        maskWidth = Mathf.Lerp(MASK_WIDTH_MAX, MASK_WIDTH_MIN, transitionPercentage);

        uiOverlayRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maskWidth);
        uiOverlayRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maskWidth);

        uiMaskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maskWidth);
        uiMaskRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maskWidth);

        uiOverlayColour.a = transitionPercentage;
        uiOverlayImage.color = uiOverlayColour;
        uiBackgroundImage.color = uiOverlayColour;

        if (transitionTimer <= 0)
            uiObject.SetActive(false);
    }

    public void SetMenu(UserInterfaceTransitionData data)
    {
        uiObject.SetActive(true);

        if(data != null)
        {
            uiOverlayImage.sprite = data.overlaySprite ?? defaultOverlaySprite;
            uiOverlayColour = data.overlayColour;
            transitionInterval = data.transitionInterval;
            audioSource.clip = data.transitionSound ?? defaultTransitionSound;
        }
        else
        {
            uiOverlayImage.sprite = defaultOverlaySprite;
            uiOverlayColour = defaultOverlayColour;
            transitionInterval = DEFAULT_TRANSITION_INTERVAL;
            audioSource.clip = defaultTransitionSound;
        }

        isActive = true;
        transitionTimer = 0.0F;
        transitionPercentage = 0.0F;

        maskWidth = MASK_WIDTH_MAX;

        if (audioSource.clip != null)
            audioSource.Play();
    }

    public void UnsetMenu()
    {
        isActive = false;
    }
}
