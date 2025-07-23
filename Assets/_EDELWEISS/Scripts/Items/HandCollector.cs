using System;
using Edelweiss.Core;
using Edelweiss.Core.FX;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandCollector : EdelMono
{
    [SerializeField]
    private PlayerInput input;

    private InputAction _triggerAction;

    [SerializeField]
    private bool isRightHand;

    private Rigidbody rb;

    // Hover = item in range you could grab.
    private GrabbableItem hoverCollectible;

    // Held = item you have grabbed.
    private GrabbableItem heldCollectible;

    private ConfigurableJoint _joint;


    private bool holdingTrigger;
    private bool grabbingItem;

    [SerializeField]
    private HoverItemEffect hoverItemEffect;

    private void Awake()
    {
        ValidateComponent(ref rb);
    }

    private void OnEnable()
    {
        _triggerAction = isRightHand ? input.actions["RightPunch"] : input.actions["LeftPunch"];
        _triggerAction.Enable();
    }

    private void OnDisable()
    {
        _triggerAction.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        var jc = other.GetComponent<GrabbableItem>();
        if (jc != null && !grabbingItem)
        {
            hoverCollectible = jc;
            hoverItemEffect.CreateString(hoverCollectible.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var jc = other.GetComponent<GrabbableItem>();
        if (jc != null && jc == hoverCollectible && !grabbingItem)
        {
            hoverCollectible = null;
            hoverItemEffect.ResetEffect();
        }

        if (jc != null)
        {
            hoverItemEffect.ResetEffect();
        }
    }

    private void Update()
    {
        holdingTrigger = _triggerAction.ReadValue<float>() > 0.1f;
        if (holdingTrigger && !grabbingItem && hoverCollectible != null && hoverCollectible.CanBeGrabbed)
        {
            heldCollectible = hoverCollectible;
            //heldCollectible.AttachTo(GetComponentInParent<Rigidbody>());
            Attach();
            grabbingItem     = true;
            hoverCollectible = null;
        }
        else if (!holdingTrigger && grabbingItem && heldCollectible != null)
        {
            //heldCollectible.Detach();
            Detach();
            heldCollectible = null;
            grabbingItem    = false;
        }
    }


    private void Attach()
    {
        // _joint                              = gameObject.AddComponent<ConfigurableJoint>();
        // _joint.connectedBody                = heldCollectible.GetRigidbody();

        _joint                              = heldCollectible.gameObject.AddComponent<ConfigurableJoint>();
        _joint.connectedBody                = rb;
        _joint.autoConfigureConnectedAnchor = false;

        heldCollectible.OnGrab(this);

        _joint.anchor             = Vector3.zero;
        
        if(!heldCollectible.IsAnchored) _joint.connectedMassScale = 0;
        else
        {
            void OnBroken()
            {
                _joint.connectedMassScale = 0;
                heldCollectible.OnAnchorBroken -= OnBroken;
            }
            heldCollectible.OnAnchorBroken += OnBroken;
        }

        _joint.angularXMotion = ConfigurableJointMotion.Free;
        _joint.angularYMotion = ConfigurableJointMotion.Free;
        _joint.angularZMotion = ConfigurableJointMotion.Free;

        _joint.xMotion = ConfigurableJointMotion.Limited;
        _joint.yMotion = ConfigurableJointMotion.Limited;
        _joint.zMotion = ConfigurableJointMotion.Limited;
    }

    private void Detach()
    {
        Destroy(_joint);
        heldCollectible.OnRelease();
        _joint = null;
    }
}