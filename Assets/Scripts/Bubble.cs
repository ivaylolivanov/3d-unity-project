using UnityEngine;
using UnityEngine.Events;

public class Bubble : MonoBehaviour
{
    public UnityAction<GameObject, GameObject> OnTriggerEntered;
    public UnityAction<GameObject, GameObject> OnTriggerExited;

    private void OnTriggerEnter(Collider other)
        => OnTriggerEntered?.Invoke(other.gameObject, gameObject);

    private void OnTriggerExit(Collider other)
        => OnTriggerExited?.Invoke(other.gameObject, gameObject);
}
