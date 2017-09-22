using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Inventory : NetworkBehaviour {
    public List<Item> inventory = new List<Item>();
    public List<iLHelper> inventoryListings = new List<iLHelper>();

    public Transform content;

    public GameObject iL;

    bool invActive = false;

    public float tradeRadius;
    Trade currentTrade;

    private void Awake(){
        content = FindObjectOfType<Canvas>().transform.GetChild(0);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Inventory"))
        {
            invActive = !invActive;
            content.gameObject.SetActive(invActive);
        }
    }

    public void Add(Item item){
        inventory.Add(item);
        iLHelper n = Instantiate(iL, Vector3.zero, Quaternion.identity).GetComponent<iLHelper>();
        n.transform.parent = content;
        n.Fill(this, item, inventory.Count - 1);
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

    public void AcceptTrade() { 

    }
    public void DeclineTrade()
    {

    }

    
    public void Trade(int i)
    {
        ItemManager.instance.CmdTrade(0, gameObject.name);
    }

    public Item IDToItem(int id)
    {
        return ItemManager.instance.database[id];
    }

    [ClientRpc]
    public void RpcReceiveTrade(string sender, int itemID)
    {
        Debug.Log("Hello! iz me " + gameObject.name + "! An i gots a trade frem my fren: " + sender + ", and he wan to gifes me: " + IDToItem(itemID).name);
    }

    public void Refresh(int i){
        for(;i < inventoryListings.Count; i++){
            inventoryListings[i].Renumber(this, i);
        }
    }
    public void ChangeValue(int i){
        print(i);
        inventory[i].value = Int32.Parse(inventoryListings[i].valueField.text);
        inventoryListings[i].Refresh(this);
    }
}
