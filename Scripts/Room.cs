using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    Trait radiation;
    Trait temperature;
    Trait humidity;
    Trait sound;

    [SerializeField]
    Table[] tables;

    public Trait Radiation { get => radiation; set => radiation = value; }
    public Trait Temperature { get => temperature; set => temperature = value; }
    public Trait Humidity { get => humidity; set => humidity = value; }
    public Trait Sound { get => sound; set => sound = value; }
    public Table[] Tables { get => tables; set => tables = value; }

    private void Awake()
    {
        System.Random rand = new System.Random();
        Radiation = new Trait(TraitsEnum.Radiation, rand.Next(0, 7));
        Temperature = new Trait(TraitsEnum.Temperature, rand.Next(0, 7));
        humidity = new Trait(TraitsEnum.Humidity, rand.Next(0,7));
        Sound = new Trait(TraitsEnum.Sound, rand.Next(0, 7));
    }
    // Start is called before the first frame update
    void Start()
    {
        System.Console.WriteLine(radiation.GetValue());
        System.Console.WriteLine(temperature.GetValue());
        System.Console.WriteLine(humidity.GetValue());
        System.Console.WriteLine(sound.GetValue());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetRadiation(int radiation)
    {
        this.radiation.SetValue(radiation);
    }

    public void SetTemperature(int temperature) {
        this.temperature.SetValue(temperature);
    }

    public void SetHumidity(int humidity) {
        this.humidity.SetValue(humidity);
    }

    public void SetSound(int sound) {
        this.sound.SetValue(sound);
    }
}
