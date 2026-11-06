using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //needs prefabs for Table, petriDish, 
    private static GameManager _instance;

    [SerializeField]
    GameObject bac;
    private Bacterium bacteriaScript;
    private PetriDish petriScript;
    [SerializeField]
    Sprite[] bacSprites;
    [SerializeField]
    public GameObject petriDishPrefab;
    [SerializeField]
    GameObject room;
    [SerializeField]
    GameObject table;

    [SerializeField]
    UiManager uiManager;

    private GameObject petriDish;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        bacteriaScript = bac.GetComponent<Bacterium>();
        petriScript = petriDishPrefab.GetComponent<PetriDish>();
    }

    void Start()
    {
        
        petriDish = Instantiate(petriDishPrefab, new Vector3(0f, 0f, -0.5f), Quaternion.identity, table.transform);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                // erstellt Bakterien auf einer Petrischale
                GameObject tempBacteria;
                tempBacteria = Instantiate(bac, petriDish.transform.position, Quaternion.identity, petriDish.transform);
                petriDish.GetComponent<PetriDish>().bacteria[i, j] = tempBacteria;      // Fills Array with Bacteria in Petridish
                tempBacteria.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
                Vector3 tempPos = new Vector3(petriDish.transform.position.x - 0.7f, petriDish.transform.position.y - 0.6f, 0.0f);
                tempBacteria.transform.localPosition = new Vector3(tempPos.x + i * 0.15f, tempPos.y + j * 0.15f, tempPos.z);
                Bacterium bacScript = tempBacteria.GetComponent<Bacterium>();
                bacScript.petriDish = petriDish;
                tempBacteria.GetComponent<SpriteRenderer>().sprite = bacSprites[Random.Range(0,4)];
                Quaternion roation = Quaternion.identity;
                roation.eulerAngles = new Vector3(0, 0, Random.Range(0,360));
                tempBacteria.transform.rotation = roation;
                bacScript.posX = i;
                bacScript.posY = j;
                tempBacteria.SetActive(false);
            }
        }

        GameStart();

        InitUi();
    }

    /// <summary>
    /// Goes through the survival, growth and Mutation of Cells.
    /// </summary>
    private void BacterialLoop() 
    {

        ArrayList extenders = new ArrayList();
        Room top_room = room.GetComponent<Room>();

        foreach (Table table in top_room.Tables)
        {
            GameObject[] tableChildren = new GameObject[table.transform.childCount];
            for (int i = 0; i < table.transform.childCount; i++)
            {
                tableChildren[i] = table.transform.GetChild(i).gameObject;
            }

            foreach (GameObject dishObject in tableChildren)
            {
                PetriDish dish = dishObject.GetComponent<PetriDish>();
                for (int i = 0; i < dish.bacteria.GetLength(0); i++)
                {
                    for (int j = 0; j < dish.bacteria.GetLength(1); j++)
                    {
                        Bacterium bacterium = dish.bacteria[i, j].GetComponent<Bacterium>();
                        if (bacterium.isActiveAndEnabled)
                        {
                           bacterium.CalculateSurvival();
                        }
                    }
                }

                for (int i = 0; i < dish.bacteria.GetLength(0); i++)
                {
                    for (int j = 0; j < dish.bacteria.GetLength(1); j++)
                    {
                        Bacterium bacterium = dish.bacteria[i, j].GetComponent<Bacterium>();
                        if (bacterium.isActiveAndEnabled)
                            bacterium.RandomExtension();
                    }
                }

                for (int i = 0; i < dish.bacteria.GetLength(0); i++)
                {
                    for (int j = 0; j < dish.bacteria.GetLength(1); j++)
                    {
                        Bacterium bacterium = dish.bacteria[i, j].GetComponent<Bacterium>();
                        if (bacterium.isActiveAndEnabled)
                            bacterium.RandomMutation();
                    }
                }

                for (int i = 0; i < dish.bacteria.GetLength(0); i++)
                {
                    for (int j = 0; j < dish.bacteria.GetLength(1); j++)
                    {
                        Bacterium bacterium = dish.bacteria[i, j].GetComponent<Bacterium>();
                        if (bacterium.isActiveAndEnabled)
                        {
                            foreach (Bacterium ext in bacterium.extensions) 
                            {
                                ext.gameObject.SetActive(true);
                            }

                            foreach (Bacterium takenOver in bacterium.tookOver)
                            {
                                takenOver.gameObject.SetActive(true);
                                takenOver.tookOver = new ArrayList();
                            }

                        }
                    }
                }


                for (int i = 0; i< dish.bacteria.GetLength(0); i++)
                {
                    for (int j = 0; j < dish.bacteria.GetLength(1); j++)
                    {
                        Bacterium bacterium = dish.bacteria[i, j].GetComponent<Bacterium>();
                        if (bacterium.isActiveAndEnabled)
                        {
                            bacterium.ChangeColor(dish.bacteria[i, j]);
                        }
                    }
                }

            }
        }
    }

    public void NextRoundEvent()
    {
        BacterialLoop();
        // TODO: Remove Log and Handle Click Event
        uiManager.SetCustomUiText(UiTextField.MESSAGE, "Next Round Button Clicked.");
    }

    /// <summary>
    /// User wants to buy an item from the Temperature Trait Category.
    /// </summary>
    public void BuyTemperatureItemClick()
    {
        // TODO: Remove Log and Handle Click Event
        uiManager.SetCustomUiText(UiTextField.MESSAGE, "Buy Temperature Item Clicked.");
    }

    /// <summary>
    /// User wants to buy an item from the Humidity Trait Category.
    /// </summary>
    public void BuyHumidityItemClick()
    {
        // TODO: Remove Log and Handle Click Event
        uiManager.SetCustomUiText(UiTextField.MESSAGE, "Buy Humidity Item Clicked.");
    }

    /// <summary>
    /// User wants to buy an item from the Radiation Trait Category.
    /// </summary>
    public void BuyRadiationItemClick()
    {
        // TODO: Remove Log and Handle Click Event
        uiManager.SetCustomUiText(UiTextField.MESSAGE, "Buy Radiation Item Clicked.");
    }

    /// <summary>
    /// User wants to buy an item from the Sound Trait Category.
    /// </summary>
    public void BuySoundItemClick()
    {
        // TODO: Remove Log and Handle Click Event
        uiManager.SetCustomUiText(UiTextField.MESSAGE, "Buy Sound Item Clicked.");
    }

    /// <summary>
    /// Starts the game with 3 Random Bacterias
    /// </summary>
    public void GameStart()
    {
        for (int i = 0; i < 3; i++)
        {
            int place = Random.Range(0, 100);
            int y = place / 10;
            int x = place % 10;
            GameObject temp = petriDish.GetComponent<PetriDish>().bacteria[x, y];
            temp.SetActive(true);
            temp.GetComponent<Bacterium>().ChangeColor(temp);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void InitUi()
    {
        var tRoom = room.GetComponent<Room>();

        uiManager.SetValueUiText(UiTextField.TEMPERATURE, tRoom.Temperature.GetValue().ToString());
        uiManager.SetValueUiText(UiTextField.HUMIDITY, tRoom.Humidity.GetValue().ToString());
        uiManager.SetValueUiText(UiTextField.RADIATION, tRoom.Radiation.GetValue().ToString());
        uiManager.SetValueUiText(UiTextField.SOUND, tRoom.Sound.GetValue().ToString());
        uiManager.SetValueUiText(UiTextField.MESSAGE, "");
    }

    public void IncreaseTemperature()
    {
        // This is ugly. Should be handled inside Trait with min/max values.
        var tRoom = room.GetComponent<Room>();
        var newTemperature = tRoom.Temperature.GetValue();
        if (7 > newTemperature)
        {
            ++newTemperature;
        }
        tRoom.Temperature.SetValue(newTemperature);
        uiManager.SetValueUiText(UiTextField.TEMPERATURE, newTemperature.ToString());
    }

    public void DecreaseTemperature()
    {
        // This is ugly. Should be handled inside Trait with min/max values.
        var tRoom = room.GetComponent<Room>();
        var newTemperature = tRoom.Temperature.GetValue();
        if (0 < newTemperature)
        {
            --newTemperature;
        }
        tRoom.Temperature.SetValue(newTemperature);
        uiManager.SetValueUiText(UiTextField.TEMPERATURE, newTemperature.ToString());
    }

    public void IncreaseHumidity()
    {
        // This is ugly. Should be handled inside Trait with min/max values.
        var tRoom = room.GetComponent<Room>();
        var newHumidity = tRoom.Humidity.GetValue();
        if (7 > newHumidity)
        {
            ++newHumidity;
        }
        tRoom.Humidity.SetValue(newHumidity);
        uiManager.SetValueUiText(UiTextField.HUMIDITY, newHumidity.ToString());
    }

    public void DecreaseHumidity()
    {
        // This is ugly. Should be handled inside Trait with min/max values.
        var tRoom = room.GetComponent<Room>();
        var newHumidity = tRoom.Humidity.GetValue();
        if (0 < newHumidity)
        {
            --newHumidity;
        }
        tRoom.Humidity.SetValue(newHumidity);
        uiManager.SetValueUiText(UiTextField.HUMIDITY, newHumidity.ToString());
    }
}