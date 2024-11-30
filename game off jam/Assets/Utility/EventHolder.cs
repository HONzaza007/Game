using UnityEngine;
using UnityEngine.Events;

public class EventHolder : MonoBehaviour
{
    public UnityEvent[] eventArray;
    

    public void TriggerEvent(int i)
    {
        eventArray[i].Invoke();
    }
}
