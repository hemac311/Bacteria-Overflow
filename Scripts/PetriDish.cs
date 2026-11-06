using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriDish : MonoBehaviour
{
    Trait phValue;
    Trait ground;
    public GameObject table;
    public GameObject[,] bacteria;

    public Trait Ground { get => ground; set => ground = value; }
    public Trait PhValue { get => phValue; set => phValue = value; }
    public GameObject Table { get => table; set => table = value; }

    // Start is called before the first frame update
    private void Awake()
    {
        System.Random rand = new System.Random();
        PhValue = new Trait(TraitsEnum.PhValue, rand.Next(0, 7));
        Ground = new Trait(TraitsEnum.Ground, 1);
        bacteria = new GameObject[10, 10];
    }

}
