﻿using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CP.AILibrary.FiniteStateMachine
{
    [CreateAssetMenu(fileName = "State", menuName = "CP/FSM")]
    public class State : ScriptableObject
    {
        public Action[] actions;
        public Transition[] transitions;

        public void EnterState(StateMachine stateMachine)
        {
            foreach (Action action in actions)
            {
                action.Enter(stateMachine);
            }
        }

        public void UpdateState(StateMachine stateMachine)
        {
            foreach (Action action in actions)
            {
                action.Act(stateMachine);
            }

            foreach (Transition transition in transitions)
            {
                stateMachine.CurrentState = transition.decision.Decide(stateMachine) ? transition.trueState : transition.falseState;
            }
        }

        public void ExitState(StateMachine stateMachine)
        {
            foreach (Action action in actions)
            {
                action.Exit(stateMachine);
            }
        }

    }

}
