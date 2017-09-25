using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemManager : NetworkBehaviour
{
    public static ItemManager instance;

    public Dictionary<int, Trade> ongoingTrades;

    private void Awake()
    {
        instance = this;
        ongoingTrades = new Dictionary<int, Trade>();
    }

    public void TradeAccepted(int tradeID, string playerID)
    {

    }

    public void TradeDeclined(int tradeID, string playerID)
    {

    }
    
    public void SendTrade(byte[] serTrade, string playerID)
    {
        Trade trade = Trade.DeserializeFromNetwork(serTrade);
        int newTradeID = CreateRandomTradeID();
        ongoingTrades.Add(newTradeID, trade);
        foreach (string s in trade.receivers)
        {
            GameObject target = GameObject.Find(s);
            target.GetComponent<Inventory>().TargetRecieveTrade(target.GetComponent<NetworkIdentity>().connectionToClient, serTrade, newTradeID);
        }
    }

    public int CreateRandomTradeID()
    {
        int rng = UnityEngine.Random.Range(0, 1000);
        if (ongoingTrades.Count > 0)
        {
            if (ongoingTrades.ContainsKey(rng))
            {
                rng = CreateRandomTradeID();
            }
        }
        return rng;
    }

    public void GiveItemToPlayerID(string id, Item itemToGive)
    {
        GameObject.Find(id).GetComponent<Inventory>().Add(itemToGive);
    }
}
