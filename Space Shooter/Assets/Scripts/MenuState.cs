using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter.States
{
    public class MenuState : GameStateBase
    {
        public override string SceneName
        {
            get { return "MainMenu"; }
        }

        public override GameStateType StateType
        {
            get { return GameStateType.MainMenu; }
        }

        public MenuState()
        {
            AddTargetState(GameStateType.Level1);
        }

        public override void Activate()
        {
            base.Activate();

            GameManager.Instance.Reset();
        }
    }
}
