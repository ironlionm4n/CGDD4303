using UnityEngine;
using UnityEngine.Events;
public class GameEventListener : MonoBehaviour
{
    [Header("Game Event to listen for")]
    public GameEvent gameEvent;

    [Header("Event to fire")]
    public UnityEvent onEventTriggered;

    void OnEnable()
    {
        gameEvent.AddListener(this);
    }

    void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }

    public void OnEventTriggered()
    {
        onEventTriggered.Invoke();
    }
}
