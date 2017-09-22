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
    }
    
    public void SendTrade(byte[] serTrade, string playerID)
    {
        Trade trade = Trade.DeserializeFromNetwork(serTrade);
        int newTradeID = CreateRandomTradeID();
        ongoingTrades.Add(newTradeID, trade);
        foreach (KeyValuePair<string, Inventory> kvp in trade.receivers)
        {
            kvp.Value.RpcReceiveTrade(serTrade, newTradeID);
        }
    }

    public int CreateRandomTradeID()
    {
        int rng = UnityEngine.Random.Range(0, 1000);
        if (ongoingTrades.ContainsKey(rng))
        {
            rng = CreateRandomTradeID();
        }
        return rng;
    }
}
