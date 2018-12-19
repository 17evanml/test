using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericQueue<T> {

    internal List<T> myQueue = new List<T>();
    virtual public void Add(T element)
    {
        myQueue.Add(element);
    }


    public T Next()
    {
        if (myQueue.Count > 0)
        {
            T firstItem = myQueue[0];
            myQueue.RemoveAt(0);
            return firstItem;
        }
        else
        {
            throw new System.Exception("Queue is empty!");
        }
    }

    public int Length()
    {
        return myQueue.Count;
    }

    public void Clear()
    {  
        myQueue.Clear();
    }
}
