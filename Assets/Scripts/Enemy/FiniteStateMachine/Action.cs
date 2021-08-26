using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CP.AILibrary.FiniteStateMachine
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Enter(StateMachine stateMachine);

        public abstract void Act(StateMachine stateMachine);

        public abstract void Exit(StateMachine stateMachine);
    }
}
