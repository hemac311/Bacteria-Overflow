using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorizeBacteria
{
    public static List<int> randomList = new List<int>();
    static int myRandomNumber = 0;
    private static int[] traitsNumbers = new int[6];
    public static Dictionary<string,Color> colorMap = new Dictionary<string, Color>();

    public static Color PicColor(Bacterium bacterium)
    {
        
        if (colorMap.ContainsKey(bacterium.GetHash()))
        {
            return colorMap[bacterium.GetHash()];
        }
        else
        {
            Color temp = PicColorhelper(bacterium);
            colorMap.Add(bacterium.GetHash(), temp);
            return temp;
        }
    }

    /// <summary>
    /// chooses a color for a given Bacterium based on 6 random traids 
    /// </summary>
    /// <returns></returns>
    public static Color PicColorhelper(Bacterium bacterium)
    {
        randomList.Clear(); // this is important so that for every new Start of the game there are new Random numbers
        for (int i = 0; i < 6; i++)
        {
            Trait[] tempArray = bacterium.GetTraitsArray();
            traitsNumbers[i] = tempArray[GetRandomNumber()].GetValue();
        }
       
        int r = 1 << traitsNumbers[0] | 1 << traitsNumbers[1];
        int g = 1 << traitsNumbers[2] | 1 << traitsNumbers[3];
        int b = 1 << traitsNumbers[4] | 1 << traitsNumbers[5];

        for (int i = 0; i<6; i++)               //this is so that colors dont get to bright
        {                                       //trust me, you dont need to think this trough, its stupid
            if (traitsNumbers[i] < 2)
            {
                if (i < 2)
                {
                    r = 1011;
                }else if (i < 4)
                {
                    g = 1011;
                }
                else
                {
                    b = 1011;
                }
            }
        }
        return new Color(r, g, b);
    }

    private static int GetRandomNumber()
    {
        myRandomNumber = Random.Range(0, 15);
        while (randomList.Contains(myRandomNumber))
        {
            myRandomNumber = Random.Range(0, 15);
        }
        randomList.Add(myRandomNumber);
        return myRandomNumber;
    }
}
