using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AuthorityHandler : NetworkBehaviour {
    
    public void SetAuth(NetworkIdentity objectId, NetworkIdentity player)
    {
        var otherOwner = objectId.clientAuthorityOwner;

        if (otherOwner == player.connectionToClient)
        {
            return;
        }
        else
        {
            if (otherOwner != null)
            {
                objectId.RemoveClientAuthority(otherOwner);
            }
            objectId.AssignClientAuthority(player.connectionToClient);
        }
    }
}
