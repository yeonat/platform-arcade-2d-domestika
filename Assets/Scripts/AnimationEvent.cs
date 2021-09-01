using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public UnityEvent[] eventToRise;

    private int _id;

    public void RunConsecutiveEvent()
    {
        eventToRise[_id]?.Invoke();
        _id++;
    }

    public void RunEventById(int id)
    {
        _id = id;
        eventToRise[_id]?.Invoke();

        _id++;
    }
}