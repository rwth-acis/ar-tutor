using UnityEngine;
// NavMesh
using UnityEngine.AI;
// Third person animation
using UnityStandardAssets.Characters.ThirdPerson;
// IEnumerator
using System.Collections;
// Tasks
using System.Collections.Generic;
using VirtualAgentsFramework.AgentTasks;
// Action
using System;
// Rigs
using UnityEngine.Animations.Rigging;

namespace VirtualAgentsFramework
{
    namespace AgentTasks
    {
        /// <summary>
        /// Common methods and attributes for all AgentTasks
        /// </summary>
        public interface IAgentTask
        {
            // Get the agent's data, prepare for and start task execution
            void Execute(Agent agent);
            // Perform frame-to-frame task execution
            void Update();
            // Fire when the task is finished to let the agent know
            // !!! This event cannot be triggered inside Execute().
            // Recommendation: trigger it from a coroutine or Update()
            event Action OnTaskFinished;
        }

        /// <summary>
        /// Makes it possible to avoid implementing Update()
        /// and Execute() functions when they are not needed.
        /// Use the override keyword if you decide to implement these functions
        /// </summary>
        public abstract class AgentLazyTask : IAgentTask
        {
            // Get the agent's data, prepare for and start task execution
            public virtual void Execute(Agent agent) { } // !!! Use the override keyword if you implement this function
            // Perform frame-to-frame task execution
            public virtual void Update() { } // !!! Use the override keyword if you implement this function
            // Fire when the task is finished to let the agent know
            public event Action OnTaskFinished;
            // !!! This function cannot be called inside Execute(). Recommendation: use a coroutine
            protected virtual void FinishTask() // Helper function since the event itself cannot be called
                                                // in derived classes (https://stackoverflow.com/a/31661451)
            {
                OnTaskFinished();
            }

            /*
            // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/events/how-to-raise-base-class-events-in-derived-classes
            // The event. Note that by using the generic EventHandler<T> event type
            // we do not need to declare a separate delegate type.
            public event EventHandler<ShapeEventArgs> OnTaskFinished;

            //The event-invoking method that derived classes can override.
            protected virtual void OnShapeChanged(ShapeEventArgs e)
            {
                // Safely raise the event for all subscribers
                ShapeChanged?.Invoke(this, e);
            }*/

            /*public delegate void ProgressStartedType(string title, int total);

            // Raised when progress on a potentially long running process is started.
            public event ProgressStartedType ProgressStarted;

            // Used from derived classes to raise ProgressStarted.
            protected void RaiseProgressStarted(string title, int total)
            {
                if (ProgressStarted != null) ProgressStarted(title, total);
            }*/
        }

        /// <summary>
        /// An internal AgentTask subtask-queue in addition to
        /// the universal AgentTask methods and attributes
        /// </summary>
        public abstract class AgentComplexTask : IAgentTask
        {
            /// <summary>
            /// Reference to a corresponding agent
            /// <summary>
            protected Agent agent;

            /// <summary>
            /// Fire when the task is finished to let the agent know.
            /// The OnTaskFinished event itself cannot be called directly,
            /// so use the ScheduleTaskTermination() method instead
            /// <summary>
            public event Action OnTaskFinished;

            /// <summary>
            /// Any AgentTask can be scheduled as a subtask, including complex AgentTasks
            /// <summary>
            protected AgentTaskManager subTaskQueue;

            /// <summary>
            /// Current subtask execution state
            /// <summary>
            protected State currentState;

            /// <summary>
            /// Task's current subtask
            /// </summary>
            protected IAgentTask currentSubTask;

            /// <summary>
            /// Signalizes when to prepare for task termination
            /// </summary>
            protected bool finishFlag = false;

            /// <summary>
            /// Request the next subtask from the task's subtask queue
            /// </summary>
            protected void RequestNextSubTask()
            {
                IAgentTask nextSubTask = subTaskQueue.RequestNextTask();
                if(nextSubTask == null)
                {
                    // The queue is empty, thus change the agent's current state to idle
                    currentState = State.idle;
                    if (finishFlag) { OnTaskFinished(); }
                }
                else
                {
                    // The queue is not empty, thus...
                    // change the agent's current state to busy,
                    currentState = State.busy;
                    // execute the next task,
                    nextSubTask.Execute(agent);
                    // save the current task,
                    currentSubTask = nextSubTask;
                    // subscribe to the task's OnTaskFinished event to set the agent's state to idle after task execution
                    currentSubTask.OnTaskFinished += OnSubTaskFinished;
                }
            }

            /// <summary>
            /// Helper function to be called when a subtask has been executed.
            /// Set task's state to idle and unsubscribe from the terminated subtask's OnTaskFinished event
            /// </summary>
            protected void OnSubTaskFinished()
            {
                currentState = State.idle;
                // Unsubscribe from the event
                currentSubTask.OnTaskFinished -= OnSubTaskFinished;
            }

            /// </summary>
            /// The OnTaskFinished event itself cannot be called directly,
            /// so use this method instead
            /// </summary>
            protected void ScheduleTaskTermination()
            {
                finishFlag = true;
            }

            /// </summary>
            /// Get the agent's data, prepare for subtask scheduling and execution.
            /// If you override this function, you might still want to call it
            /// using base.Execute(yourAgent)
            /// </summary>
            public virtual void Execute(Agent agent)
            {
                this.agent = agent;

                subTaskQueue = new AgentTaskManager(); // IMPORTANT for complex tasks
                currentState = State.idle;
            }

            /// </summary>
            /// Perform frame-to-frame subtask management and execution.
            /// If you override this function, you still want to call it
            /// using base.Update() for subtask management
            /// </summary>
            public virtual void Update()
            {
                switch(currentState)
                {
                    case State.inactive: // do nothing
                        break;
                    case State.idle:
                        RequestNextSubTask(); // request new tasks
                        break;
                    case State.busy:
                        currentSubTask.Update(); // perform frame-to-frame updates required by the current task
                        break;
                }
            }
        }

        /// <summary>
        /// Possible subtask execution states
        /// </summary>
        public enum State
        {
            inactive, // i.e. requesting new subtasks is disabled
                      // (for testing purposes or basic tasks)
            idle, // i.e. requesting new subtasks is enabled
            busy // i.e. currently executing a subtask
        }
    }
}
