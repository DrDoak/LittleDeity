using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Phantom : Property {

    public override void OnAddProperty()
    {
        GetComponent<Attackable>().Faction = FactionType.NONE;
    }

    public override void OnRemoveProperty()
    {
        GetComponent<Attackable>().Faction = FactionType.HOSTILE;
    }

}
