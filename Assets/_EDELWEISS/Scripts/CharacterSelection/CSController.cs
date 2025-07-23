using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterCon : MonoBehaviour
{
    private Gamepad controllingGamepad;
    private bool isControlActive = false;

    public void SetInputDevice(Gamepad device)
    {
        controllingGamepad = device;
        isControlActive = device != null;
    }

    private void Update()
    {
        if (!isControlActive || controllingGamepad == null) return;

        Vector2 move = controllingGamepad.leftStick.ReadValue();
        if (move.magnitude > 0.01f)
            transform.Translate(move.x * Time.deltaTime * 5f, 0, 0);
    }
}