using Edelweiss.Core;
using UnityEngine;

namespace Edelweiss.Utils.Pool
{
    public interface IPool<T> where T : EdelMono, IPoolable
    {
        T    Get();
        T    Get(Vector3     position, Quaternion rotation);
        void Return(T        obj);
        void ReturnDelayed(T obj, float delay);
    }
}