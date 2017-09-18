using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour {
    public static Inventory invManager;

    public List<Item> inventory = new List<Item>();
    public List<iLHelper> inventoryListings = new List<iLHelper>();

    public Transform content;

    public GameObject iL;

    private void Awake(){
        invManager = this;
    }
    private void Update(){
        if (Input.GetButtonDown("Jump")){
            Add(new Item("Hary", 10));
        }
    }
    public void Add(Item item){
        inventory.Add(item);
        iLHelper n;
        n = Instantiate(iL, Vector3.zero, Quaternion.identity).GetComponent<iLHelper>();
        n.transform.parent = content;
        n.Fill(item, inventory.Count - 1);
        inventoryListings.Add(n);
        //Add item in UI
    }
    public void Drop(int i){
        inventory.RemoveAt(i);
        int n = inventoryListings[i].listIndex;
        Destroy(inventoryListings[i].gameObject);
        inventoryListings.RemoveAt(i);
        Refresh(n);
        //Delete item in UI

    }
    public void Refresh(int i){
        for(;i < inventoryListings.Count; i++){
            inventoryListings[i].Renumber(i);
        }
    }
    public void ChangeValue(int i){
        print(i);
        inventory[i].value = Int32.Parse(inventoryListings[i].valueField.text);
        inventoryListings[i].Refresh();
    }
}
