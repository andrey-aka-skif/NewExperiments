﻿using UnityEngine;

namespace SteelHeart.Interactable
{
    public class MiddleIdleState : BaseLiftState
    {
        public MiddleIdleState(IStateSwitcher switcher,
                               float speed,
                               Transform upPoint,
                               Transform downPoint,
                               Transform platform)
            : base(switcher, speed, upPoint, downPoint, platform)
        {
        }

        public override void Update() { }
    }
}