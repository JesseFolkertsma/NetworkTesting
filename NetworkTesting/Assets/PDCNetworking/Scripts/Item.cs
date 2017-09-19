using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item{
    public string name;
    public int value;

    public Item(string _name, int _value){
        name = _name;
        value = _value;
    }
}
