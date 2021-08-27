using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using CP.AILibrary.Storage;

namespace CP.AILibrary.FiniteStateMachine
{
    public class StateMachine : MonoBehaviour
    {
        public State startingState;
        public Memory memory;
        public State CurrentState
        {
            get => currentState;
            set
            {
                if(value != null)
                {
                    if(currentState != null)
                        currentState.ExitState(this);
                    currentState = value;
                    currentState.EnterState(this);
                }
            }
        }
        private State currentState;
        private bool isActive = true;

        public void Start()
        {
            currentState = startingState;

            currentState.EnterState(this);
        }

        public void Update()
        {
            if (!isActive)
                return;

            if (currentState == null)
            {
                isActive = false;
                return;
            }

            currentState.UpdateState(this);
        }
    }
}
