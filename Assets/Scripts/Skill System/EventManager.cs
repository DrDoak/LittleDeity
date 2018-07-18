using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public const int USE_ABILITY = 0;
    public const int TRANSFER_SPECIAL = 1;
    public const int MELEE_SPECIAL = 2;
    public const int LEVEL_UP = 3;

    public delegate void VoidDelegate();

    public static event VoidDelegate UseAbilityEvent;

    public static event VoidDelegate TransferSpecialEvent;

    public static event VoidDelegate MeleeSpecialEvent;

    public static event VoidDelegate LevelUpEvent;

    public static void TriggerEvent(int index = 0)
    {
        switch (index)
        {
            case USE_ABILITY:
                UseAbilityEvent();
                break;
            case TRANSFER_SPECIAL:
                if (TransferSpecialEvent != null)
                    TransferSpecialEvent();
                break;
            case MELEE_SPECIAL:
                if (MeleeSpecialEvent != null)
                    MeleeSpecialEvent();
                break;
            case LEVEL_UP:
                if (LevelUpEvent != null)
                    LevelUpEvent();
                break;
        }
    }

    
}
