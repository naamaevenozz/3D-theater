namespace Edelweiss.StateMachine
{
    public class Transition<T> : ITransition<T> where T : IState
    {
        public T To { get; }
        public IPredicate Condition { get; }

        public Transition(T to, IPredicate condition)
        {
            To = to;
            Condition = condition;
        }
    }
}