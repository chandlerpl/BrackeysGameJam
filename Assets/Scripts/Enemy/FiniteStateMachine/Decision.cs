using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CP.AILibrary.FiniteStateMachine
{
    public abstract class Decision : ScriptableObject
    {
        public abstract bool Decide(StateMachine stateMachine);
    }
}
