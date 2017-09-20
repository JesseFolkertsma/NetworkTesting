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

    float tradeRadius;
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

        Collider[] n = Physics.OverlapSphere(transform.position, tradeRadius, LayerMask.NameToLayer("RemotePlayer"));
        if (n.Length < 0)
        {
            Inventory[] nl = new Inventory[n.Length];
            for (int ii = 0; ii < n.Length; ii++){
                nl[ii] = n[ii].transform.GetComponent<Inventory>();
            }
            Trade newTrade = new Trade(inventory[i], this, nl);
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
        print("Ohboi waddup its hary");
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
