using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Bacterium : MonoBehaviour
{
    Trait[] traits;
    ArrayList allOnGrid; //All the same cells on the playing field
    ArrayList neighbors; //All direct neighbors around this cell
    Trait[] environment; //All Traits in the environment of this cell

    [SerializeField]
    int hitpoints;
    public int posX; //array pos not absolute on grid       // Wird von Petri dish gesetzt
    public int posY; //array pos not absolute on grid

    [SerializeField]
    private string hash;
    public Color color;

    Trait radiation; //0
    Trait vibration;
    Trait phValue;
    Trait ground;
    Trait origin;
    Trait mutationFactor;
    Trait aggression;
    Trait max_growth;
    Trait tenacity;
    Trait cleaniness;
    Trait temperature;
    Trait humidity;
    Trait lightintensity;
    Trait sound;
    Trait willingnessToMate; //14

    public GameObject petriDish;
    public ArrayList extensions;
    public ArrayList tookOver;

    // for mouse over
    [SerializeField]
    Text textField;
    bool mouseTrigger = false;

    GameObject RoomgameObject;


    private void Awake()
    {
        RoomgameObject = GameObject.Find("The Room");
        textField = GameObject.Find("Bacteria Explanation").GetComponent<Text>();
        this.hitpoints = 100;
        int[] traits_randoms = new int[15];
        for (int i = 0; i < traits_randoms.Length; i++)
        {
            System.Random rand = new System.Random();
            traits_randoms[i] = rand.Next(0, 7);
        }

        this.radiation = new Trait(TraitsEnum.Radiation, traits_randoms[0]);
        this.vibration = new Trait(TraitsEnum.Vibration, traits_randoms[1]);
        this.phValue = new Trait(TraitsEnum.PhValue, traits_randoms[2]);
        this.ground = new Trait(TraitsEnum.Ground, traits_randoms[3]);
        this.origin = new Trait(TraitsEnum.Origin, traits_randoms[4]);
        this.mutationFactor = new Trait(TraitsEnum.MutationFactor, traits_randoms[5]);
        this.aggression = new Trait(TraitsEnum.Aggression, traits_randoms[6]);
        this.max_growth = new Trait(TraitsEnum.Max_growth, traits_randoms[7]);
        this.tenacity = new Trait(TraitsEnum.Tenacity, traits_randoms[8]);
        this.cleaniness = new Trait(TraitsEnum.Cleaniness, traits_randoms[9]);
        this.temperature = new Trait(TraitsEnum.Temperature, traits_randoms[10]);
        this.humidity = new Trait(TraitsEnum.Humidity, traits_randoms[11]);
        this.lightintensity = new Trait(TraitsEnum.Lightintensity, traits_randoms[12]);
        this.sound = new Trait(TraitsEnum.Sound, traits_randoms[13]);
        this.willingnessToMate = new Trait(TraitsEnum.WillingnessToMate, traits_randoms[14]);

        Trait[] traits = new Trait[15];

        //petrishDish
        traits[0] = phValue;
        traits[1] = ground;

        //table
        traits[2] = cleaniness;
        traits[3] = vibration;
        traits[4] = lightintensity;

        //room
        traits[5] = radiation;
        traits[6] = sound;
        traits[7] = temperature;
        traits[8] = humidity;

        //bacteria
        traits[9] = origin;
        traits[10] = mutationFactor;
        traits[11] = aggression;
        traits[12] = max_growth;
        traits[13] = tenacity;
        traits[14] = willingnessToMate;

        this.traits = traits;

        this.neighbors = new ArrayList();
        this.allOnGrid = new ArrayList();
        this.environment = new Trait[9];
        this.hash = GenerateHash(traits);

        //can this break? 
        PetriDish dish = this.petriDish.GetComponent<PetriDish>();
        environment[0] = dish.PhValue;
        environment[1] = dish.Ground;

        Table table = dish.table.GetComponent<Table>();
        environment[2] = table.LightIntensity;
        environment[3] = table.Vibration;
        environment[4] = table.LightIntensity;

        Room room = table.room.GetComponent<Room>();

        environment[5] = room.Radiation;
        environment[6] = room.Sound;
        environment[7] = RoomgameObject.GetComponent<Room>().Temperature;
        Debug.Log(RoomgameObject.GetComponent<Room>().Temperature.GetValue());
        environment[8] = RoomgameObject.GetComponent<Room>().Humidity;

        extensions = new ArrayList();
        tookOver = new ArrayList();

    }

    public void ChangeColor(GameObject otherBac)
    {
        color = ColorizeBacteria.PicColor(otherBac.GetComponent<Bacterium>()) / 255; // divided by 255 cause unity uses rgb 0 - 1.0
        color.r = 1 - color.r;      // Werte werden invertiert um mehr hellere Werte zu erreichen
        color.b = 1 - color.b;
        color.g = 1 - color.g;
        color.a = hitpoints * 1.0f / 100;
        // print(color);
        this.GetComponent<SpriteRenderer>().color = color;
    }

    private String GenerateHash(Trait[] traits)
    {
        String hash = "";
        for (int i = 0; i < traits.Length; i++)
        {
            hash += traits[i].GetValue().ToString();
        }
        return hash;
    }

    public void RandomMutation()
    {
        //first calculate mutation factor
        System.Random rand = new System.Random();
        if (mutationFactor.GetValue() == 0)
        {
            int chance = rand.Next(1, 100);
            if (chance == 1) {
                FullRandomMutation();
            }
        }
        else
        {
            int chance = rand.Next(0, 14);
            if (chance == mutationFactor.GetValue()) FullRandomMutation();
            this.hash = GenerateHash(this.traits);
            Console.WriteLine(hash);
        }
    }

    public void ActivateDiamond()
    {
        if (!mouseTrigger)
        {
            mouseTrigger = true;
            this.transform.localScale = new Vector3(0.7f, 0.7f);
            textField.GetComponent<Text>().text = "#159X" + this.hash.Substring(7,2) + "X489#";
            textField.gameObject.SetActive(true);
        }
    }

    public void DeactivateDiamond()
    {
        mouseTrigger = false;
        this.transform.localScale = new Vector3(0.5f, 0.5f);
        textField.gameObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        foreach(GameObject bac in petriDish.GetComponent<PetriDish>().bacteria)
        {
            if (this.hash == bac.GetComponent<Bacterium>().hash)
                bac.GetComponent<Bacterium>().ActivateDiamond();      
        }
    }

    private void OnMouseExit()
    {
        foreach (GameObject bac in petriDish.GetComponent<PetriDish>().bacteria)
        {
            if (this.hash == bac.GetComponent<Bacterium>().hash)
                bac.GetComponent<Bacterium>().DeactivateDiamond();
        }
    }

    /// <summary>
    /// Literally renews all Traits randomly.
    /// </summary>
    private void FullRandomMutation()
    {
        System.Random rand = new System.Random();
        for (int i = 0; i < this.traits.Length; i++)
        {
            traits[i].SetValue(rand.Next(0, 7));
        }
    }

    public void CalculateSurvival()
    {
        int max_diff =  14; //current standardized trait lenght -> theoretically this needs to be calculated.
        int survivorscore = CalculateSurvivalHumidityTemperature();


        System.Random rand = new System.Random();
        //we just take half of the max diff as surviving score this is something we can play around with and create different formulas for.
            
            this.hitpoints -= survivorscore * 5;
        
        /*
        else
        {
            if (this.hitpoints < 100) hitpoints += survivorscore / 100;
            if (this.hitpoints > 100) hitpoints = 100;
        }*/

        if (hitpoints <= 0) {
            this.gameObject.SetActive(false);
        }
    
    }

    /// <summary>
    /// Adds differences beetween preferences and environment. These will be the survival score. A high value is bad.
    /// </summary>
    /// <returns>The Score as int</returns>
    private int CalculateSurvivalScore()
    {
        int survivorscore = 0;
        for (int i = 0; i < environment.Length; i++)
        {
            //this is dangerous as the environment and traits array need to be organized and ordered in the same manner
            //but it enables us to do an easy walkthrough and comparision of traits.
            Trait envTrait = environment[i];
            Trait bacTrait = traits[i];         // seems like the class Trait needs to have the Serializable clarifyer for this, dont know      
            int diff = Math.Abs(envTrait.GetValue() - bacTrait.GetValue());
            survivorscore += diff;
        }

        return survivorscore;
    }


    private int CalculateSurvivalHumidityTemperature() {
        int diff = 0;

        diff += Math.Abs(traits[7].GetValue() -environment[7].GetValue());
        diff += Math.Abs(traits[8].GetValue() -environment[8].GetValue());
        return diff;
    }

    /// <summary>
    /// Calculates how good a bacteria fells in its environment.
    /// </summary>
    /// <returns></returns>
    private int CalculateFeelGoodScore() {
        int survivorScore = CalculateSurvivalScore();
        return 150 - survivorScore;
    }

    /// <summary>
    /// This sets a lit of bacteria that are of the same hash.
    /// </summary>
    /// <param name="sameBacteriaOnGrid">A list of all the bacteria on the grid. </param>
    public void SetAllOnGrid(ArrayList sameBacteriaOnGrid)
    {
        this.allOnGrid = sameBacteriaOnGrid;
    }

    public Trait[] GetTraitsArray()
    {
        return this.traits;
    }

    public ArrayList GetSameBacteriaOnGrid()
    {
        return this.allOnGrid;
    }

    public ArrayList GetNeighborArray()
    {
        return this.neighbors;
    }

    public String GetHash()
    {
        return this.hash;
    }

    /// <summary>
    /// 
    /// </summary>
    public void CalculateNeigbors()
    {
        neighbors.Clear(); //in case of changes in the neighborhood clear neighbors or when we call this twice.
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                try
                {
                    if (i != 0 && j != 0)
                    { //we dont want the bacteria as its own neighbor
                        neighbors.Add(petriDish.GetComponent<PetriDish>().bacteria[posX + i, posY + j]);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// This function does Attemps an Extentsion on their neighboring grid places for this specific cell unless hp < 0.
    /// This can either be nothing if the gridplace is of the same cell,
    /// </summary>
    public void RandomExtension()
    { 
        if (this.hitpoints < 0)
        {
            //dead cells cannot do anything
            return;
        }
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                try
                {
                    Bacterium neighbor = petriDish.GetComponent<PetriDish>().bacteria[posX + i, posY + j].GetComponent<Bacterium>();
                    if (!neighbor.isActiveAndEnabled)
                    {
                        TryToExtendToTile(posX + i, posY + j);

                    }
                    else if (neighbor.hash == this.hash)
                    {
                        continue;
                    }
                    else if (neighbor.hash != this.hash)
                    {
                        AttemptHostileTakeOver(neighbor);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }
        }
    }


    /// <summary>
    /// Copys all traits from one bacterium into another and changes its hash.
    /// </summary>
    /// <param name="copy">the bacterium that gets all the traits.</param>
    public void CopyAllTraits(Bacterium copy) {
        for (int i = 0; i < this.GetTraitsArray().Length; i++) {
            copy.GetTraitsArray()[i].SetValue(this.GetTraitsArray()[i].GetValue());
        }
        copy.hash = GenerateHash(copy.GetTraitsArray());
    }

    /// <summary>
    /// Copies all traits fo this into a bacterium but sets the hitpoints to 100
    /// </summary>
    /// <param name="copy">the bacterium that gets this values</param>
    public void DeepCopyBacteria100HP(Bacterium copy)
    {
        for (int i = 0; i < this.GetTraitsArray().Length; i++)
        {
            copy.GetTraitsArray()[i].SetValue(this.GetTraitsArray()[i].GetValue());
        }
        copy.hitpoints = 100;
        copy.hash = GenerateHash(copy.GetTraitsArray());
    }


    /// <summary>
    /// This method is used to grow into a neighboring tile, when its empty.
    /// </summary>
    /// <param name="x">the x coordinate of the tile the bacterium wants to capture</param>
    /// <param name="y">the y coordinate of the tile the bacterium wants to capture</param>
    private void TryToExtendToTile(int x, int y)
    {
        System.Random rand = new System.Random();
        int feelGoodScore = this.CalculateSurvivalHumidityTemperature();
        Bacterium neighbor = petriDish.GetComponent<PetriDish>().bacteria[x, y].GetComponent<Bacterium>();
        if (feelGoodScore < 4)
        {
            if (this.hitpoints > 50) // && this.max_growth.GetValue() * 7 + 20 > rand.Next(0, 100)
            {
                GameObject otherBacTemp = petriDish.GetComponent<PetriDish>().bacteria[x, y];
                this.DeepCopyBacteria100HP(neighbor);

                //we should bias this better.
                if (neighbor.mutationFactor.GetValue() * 2 < rand.Next(0, 100)) //1% chance.
                {
                    neighbor.RandomMutation();
                }
            }
            extensions.Add(neighbor);
  
        }
    }

    /// <summary>
    /// Defines how this.bacteria is attacking a neighbor in trying to spread.
    /// </summary>
    /// <param name="neighbor">The neighboring cell we try to take over</param>
    private void AttemptHostileTakeOver(Bacterium neighbor)
    {
        System.Random rand = new System.Random();
        if (this.willingnessToMate.GetValue() > 5 && neighbor.willingnessToMate.GetValue() > 5)
        {

            if (this.willingnessToMate.GetValue() + neighbor.willingnessToMate.GetValue() > rand.Next(0, 14))
            {
                Mating(neighbor);
                return;
            }
            //we could add some stuff in case of mating failure
        }
        else
        {
            int pointsNeighbor = 0;
            int pointsThis = 0;
            int thisSurvivorScore = this.CalculateSurvivalScore();
            int neighborSurvivorScore = neighbor.CalculateSurvivalScore();
            int survivorScoreDiff = Math.Abs(thisSurvivorScore / 15 - neighborSurvivorScore / 15);
            int points = rand.Next(0, survivorScoreDiff);
            if (thisSurvivorScore > neighborSurvivorScore)
            {

                pointsThis += points;
            }
            else
            {
                pointsNeighbor += points;
            }

            if (this.aggression.GetValue() > neighbor.tenacity.GetValue())
            {
                int aggroDiff = Math.Abs(this.aggression.GetValue() - neighbor.tenacity.GetValue());
                pointsThis += rand.Next(0, aggroDiff);
            }
            else
            {
                int aggroDiff = Math.Abs(this.aggression.GetValue() - neighbor.tenacity.GetValue());
                pointsNeighbor += rand.Next(0, aggroDiff);
            }

            int hpDiff = Math.Abs(this.hitpoints / 10 - neighbor.hitpoints / 10);
            int hpPoints = rand.Next(0, hpDiff);
            if (this.hitpoints > neighbor.hitpoints)
            {
                pointsThis += hpPoints;
            }
            else
            {
                pointsNeighbor += hpPoints;
            }

            if (pointsThis >= pointsNeighbor)
            {
                this.CopyAllTraits(neighbor);
                neighbor.gameObject.SetActive(false);
                tookOver.Add(neighbor);
            }
            else 
            {
                this.hitpoints -= rand.Next(0, neighbor.aggression.GetValue());
            }

            if (this.hitpoints <= 0)
            {
                this.gameObject.SetActive(false);
            }
    

        }
        
    }

    /// <summary>
    /// Defines the mating ritual of two bacteria.
    /// </summary>
    /// <param name="neighbor">The neighboring bacterium this.bacterium is mating with</param>
    private void Mating(Bacterium neighbor)
    {
        Bacterium dominantPartner;
        Bacterium submissivePartner;

        if (this.aggression.GetValue() > neighbor.aggression.GetValue())
        {
            dominantPartner = this;
            submissivePartner = neighbor;
        }
        else
        {
            dominantPartner = neighbor;
            submissivePartner = this;
        }

        System.Random rand = new System.Random();
        //Bacterium child1 = Bacterium.Instantiate(dominantPartner);
        //Bacterium child2 = Bacterium.Instantiate(submissivePartner);
        Trait[] tempDominantTraits = new Trait[dominantPartner.traits.Length];

        for (int i = 0; i < dominantPartner.GetTraitsArray().Length; i++) {

            tempDominantTraits[i] = dominantPartner.traits[i];

            if (dominantPartner.aggression.GetValue() >= rand.Next(submissivePartner.aggression.GetValue(), 7) ) {
                //child1.GetTraitsArray()[i].SetValue(dominantPartner.GetTraitsArray()[i].GetValue());
                submissivePartner.GetTraitsArray()[i].SetValue(dominantPartner.GetTraitsArray()[i].GetValue());
            } else {
                dominantPartner.GetTraitsArray()[i].SetValue(submissivePartner.GetTraitsArray()[i].GetValue());
            }
        }

        //is this changing object references??

        //mating / mutation


    }


}
