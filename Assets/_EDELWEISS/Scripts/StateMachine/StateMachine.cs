using System;
using System.Collections.Generic;
using ImprovedTimers;

namespace Edelweiss.StateMachine
{
    public class StateMachine<T> where T : IState
    {
        private StateNode currentState;
        public  string    CurrentStateName => CurrentState.ToString();

        StateNode Current
        {
            get => currentState;
            set
            {
                OnStateChange?.Invoke(new StateChaneEvent<T>(Current.State, value.State));
                currentState = value;
            }
        }

        Dictionary<Type, StateNode> nodes          = new();
        HashSet<ITransition<T>>     anyTransitions = new();

        public event Action<StateChaneEvent<T>> OnStateChange;

        protected T CurrentState => Current != null ? Current.State : default;

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);

            Current.State?.Update();
        }

        public void FixedUpdate()
        {
            Current.State?.FixedUpdate();
        }

        public void SetState(T state)
        {
            Current = nodes[state.GetType()];
            Current.State?.OnEnter();
        }

        void ChangeState(T state)
        {
            if (state.Equals(Current.State)) return;

            var previousState = Current.State;
            var nextState     = nodes[state.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();
            Current = nodes[state.GetType()];
        }

        ITransition<T> GetTransition()
        {
            foreach (var transition in anyTransitions)
                if (transition.Condition.Evaluate())
                    return transition;

            foreach (var transition in Current.Transitions)
                if (transition.Condition.Evaluate())
                    return transition;

            return null;
        }

        public void AddTransition(T from, T to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(T to, IPredicate condition)
        {
            anyTransitions.Add(new Transition<T>(GetOrAddNode(to).State, condition));
        }

        StateNode GetOrAddNode(T state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());

            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(), node);
            }

            return node;
        }

        public override string ToString()
        {
            return CurrentState.GetType().Name;
        }

        public void Kill()
        {
            Current = null;
            nodes.Clear();
            anyTransitions.Clear();
        }

        class StateNode
        {
            public T                       State       { get; }
            public HashSet<ITransition<T>> Transitions { get; }

            public StateNode(T state)
            {
                State       = state;
                Transitions = new HashSet<ITransition<T>>();
            }

            public void AddTransition(T to, IPredicate condition)
            {
                Transitions.Add(new Transition<T>(to, condition));
            }
            
            public override string ToString()
            {
                return State.GetType().Name;
            }
        }

        public TransitionBuilder Transition() => new TransitionBuilder(this);

        public class TransitionBuilder
        {
            private readonly StateMachine<T> stateMachine;
            private readonly List<T>         from;
            private          T               to;
            private          IPredicate      condition;

            public TransitionBuilder(StateMachine<T> stateMachine)
            {
                this.stateMachine = stateMachine;
                this.from         = new List<T> { };
            }

            public TransitionBuilder From(T state)
            {
                from.Add(state);
                return this;
            }

            public TransitionBuilder From(params T[] states)
            {
                from.AddRange(states);
                return this;
            }

            public TransitionBuilder To(T state)
            {
                to = state;
                return this;
            }

            public TransitionBuilder When(IPredicate condition)
            {
                this.condition = condition;
                return this;
            }

            public TransitionBuilder When(Func<bool> condition)
            {
                this.condition = new FuncPredicate(condition);
                return this;
            }

            public void Add()
            {
                foreach (var state in from)
                {
                    stateMachine.AddTransition(state, to, condition);
                }
            }
        }
    }
}