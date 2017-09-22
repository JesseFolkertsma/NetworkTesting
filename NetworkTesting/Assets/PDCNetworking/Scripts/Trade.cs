using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Trade {
    public Item item;
    public Inventory seller;
    public Dictionary<string, Inventory> receivers;

    public Trade()
    {
        item = null;
        seller = null;
        receivers = new Dictionary<string, Inventory>();
    }

    public Trade(Item _item, Inventory _seller, Inventory[] _receivers){
        item = _item;
        seller = _seller;
        receivers = new Dictionary<string, Inventory>();
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

    public static Trade DeserializeFromNetwork(byte[] networkSerializedTrade)
    {
        MemoryStream stream = new MemoryStream(networkSerializedTrade);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        return (Trade)binaryFormatter.Deserialize(stream);
    }

    public byte[] SerializeForNetwork()
    {
        MemoryStream stream = new MemoryStream();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(stream, this);
        return stream.GetBuffer();
    }
}
