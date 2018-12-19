using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnObservableObject {
    protected internal List<IPropertyChangeListener> listeners = new List<IPropertyChangeListener>();
	// Use this for initialization
    public void Register(IPropertyChangeListener listener)
    {
        listeners.Add(listener);
        GameObject.Find("Player").GetComponent<PlayerController>().LetMeKnow("Listeners: " + listeners.Count);

    }

    public virtual void NotifyAllListeners(string propertyName, System.Object oldValue, System.Object newValue)
    {
        GameObject.Find("Player").GetComponent<PlayerController>().LetMeKnow("Notifying!");
        GameObject.Find("GameRules").GetComponent<EnemySpawner>().NewPropertyChangeEvent(new PropertyChangeEvent(this, propertyName, oldValue, newValue));
        for (int i = 0; i < listeners.Count; i++) {
            Debug.Log(newValue);
            listeners[i].NewPropertyChangeEvent(new PropertyChangeEvent(this, propertyName, oldValue, newValue));
        }
    }
}
