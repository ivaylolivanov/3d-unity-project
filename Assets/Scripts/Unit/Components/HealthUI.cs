using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Slider _healthSlider;

    private void OnEnable()
    {
        _healthSlider = GetComponentInChildren<Slider>();
        if (_healthSlider == null)
        {
            Debug.LogError($"Failed to find slider in HealthUI.cs of {gameObject.name}");
            return;
        }
    }

    public void UpdateCurrentValue(int currentHealth)
        => _healthSlider.value = currentHealth;
    public void UpdateMaxValue(int maxValue)
        => _healthSlider.maxValue = maxValue;
}
