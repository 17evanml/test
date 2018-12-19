using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyChangeEvent {
    public System.Object Source { get; private set; }
    public string PropertyName { get; private set; }
    public System.Object OldValue { get; private set; }
    public System.Object NewValue { get; private set; }

    public PropertyChangeEvent(System.Object source, string propertyName, System.Object oldValue, System.Object newValue)
    {
        Source = source;
        PropertyName = propertyName;
        OldValue = oldValue;
        NewValue = newValue;
    }
}
