using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SingletonsCreator {
    internal static MoveSetTable moveSetTable;
    internal static CombatQueue defenseQueue;
    internal static CombatQueue attackQueue;
    internal static AnimationQueue animQueue;

    public static MoveSetTable SingleMoveSetTable()
    {
        if (moveSetTable == null) {
            return new MoveSetTable();
        } else {
            return moveSetTable;
        }
    }

    public static CombatQueue DefenseQueue()
    {
        if (defenseQueue == null)
        {
            return new CombatQueue();
        }
        else
        {
            return defenseQueue;
        }
    }

    public static CombatQueue AttackQueue()
    {
        if (attackQueue == null)
        {
            return new CombatQueue();
        }
        else
        {
            return attackQueue;
        }
    }
    public static AnimationQueue AnimationQueue()
    {
        if (animQueue == null)
        {
            return new AnimationQueue();
        }
        else
        {
            return animQueue;
        }
    }


}
