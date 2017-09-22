using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemManager : NetworkBehaviour
{
    public static ItemManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Item> database = new List<Item>();

    [Command]
    public void CmdTrade(int itemID, string playerID)
    {
        GameObject playerGO = GameObject.Find(playerID);
        Debug.Log("Looking for tradey friendz");
        Collider[] n = Physics.OverlapSphere(playerGO.transform.position, 1000);
        List<Inventory> remotePlayerList = new List<Inventory>();
        Debug.Log(n.Length + " is my amount of maybe frendz");
        foreach (Collider nn in n)
        {
            if (nn.transform.root.tag == "Player" && nn.transform.root != transform)
            {
                Debug.Log("Added frend : " + nn.transform.root.name);
                remotePlayerList.Add(nn.transform.root.GetComponent<Inventory>());
            }
        }
        if (remotePlayerList.Count > 0)
        {
            foreach(Inventory i in remotePlayerList)
            {
                i.RpcReceiveTrade(playerID, itemID);
            }
        }
        else
        {
            print("No frendz to trad hary with");
        }

    }
}
