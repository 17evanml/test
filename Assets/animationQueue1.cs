using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationQueue {

    internal List<AnimationBean> animQueue = new List<AnimationBean>();
    public void Add(AnimationBean animSet)
    {
        animQueue.Add(animSet);
    }


    public AnimationBean Next()
    {
        if (animQueue.Count > 0)
        {
            AnimationBean firstItem = animQueue[0];
            animQueue.RemoveAt(0);
            return firstItem;
        }
        else
        {
            throw new System.Exception("Animation Queue is empty!");
        }
    }

    public int Length()
    {
        return animQueue.Count;
    }

    public void Clear()
    {  
        animQueue.Clear();
    }
}
