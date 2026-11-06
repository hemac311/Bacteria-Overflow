using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    // Replaced with icons
    // string[] textPrefix = {"Day: ", "Money: ", "Temperature: ", "Humidity: ", "Radiation: ", "Sound: ", ""};
    string[] textPostfix = {"", " €", " °C", " %", " Bq", " dB", ""};

    UiState state;

    [SerializeField]
    TMPro.TextMeshProUGUI[] uiTextFields;

    [SerializeField]
    GameObject gameplayUi;

    [SerializeField]
    GameObject menuUi;

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        state = UiState.MENU;
        gameplayUi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods

    /// <summary>
    /// Sets a custom text for the given TextField.
    /// </summary>
    public void SetCustomUiText(UiTextField textField, string text)
    {
        uiTextFields[(int)textField].text = text;
    }

    /// <summary>
    /// Sets the value for the given TextField with it's appropriate unit.
    /// </summary>
    public void SetValueUiText(UiTextField textField, string textValue)
    {
        var i = (int)textField;
        var text = textValue + textPostfix[i];
        SetCustomUiText(textField, text);
    }

    public void setStateToGameplay() {
        state = UiState.GAMEPLAY;
        menuUi.SetActive(false);
        gameplayUi.SetActive(true);
    }

    public void setStateToMenu() {
        state = UiState.MENU;
        menuUi.SetActive(true);
        gameplayUi.SetActive(false);
    }

    #endregion

}

public enum UiTextField
{
    [Description("Day: ")]
    DAY = 0,
    MONEY,
    TEMPERATURE,
    HUMIDITY,
    RADIATION,
    SOUND,
    MESSAGE
}

public enum UiState {
    MENU = 0,
    GAMEPLAY = 1,
}