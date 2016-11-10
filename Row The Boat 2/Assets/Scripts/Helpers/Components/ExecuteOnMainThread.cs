namespace Assets.Scripts.Helpers.Components
{
    using System;
    using System.Collections.Generic;

    public class ExecuteOnMainThread : BaseClass
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
