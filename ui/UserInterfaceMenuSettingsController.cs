using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.script.GameConstants;

public class UserInterfaceMenuSettingsController : MonoBehaviour
{
    private GameObject uiApplyButtonObject;
    private Button uiApplyButton;
    private GameObject uiCancelButtonObject;
    private Button uiCancelButton;

    private GameObject uiVolumeMasterSliderObject;
    private Slider uiVolumeMasterSlider;
    private GameObject uiVolumePlayerSliderObject;
    private Slider uiVolumePlayerSlider;
    private GameObject uiVolumeMobSliderObject;
    private Slider uiVolumeMobSlider;
    private GameObject uiVolumePropSliderObject;
    private Slider uiVolumePropSlider;
    private GameObject uiVolumeFootstepSliderObject;
    private Slider uiVolumeFootstepSlider;
    private GameObject uiVolumeVoxSliderObject;
    private Slider uiVolumeVoxSlider;
    private GameObject uiVolumeMusicSliderObject;
    private Slider uiVolumeMusicSlider;
    private GameObject uiVolumeAmbienceSliderObject;
    private Slider uiVolumeAmbienceSlider;

    void Start()
    {
        // get buttons.

        uiApplyButtonObject = GameObject.Find("ui_button_apply");
        uiApplyButton = uiApplyButtonObject.GetComponent<Button>();
        uiApplyButton.onClick.AddListener(OnApplyButtonClick);

        uiCancelButtonObject = GameObject.Find("ui_button_cancel");
        uiCancelButton = uiCancelButtonObject.GetComponent<Button>();
        uiCancelButton.onClick.AddListener(OnCancelButtonClick);

        // get volume sliders.

        uiVolumeMasterSliderObject = GameObject.Find("ui_slider_volume_master");
        uiVolumeMasterSlider = uiVolumeMasterSliderObject.GetComponent<Slider>();
        uiVolumePlayerSliderObject = GameObject.Find("ui_slider_volume_player");
        uiVolumePlayerSlider = uiVolumePlayerSliderObject.GetComponent<Slider>();
        uiVolumeMobSliderObject = GameObject.Find("ui_slider_volume_mob");
        uiVolumeMobSlider = uiVolumeMobSliderObject.GetComponent<Slider>();
        uiVolumePropSliderObject = GameObject.Find("ui_slider_volume_prop");
        uiVolumePropSlider = uiVolumePropSliderObject.GetComponent<Slider>();
        uiVolumeFootstepSliderObject = GameObject.Find("ui_slider_volume_footstep");
        uiVolumeFootstepSlider = uiVolumeFootstepSliderObject.GetComponent<Slider>();
        uiVolumeVoxSliderObject = GameObject.Find("ui_slider_volume_vox");
        uiVolumeVoxSlider = uiVolumeVoxSliderObject.GetComponent<Slider>();
        uiVolumeMusicSliderObject = GameObject.Find("ui_slider_volume_music");
        uiVolumeMusicSlider = uiVolumeMusicSliderObject.GetComponent<Slider>();
        uiVolumeAmbienceSliderObject = GameObject.Find("ui_slider_volume_ambience");
        uiVolumeAmbienceSlider = uiVolumeAmbienceSliderObject.GetComponent<Slider>();

        // intialise volume sliders from settings.

        uiVolumeMasterSlider.value = GameSettingsController.Global.volumeMaster;
        uiVolumePlayerSlider.value = GameSettingsController.Global.volumePlayer;
        uiVolumeMobSlider.value = GameSettingsController.Global.volumeMob;
        uiVolumePropSlider.value = GameSettingsController.Global.volumeProp;
        uiVolumeFootstepSlider.value = GameSettingsController.Global.volumeFootstep;
        uiVolumeVoxSlider.value = GameSettingsController.Global.volumeVox;
        uiVolumeMusicSlider.value = GameSettingsController.Global.volumeMusic;
        uiVolumeAmbienceSlider.value = GameSettingsController.Global.volumeAmbience;

        // initialise.

        var ev = EventSystem.current;
        ev.SetSelectedGameObject(uiApplyButtonObject);
        uiApplyButton.Select();
    }

    public void OnApplyButtonClick()
    {
        // save volume sliders.

        GameSettingsController.Global.volumeMaster = uiVolumeMasterSlider.value;
        GameSettingsController.Global.volumePlayer = uiVolumePlayerSlider.value;
        GameSettingsController.Global.volumeMob = uiVolumeMobSlider.value;
        GameSettingsController.Global.volumeProp = uiVolumePropSlider.value;
        GameSettingsController.Global.volumeFootstep = uiVolumeFootstepSlider.value;
        GameSettingsController.Global.volumeVox = uiVolumeVoxSlider.value;
        GameSettingsController.Global.volumeMusic = uiVolumeMusicSlider.value;
        GameSettingsController.Global.volumeAmbience = uiVolumeAmbienceSlider.value;

        GameSettingsController.Global.SaveSettings();

        GameLoadSceneController.Global.StartLoadMenuScene("menu_main", GAME_STATE_MENU_MAIN);
    }

    public void OnCancelButtonClick()
    {
        GameLoadSceneController.Global.StartLoadMenuScene("menu_main", GAME_STATE_MENU_MAIN);
    }
}
