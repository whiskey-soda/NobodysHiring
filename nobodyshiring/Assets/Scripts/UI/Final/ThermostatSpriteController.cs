using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ThermostatSpriteController : MonoBehaviour
{
    Thermostat thermostat;
    Button thermostatButton;

    [SerializeField] Sprite thermostatOn;
    [SerializeField] Sprite thermostatOff;
    [SerializeField] Sprite thermostatBlank;

    private void Start()
    {
        thermostat = Thermostat.Instance;
        thermostatButton = GetComponent<Button>();

        SetThermostatSprite();
    }

    public void SetThermostatSprite()
    {
        if (thermostat.isOn) { thermostatButton.image.sprite = thermostatOn; }
        else if (!thermostat.isOn) { thermostatButton.image.sprite = thermostatOff; }
        else { thermostatButton.image.sprite = thermostatBlank; }
    }
}
