using System.Collections.Generic;
using Edelweiss.Core;
using Edelweiss.Utils;
using UnityEngine;

namespace Edelweiss.Damage
{
    public class HitContext : EventContextBase<HitAccumulator>
    {
        public readonly DamageablePart  TargetPart;
        public readonly DamageComponent Damager;

        public readonly float Damage;

        public readonly float   Magnitude;
        public readonly Vector3 Impulse;
        public readonly Vector3 Normal;
        public readonly Vector3 Point;
        public readonly Vector3 Direction;
        public readonly Vector3 RelativeVelocity;

        public HitContext(DamageComponent damager, DamageablePart targetPart, Collision collision) :
            base(targetPart.HitAccumulator)
        {
            TargetPart = targetPart;
            Damager    = damager;

            Impulse          = collision.impulse.With(z: 0) * EdelweissConfiguration.Instance.ImpulseScale;
            Magnitude        = Impulse.magnitude;
            Normal           = collision.contacts.Length > 0 ? collision.contacts[0].normal : Vector3.zero;
            Point            = collision.contacts.Length > 0 ? collision.contacts[0].point : Vector3.zero;
            Direction        = Impulse.normalized;
            RelativeVelocity = collision.relativeVelocity.With(z: 0);

            Damage = CalculateDamage();
        }

        private float CalculateDamage()
        {
            float atkScale = Damager.DamageScale;
            float atkAdd   = Damager.DamageAddition;
            float defScale = TargetPart.DamageScale;
            float defAdd   = TargetPart.DamageAddition;

            return (Magnitude * atkScale + atkAdd) * defScale + defAdd;
        }

        protected override List<(string, object)> GetDebugStringFields()
        {
            return new()
                   {
                       ("Sender", TargetPart.gameObject.name),
                       ("Damager", Damager.gameObject.name),
                       ("Damage", Damage),
                       ("Magnitude", Magnitude),
                       ("Impulse", Impulse.ToString()),
                       ("Normal", Normal.ToString()),
                       ("Point", Point.ToString()),
                       ("Direction", Direction.ToString())
                   };
        }
    }
}