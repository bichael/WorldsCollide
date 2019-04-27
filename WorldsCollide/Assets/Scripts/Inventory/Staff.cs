using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : InventoryItemBase
{
    public Player player;
    public override void OnUse()
    {
        base.OnUse();
    }

    public override void OnPickup()
    {
        base.OnPickup();
        player.HasStaff = true;
    }
}
