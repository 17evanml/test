using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SingletonsCreator {
    internal static MoveSetTable moveSetTable;
    internal static LockableQueue<CombatResponse> defenseQueue;
    internal static LockableQueue<CombatResponse> attackQueue;
    internal static GenericQueue<AnimationBean> animQueue;
    internal static ANotifyingList<EnemyLogic> enemyList;

    public static MoveSetTable SingleMoveSetTable()
    {
        if (moveSetTable == null) {
            return new MoveSetTable();
        } else {
            return moveSetTable;
        }
    }

    public static LockableQueue<CombatResponse> DefenseQueue()
    {
        if (defenseQueue == null)
        {
            return new LockableQueue<CombatResponse>();
        }
        else
        {
            return defenseQueue;
        }
    }

    public static LockableQueue<CombatResponse> AttackQueue()
    {
        if (attackQueue == null)
        {
            return new LockableQueue<CombatResponse>();
        }
        else
        {
            return attackQueue;
        }
    }
    public static GenericQueue<AnimationBean> AnimationQueue()
    {
        if (animQueue == null)
        {
            return new GenericQueue<AnimationBean>();
        }
        else
        {
            return animQueue;
        }
    }

    public static ANotifyingList<EnemyLogic> EnemyList()
    {
        if(enemyList == null)
        {
            return new ANotifyingList<EnemyLogic>();
        }
        else
        {
            return enemyList;
        }
    }


}
