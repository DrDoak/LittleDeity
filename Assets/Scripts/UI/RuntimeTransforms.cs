using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class RuntimeTransforms
{
    public static Vector3 LeftBase = new Vector3(-60,0,0), MiddleBase = new Vector3(0,-75,0), RightBase = new Vector3(60,0,0);
    public static Vector3 UpBase = new Vector3(0, 75, 0), DownBase = new Vector3(0, -75, 0);

    public static int LevelCount = 0;
    public static int Level;

    public static BranchDirection GetBranchDirection(AbilityType abilityType)
    {
        switch (abilityType)
        {
            case AbilityType.COMBAT:
                return BranchDirection.LEFT;
            case AbilityType.SPECIAL:
                return BranchDirection.RIGHT;
            default:
                return BranchDirection.RIGHT;
        }
    }

    public static Vector3 GetVector(int level, BranchDirection branch)
    {
        Vector3 position = new Vector3();
        LevelModifiers(level);
        //if (level == 0) return MiddleBase;
        switch (branch)
        {
            case BranchDirection.LEFT:
                position = LeftBase;
                break;
            case BranchDirection.RIGHT:
                position = RightBase;
                break;
            case BranchDirection.MIDDLE:
                position = MiddleBase;
                break;
        }
        position *= level;
        position += GetLevelPosition();
        return position;
    }

    public static void LevelModifiers(int level)
    {
        if (Level != level)
        {
            Level = level;
            LevelCount = 0;
        }
        LevelCount++;
    }

    public static Vector3 GetLevelPosition()
    {
        Vector3 pos = new Vector3();
        switch (LevelCount)
        {
            case 2:
                pos = UpBase;
                break;
            case 1:
                pos = Vector3.zero;
                break;
            case 3:
                pos = DownBase;
                break;
        }
        return pos;
    }
}

public enum BranchDirection
{
    LEFT, RIGHT, MIDDLE, UP, DOWN
}
