using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockableQueue<T>: GenericQueue<T> {
    private bool IsLock { get; set; }
    internal List<T> combatQueue = new List<T>();
    override public void Add(T element) {
        if (!IsLock) {
            base.Add(element);
        }
    }


    public void Lock() {
        IsLock = true;
    }
    public void Unlock() {
        IsLock = false;
    }
}
