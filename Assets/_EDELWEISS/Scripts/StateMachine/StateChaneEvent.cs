namespace Edelweiss.StateMachine
{
    public struct StateChaneEvent<T> where T : IState
    {
        public readonly T From;
        public readonly T To;

        public StateChaneEvent(T from, T to)
        {
            From = from;
            To   = to;
        }
    }
}