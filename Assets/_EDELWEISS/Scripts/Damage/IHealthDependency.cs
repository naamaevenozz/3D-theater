namespace Edelweiss.Damage
{
    public interface IHittable
    {
        void InjectHitAccumulator(HitAccumulator hitAccumulator);
    }
}