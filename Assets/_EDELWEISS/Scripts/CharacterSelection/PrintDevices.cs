using UnityEngine;
using UnityEngine.InputSystem;

public class PrintDevices : MonoBehaviour
{
    void Start()
    {
        foreach (var device in InputSystem.devices)
        {
            Debug.Log($"Device: {device.displayName}, layout: {device.layout}, description: {device.description}");
        }
    }
}
