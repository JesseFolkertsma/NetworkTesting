using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade {
    public Item item;
    public Inventory seller;
    public Dictionary<string, Inventory> receivers;
    public Trade(Item _item, Inventory _seller, Inventory[] _receivers){
        item = _item;
        seller = _seller;
        foreach(Inventory i in _receivers)
        {
            receivers.Add(i.gameObject.name, i);
        }
    }
    public void Accept(string playerID){
        receivers[playerID].Add(item);
        receivers.Remove(playerID);
        foreach(KeyValuePair<string, Inventory> i in receivers)
        {
            i.Value.DeclineTrade();
        }
    }
    public void Decline(string playerID){
        receivers.Remove(playerID);
        if(receivers.Count < 1){
            Stop();
        }
    }
    public void Stop(){

    }
}
