using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

namespace CombatGame.Util
{
    public static class VectorHelper
    {
        public static Vector3 GetDirectionIgnoreY(Vector3 source, Vector3 target)
        {
            var myPosition = source;
            var targetPosition = target;
            //We want to ignore Y positions as it can cause calculation error
            myPosition.y = 0;
            targetPosition.y = 0;

            var targetDirection = (targetPosition - myPosition).normalized;
            return targetDirection;
        }
    }
}