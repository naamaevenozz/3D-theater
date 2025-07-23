using UnityEngine;
using UnityEngine.InputSystem;

public class CSArmsController : MonoBehaviour
{
    [Header("Target Transforms")]
    [SerializeField]
    private Transform rootTarget;

    [SerializeField]
    private Transform leftHandTarget;

    [SerializeField]
    private Transform rightHandTarget;

    [Header("Control Settings")]
    [SerializeField]
    private float maxHandDistance = 1f;

    [Header("Springs")]
    [SerializeField]
    private SpringJoint leftSpring;

    [SerializeField]
    private SpringJoint rightSpring;

    [SerializeField]
    private float handSpringForce = 100f;

    [SerializeField]
    private float handSpringDamper = 20f;

    private Vector3 lastRootPos;
    private Vector3 leftAnchor;
    private Vector3 rightAnchor;

    private bool    holdLeftTrigger;
    private bool    holdRightTrigger;
    private Vector2 rotateInput;
    private Vector3 leftDir;
    private Vector3 rightDir;

    private Gamepad controllingGamepad;
    private bool    isControlActive = false;

    public void SetInputDevice(Gamepad device)
    {
        controllingGamepad = device;
        isControlActive    = device != null;
    }

    private void Start()
    {
        lastRootPos = rootTarget.position;
        leftAnchor  = leftHandTarget.position;
        rightAnchor = rightHandTarget.position;
    }

    private void Update()
    {
        if (!isControlActive || controllingGamepad == null)
        {
            rotateInput      = Vector2.zero;
            holdLeftTrigger  = false;
            holdRightTrigger = false;
            return;
        }

        Vector2 rawInput = controllingGamepad.rightStick.ReadValue();
        float   deadZone = 0.2f;
        rotateInput = rawInput.magnitude < deadZone ? Vector2.zero : rawInput.normalized;

        holdLeftTrigger  = controllingGamepad.leftTrigger.ReadValue()  > 0.1f;
        holdRightTrigger = controllingGamepad.rightTrigger.ReadValue() > 0.1f;
    }

    void Awake()
    {
        var input = GetComponent<PlayerInput>();

        if (input != null) input.enabled = false;
    }


    private void LateUpdate()
    {
        if (!isControlActive || controllingGamepad == null) return;

        Vector3 delta = rootTarget.position - lastRootPos;
        leftAnchor  += delta;
        rightAnchor += delta;
        lastRootPos =  rootTarget.position;

        leftDir  = ComputeStretchDir(holdLeftTrigger);
        rightDir = ComputeStretchDir(holdRightTrigger);

        leftSpring.spring  = (leftDir  != Vector3.zero) ? handSpringForce : 0f;
        leftSpring.damper  = (leftDir  != Vector3.zero) ? handSpringDamper : 0f;
        rightSpring.spring = (rightDir != Vector3.zero) ? handSpringForce : 0f;
        rightSpring.damper = (rightDir != Vector3.zero) ? handSpringDamper : 0f;

        MoveHand(leftHandTarget,  leftAnchor,  leftDir);
        MoveHand(rightHandTarget, rightAnchor, rightDir);
    }

    private Vector3 ComputeStretchDir(bool isStretched)
    {
        if (!isStretched)
            return Vector3.zero;

        return new Vector3(rotateInput.x, rotateInput.y, 0f).normalized;
    }

    private void MoveHand(Transform hand, Vector3 anchor, Vector3 dir)
    {
        if (dir == Vector3.zero)
            return;

        Vector3 desired = anchor + dir * maxHandDistance;
        hand.position = desired;
    }
}