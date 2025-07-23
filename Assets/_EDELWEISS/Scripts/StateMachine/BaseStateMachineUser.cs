using System;
using Edelweiss.Core;
using UnityEngine;

namespace Edelweiss.StateMachine
{
    public abstract class BaseStateMachineUser<T, S> : EdelMono
        where T : IState where S : StateMachine<T>
    {
        [SerializeField]
        private string currentStateName;

        protected S StateMachine;
        public String CurrentStateName => this.StateMachine.ToString();

        public event Action<StateChaneEvent<T>> OnStateChange = delegate { };

        protected virtual void Awake()
        {
            SetupStateMachine();

            StateMachine.OnStateChange += (context) => OnStateChange?.Invoke(context);
            StateMachine.OnStateChange += (context) => currentStateName = context.To.GetType().Name;

            currentStateName = StateMachine.CurrentStateName;
        }

        protected abstract void SetupStateMachine();
        protected virtual void Update() => StateMachine?.Update();
        protected virtual void FixedUpdate() => StateMachine?.FixedUpdate();


        protected void At(T from, T to, IPredicate condition) =>
            this.StateMachine.AddTransition(from, to, condition);

        protected void Any(T to, IPredicate condition) => this.StateMachine.AddAnyTransition(to, condition);
        protected FuncPredicate When(Func<bool> condition) => new FuncPredicate(condition);
    }
}