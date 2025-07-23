using Edelweiss.Core;
using UnityEngine;

public class KissCollision : EdelMono
{

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("FrontFace"))
        {
            Debug.Log("Kiss collision detected!");

            GameEvents.OnKissCollision?.Invoke(collider);

        }
    }
}