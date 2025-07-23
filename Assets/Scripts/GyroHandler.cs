using UnityEngine;
using UnityEngine.InputSystem;

public class GyroCheck : MonoBehaviour
{
    void Update()
    {
        if (UnityEngine.InputSystem.Gyroscope.current != null)
        {
            Vector3 rotation = UnityEngine.InputSystem.Gyroscope.current.angularVelocity.ReadValue();
            Debug.Log("Gyro rotation: " + rotation);
        }
        else
        {
            Debug.Log("Gyroscope not available.");
        }
    }
}