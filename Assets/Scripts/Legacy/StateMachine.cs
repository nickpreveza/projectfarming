using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HempStateMachine
{
    public class StateMachine : MonoBehaviour
    {
        private Dictionary<Type, BaseState> availableStates;
        public BaseState currentState { get; private set; }
        private float timer;
        private float startTime;

        public void SetupStateMachine(Dictionary<Type, BaseState> states, float updateRate)
        {
            availableStates = states;
            startTime = updateRate;
        }

        private void Update()
        {
            //if (timer > 0f)
            //{
            //    timer -= Time.deltaTime;
            //}
            //else
            //{
            //    UpdateStateMachine();
            //    timer = startTime;
            //}
            UpdateStateMachine();
        }

        private void UpdateStateMachine()
        {
            if (currentState == null)
            {
                currentState = availableStates.Values.First();
                currentState.Enter();
            }

            var nextState = currentState?.Update(); //if currentState != null call Update() which return a Type of state

            if (nextState != null && nextState != currentState?.GetType())
            {
                SwitchState(nextState);
            }
        }

        private void SwitchState(Type nextState)
        {
            currentState.Exit();
            currentState = availableStates[nextState];
            currentState.Enter();
        }

    }
}
