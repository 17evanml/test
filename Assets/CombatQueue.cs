using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatQueue {
    private bool IsLock { get; set; }
    internal List<CombatResponse> combatQueue = new List<CombatResponse>();
    public void Add(CombatResponse attack) {
        if (!IsLock) {
            combatQueue.Add(attack);
        }
    }


    public CombatResponse Next() {
        if(combatQueue.Count > 0) {
            CombatResponse firstItem = combatQueue[0];
            combatQueue.RemoveAt(0);
            return firstItem;
        } else {
            throw new System.Exception("Combat Queue is empty!");
        }
    }

    public int Length() {
        return combatQueue.Count;
    }

    public void Clear () {
        combatQueue.Clear();
    }

    public void Lock() {
        IsLock = true;
    }
    public void Unlock() {
        IsLock = false;
    }
}
