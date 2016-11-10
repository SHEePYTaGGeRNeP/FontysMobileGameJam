namespace Assets.Scripts.Helpers.Components
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    public class ExecuteOnMainThread : MonoBehaviour
    {
        public static readonly Queue<Action> ActionsToExecute = new Queue<Action>();

        public void Update()
        {
            // dispatch stuff on main thread
            while (ActionsToExecute.Count > 0)
            {
                ActionsToExecute.Dequeue().Invoke();
            }
        }
    }
}
