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
    public void Trade(int i){
        Debug.Log("Looking for tradey friendz");
        Collider[] n = Physics.OverlapSphere(transform.position, 1000);
        List<Inventory> remotePlayerList = new List<Inventory>();
        Debug.Log(n.Length + " is my amount of maybe frendz");
        foreach(Collider nn in n)
        {
            if(nn.transform.root.tag == "Player" && nn.transform.root != transform)
            {
                Debug.Log("Added frend : " + nn.transform.root.name);
                remotePlayerList.Add(nn.transform.root.GetComponent<Inventory>());
            }
        }
        if (remotePlayerList.Count > 0)
        {
            Trade newTrade = new Trade(inventory[i], this, remotePlayerList.ToArray());
            currentTrade = newTrade;
            CmdTrade(gameObject.name, currentTrade.item.name, currentTrade.item.value);
        }
        else
        {
            print("No frendz to trad hary with");
        }

    }
    public void AcceptTrade() { 

    }
    public void DeclineTrade()
    {

    }
    [Command]
    public void CmdTrade(string PlayerID, string name, int value){
        foreach(KeyValuePair<string, Inventory> n in currentTrade.receivers)
        {
            n.Value.RpcReceiveTrade();
        }
    }
    [ClientRpc]
    public void RpcReceiveTrade(){
        Debug.Log("Ohboi waddup its hary");
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
