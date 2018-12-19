using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANotifyingList<T> : AnObservableObject {
   internal List<T> notifyingList = new List<T>();

   public void Add(T item)
    {
        notifyingList.Add(item);
    }

   public void Remove(T item) {
        int oldValue = Count();
        notifyingList.Remove(item);
        int newValue = Count();
        if(newValue == 0)
        {
           base.NotifyAllListeners("Count", oldValue, newValue);
        }
    }

    public int Count()
    {
        return notifyingList.Count;
    }

    public T Get(int index)
    {
        return notifyingList[index];
    }

    public int ListenerCount()
    {
        return listeners.Count;
    }
}
