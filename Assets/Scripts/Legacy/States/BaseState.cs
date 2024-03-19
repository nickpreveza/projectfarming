using System;
using UnityEngine;

namespace HempStateMachine
{
    public abstract class BaseState
    {
        public abstract Type Update();
        public abstract void Enter();
        public abstract void Exit();

        protected abstract Type CheckSwitchState();
    }
}

