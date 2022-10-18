using UnityEngine;
using UnityEngine.Events;

public class Bubble : MonoBehaviour
{
    public UnityAction<GameObject> OnTriggerEntered;

    private void OnTriggerEnter(Collider other)
        => OnTriggerEntered?.Invoke(other.gameObject);
}
