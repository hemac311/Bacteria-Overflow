using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Trait {
    TraitsEnum type;
    int value;
    //priority
    //value range -> NEEDS TO BE BASED ON TYPE!!!


    
    public Trait(TraitsEnum type, int value) {
        this.type = type;
        this.value = value;
    }
    public void SetValue(int value) {
        this.value = value;
    }

    public void SetType(TraitsEnum type) {
        this.type = type;
    }

    public TraitsEnum GetEnumType() {
        return this.type;
    }

    public int GetValue() {
        return this.value;
    }

    public String Prettyfy() {
        return "Type: " + type.ToString() + " Value: " + value.ToString();
    }

    
}