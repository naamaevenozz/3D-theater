using System;
using Edelweiss.Core;
using Edelweiss.Damage;
using Edelweiss.Utils;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableItem : EdelMono
{
    private const int       DEFAULT_ORIGINAL_LAYER = -1;
    private       Rigidbody _rb;

    [SerializeField]
    private EdelEvent OnGrabEvent = new();

    [SerializeField]
    private EdelEvent OnReleaseEvent = new();
    
    [SerializeField]
    private bool inheritGrabberLayer = true;

    private int originalLayer = DEFAULT_ORIGINAL_LAYER;

    private ConfigurableJoint anchorJoint;

    private float forceThreshold = 100f;
    private float pullTimer = 0;
    private float timeToBreak    = 0.75f;
    
    public event Action OnAnchorBroken;
    public bool IsAnchored => anchorJoint;
    


    
    public bool CanBeGrabbed => _state == ObjectState.Grabbable;

    [SerializeField] private DamageComponent damageComponent;
    
    
    [SerializeField] private float velThreshold = 1;
    [SerializeField] private float   timeToLand = 0.5f;
    private float          _landTimer;


    
    private ObjectState _state = ObjectState.Grabbable;
    


    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        if (TryGetComponent<ConfigurableJoint>(out var cj) && cj.connectedBody == null)
        {
            anchorJoint = cj;
            anchorJoint.breakForce  = float.MaxValue;
            anchorJoint.breakTorque = float.MaxValue;
            anchorJoint.connectedBody  = null; // anchor to world
            anchorJoint.xMotion        = ConfigurableJointMotion.Locked;
            anchorJoint.yMotion        = ConfigurableJointMotion.Locked;
            anchorJoint.zMotion        = ConfigurableJointMotion.Locked;
            anchorJoint.angularXMotion = ConfigurableJointMotion.Locked;
            anchorJoint.angularYMotion = ConfigurableJointMotion.Locked;
            anchorJoint.angularZMotion = ConfigurableJointMotion.Locked;
        }
    }
    

    private void FixedUpdate()
    {
        if (anchorJoint != null)
        {
            float pull = anchorJoint.currentForce.magnitude;

            if (pull > forceThreshold)
            {
                pullTimer += Time.fixedDeltaTime;
            }

            else pullTimer = 0;

            if (pullTimer < timeToBreak) return;

            BreakAnchorJoint();
            
        }
        
        
        switch (_state)
        {
            case ObjectState.Grabbed:
                SetDamageEnabled(true);
                break;

            case ObjectState.Thrown:
                SetDamageEnabled(true);
                if (_rb.linearVelocity.magnitude < velThreshold)
                {
                    _landTimer += Time.fixedDeltaTime;
                    if (_landTimer >= timeToLand)
                    {
                        BecomeGrabbable();
                    }
                }
                else
                {
                    _landTimer = 0f;
                }
                break;

            case ObjectState.Grabbable:
                SetDamageEnabled(false);
                break;
        }
    }

    private void BreakAnchorJoint()
    {
        
        Destroy(anchorJoint);
        anchorJoint = null;
        OnAnchorBroken?.Invoke();
    }

    public Rigidbody GetRigidbody()
    {
        return _rb;
    }

    void SetLayer(int layer)
    {
        gameObject.layer = layer;

        foreach (Transform child in transform)
        {
            child.gameObject.layer = layer;
        }
    }

    public void OnGrab(HandCollector handCollector)
    {
        if (!CanBeGrabbed) return;

        _state = ObjectState.Grabbed;
        
        if (inheritGrabberLayer)
        {
            originalLayer = gameObject.layer;
            SetLayer(handCollector.gameObject.layer);
        }

        OnGrabEvent?.Invoke();
    }

    public void OnRelease()
    {
        if (_state != ObjectState.Grabbed) return;

        _state = ObjectState.Thrown;
        _landTimer = 0f;
        
        /*if (originalLayer != DEFAULT_ORIGINAL_LAYER)
        {
            SetLayer(originalLayer);
            originalLayer = DEFAULT_ORIGINAL_LAYER;
        }*/

        OnReleaseEvent?.Invoke();

       // isGrabbed = false;
    }
    
    private void BecomeGrabbable()
    {
        _state = ObjectState.Grabbable;
        _landTimer = 0f;
        
        if (originalLayer >= 0)
        {
            SetLayer(originalLayer);
            originalLayer = DEFAULT_ORIGINAL_LAYER;
        }
    }
    
    
    
    private void SetDamageEnabled(bool state)
    {
        if (damageComponent != null && damageComponent.enabled != state)
        {
            damageComponent.enabled = state;
        }
    }
}


public enum ObjectState
{
    Grabbable,
    Thrown,
    Grabbed
}