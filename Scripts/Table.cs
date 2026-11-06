using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{

    Trait lightIntensity;
    Trait vibration;
    Trait cleaniness;

    public Room room;

    public GameObject[] petriDishes;

    public Trait LightIntensity { get => lightIntensity; set => lightIntensity = value; }
    public Trait Vibration { get => vibration; set => vibration = value; }
    public Trait Cleaniness { get => cleaniness; set => cleaniness = value; }
    public Room Room { get => room; set => room = value; }
    public GameObject[] PetriDishes { get => petriDishes; set => petriDishes = value; }

    private void Awake()
    {

        LightIntensity = new Trait(TraitsEnum.Lightintensity,7);
        Vibration = new Trait(TraitsEnum.Vibration, 0);
        Cleaniness = new Trait(TraitsEnum.Cleaniness, 7);
    }

    private void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        petriDishes[0] = gameManager.GetComponent<GameManager>().petriDishPrefab;
    }

}
