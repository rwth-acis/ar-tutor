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
    /// <summary>
    /// Agent's functionality mainly includes managing their task queue,
    /// responding to task execution statuses and changing one's state accordingly
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))] // Responsible for the proxy object's movement
    [RequireComponent(typeof(ThirdPersonCharacter))] // Responsible for the avatar's movement
    [RequireComponent(typeof(AgentAbilities))] //BA Stores the list of agent's abilities
    public class Agent : MonoBehaviour
    {
        private NavMeshAgent agent;

        //BA List of the agent's abilities
        private AgentAbilities abilities;

        //BA Communication ability
        private GameObject communicationTableau;

        /// <summary>
        /// Agent's personal task queue
        /// </summary>
        private AgentTaskManager queue = new AgentTaskManager();

        /// <summary>
        /// States an agent can be in
        /// </summary>
        public enum State
        {
            inactive, // i.e. requesting new tasks is disabled
            idle, // i.e. requesting new tasks is enabled
            busy // i.e. currently executing a task
        }

        /// <summary>
        /// Agent's current state
        /// </summary>
        private State currentState;

        /// <summary>
        /// Agent's current task
        /// </summary>
        private IAgentTask currentTask;

        /// <summary>
        /// Set up the agent
        /// </summary>
        void Start()
        {
            // Get the agent's NavMeshAgent component
            agent = GetComponent<NavMeshAgent>();
            //BA Fetch the agent's abilities
            abilities = GetComponent<AgentAbilities>();
            //BA Get communication tableau
            communicationTableau = GameObject.Find("CommunicationTableau");
            // Disable NavMeshAgent's rotation updates, since rotation is handled by ThirdPersonCharacter
            agent.updateRotation = false;
            // Make the agent start in the idle state in order to enable requesting new tasks
            // CHANGE_ME to inactive in order to disable requesting new tasks
            currentState = State.inactive;
            //BA Signal to all components that an agent with certain abilities has been instantiated
            StartCoroutine(SignalAgentInstantiation(1f));
        }

        // BA cotoutine test
        private IEnumerator SignalAgentInstantiation(float waitingTime)
        {
            yield return new WaitForSeconds(waitingTime);
            //BA Communication test
            //Communicate("I am alive!");
            EventManager.AgentInstantiated(this, abilities);
            EventManager.PostStatement("Environment", "instantiated", "agent");
        }

        /// <summary>
        /// Enable the right mode depending on the agent's status
        /// </summary>
        void Update()
        {
            switch(currentState)
            {
                case State.inactive: // do nothing
                    break;
                case State.idle:
                    RequestNextTask(); // request new tasks
                    break;
                case State.busy:
                    currentTask.Update(); // perform frame-to-frame updates required by the current task
                    break;
            }
        }

        /// <summary>
        /// React to the agent's Animator component's event that gets triggered
        /// when an animation finishes playing and return the agent's animation to idle
        /// </summary>
        public void ReturnToIdle()
        {
            AgentAnimationTask currentAnimationTask = (AgentAnimationTask)currentTask;
            currentAnimationTask.ReturnToIdle();
        }

        /// <summary>
        /// Schedule a task
        /// </summary>
        /// <param name="task">Task to be scheduled</param>
        public void ScheduleTask(IAgentTask task)
        {
            queue.AddTask(task);
        }

        /// <summary>
        /// Execute a task as soon as possible
        /// </summary>
        /// <param name="task">Task to be executed</param>
        public void ExecuteTaskASAP(IAgentTask task)
        {
            queue.ForceTask(task);
        }

        /// <summary>
        /// Request the next task from the agent's task queue
        /// </summary>
        private void RequestNextTask()
        {
            IAgentTask nextTask = queue.RequestNextTask();
            if(nextTask == null)
            {
                // The queue is empty, thus change the agent's current state to idle
                currentState = State.idle;
            }
            else
            {
                // The queue is not empty, thus...
                // change the agent's current state to busy,
                currentState = State.busy;
                // execute the next task,
                nextTask.Execute(this);
                // save the current task,
                currentTask = nextTask;
                // subscribe to the task's OnTaskFinished event to set the agent's state to idle after task execution
                currentTask.OnTaskFinished += SetAgentStateToIdle;
            }
        }

        /// <summary>
        /// Helper function to be called when a task has been executed.
        /// Set agent's state to idle and unsubscribe from the current task's OnTaskFinished event
        /// </summary>
        private void SetAgentStateToIdle()
        {
            currentState = State.idle;
            // Unsubscribe from the event
            currentTask.OnTaskFinished -= SetAgentStateToIdle;
        }

        /// <summary>
        /// Set agent's state
        /// </summary>
        public void SetAgentState(State newState)
        {
            currentState = newState;
        }

        /// <summary>
        /// Returns agent's queue
        /// </summary>
        public AgentTaskManager GetQueue()
        {
            return queue;
        }

        public void SetQueue(AgentTaskManager queue)
        {
            this.queue = queue;
        }

        /// <summary>
        /// Creates an AgentMovementTask for walking and schedules it or forces its execution.
        /// Shortcut queue management function
        /// </summary>
        /// <param name="destinationObject">GameObject the agent should walk to</param>
        /// <param name="asap">true if the task should be executed as soon as possible, false if the task should be scheduled</param>
        public void WalkTo(GameObject destinationObject, bool asap = false)
        {
            AgentMovementTask movementTask = new AgentMovementTask(destinationObject);
            ScheduleOrForce(movementTask, asap);
        }

        /// <summary>
        /// Creates an AgentMovementTask for walking and schedules it or forces its execution.
        /// Shortcut queue management function
        /// </summary>
        /// <param name="destinationCoordinates">Position the agent should walk to</param>
        /// <param name="asap">true if the task should be executed as soon as possible, false if the task should be scheduled</param>
        public void WalkTo(Vector3 destinationCoordinates, bool asap = false)
        {
            AgentMovementTask movementTask = new AgentMovementTask(destinationCoordinates);
            ScheduleOrForce(movementTask, asap);
        }

        /// <summary>
        /// Creates an AgentMovementTask for running and schedules it or forces its execution.
        /// Shortcut queue management function
        /// </summary>
        /// <param name="destinationObject">GameObject the agent should run to</param>
        /// <param name="asap">true if the task should be executed as soon as possible, false if the task should be scheduled</param>
        public void RunTo(GameObject destinationObject, bool asap = false)
        {
            AgentMovementTask movementTask = new AgentMovementTask(destinationObject, true);
            ScheduleOrForce(movementTask, asap);
        }

        /// <summary>
        /// Creates an AgentMovementTask for running and schedules it or forces its execution.
        /// Shortcut queue management function
        /// </summary>
        /// <param name="destinationCoordinates">Position the agent should run to</param>
        /// <param name="asap">true if the task should be executed as soon as possible, false if the task should be scheduled</param>
        public void RunTo(Vector3 destinationCoordinates, bool asap = false)
        {
            AgentMovementTask movementTask = new AgentMovementTask(destinationCoordinates, true);
            ScheduleOrForce(movementTask, asap);
        }

        /// <summary>
        /// Creates an AgentAnimationTask and schedules it or forces its execution.
        /// Shortcut queue management function
        /// </summary>
        /// <param name="animationName">Name of the animation to be played</param>
        /// <param name="asap">true if the task should be executed as soon as possible, false if the task should be scheduled</param>
        public void PlayAnimation(string animationName, bool asap = false)
        {
            AgentAnimationTask animationTask = new AgentAnimationTask(animationName);
            ScheduleOrForce(animationTask, asap);
        }

        /// <summary>
        /// Creates an AgentWaitingTask and schedules it or forces its execution.
        /// Shortcut queue management function
        /// </summary>
        /// <param name="secondsWaiting">Number of seconds the agent should wait</param>
        /// <param name="asap">true if the task should be executed as soon as possible, false if the task should be scheduled</param>
        public void WaitForSeconds(float secondsWaiting, bool asap = false)
        {
            AgentWaitingTask waitingTask = new AgentWaitingTask(secondsWaiting);
            ScheduleOrForce(waitingTask, asap);
        }

        /// <summary>
        /// Creates an AgentCommunicationTask and schedules it or forces its execution.
        /// Shortcut queue management function
        /// </summary>
        /// <param name="message">Message that should be communicated</param>
        /// <param name="asap">true if the task should be executed as soon as possible, false if the task should be scheduled</param>
        public void Communicate(string target, string source, bool done = false, bool asap = false)
        {
            AgentCommunicationTask task = new AgentCommunicationTask(communicationTableau, target, source, done);
            Debug.Log("Scheduled a communication task on " + communicationTableau.ToString() + ", with message: " + source);
            ScheduleOrForce(task, asap);
        }

        //TODO
        public void PressOn(Vector3 destinationCoordinates, bool asap = false)
        {
            //AgentPressingTask pressingTask = new AgentPressingTask(destinationCoordinates);
            //ScheduleOrForce(pressingTask, asap);
        }

        [SerializeField] Rig twist;
        [SerializeField] Rig leftArmStretch;
        [SerializeField] GameObject stretchTarget;
        /// <summary>
        /// Creates an AgentPointingComplexTask and schedules it or forces its execution.
        /// Shortcut queue management function
        /// </summary>
        /// <param name="destinationObject">Object to be pointed to</param>
        /// <param name="procedural">true if the animation should be computed, false if the animation should be played</param>
        /// <param name="asap">true if the task should be executed as soon as possible, false if the task should be scheduled</param>
        public void PointTo(GameObject destinationObject, bool procedural = true, bool asap = false)
        {
            /*if(procedural)
            {*/
                //AgentPointingTask pointingTask = new AgentPointingTask(destinationObject, twist, leftArmStretch, stretchTarget);
                AgentPointingComplexTask pointingTask = new AgentPointingComplexTask(destinationObject, twist, leftArmStretch, stretchTarget);
            /*}
            else
            {
                // Create a simple animation task
            }*/
            ScheduleOrForce(pointingTask, asap);
        }

        //TODO
        public void PickUp(GameObject destinationObject, GameObject rigTarget, bool asap = false)
        {
            rigTarget.transform.position = destinationObject.transform.position;
            PlayAnimation("PickingUp", asap);
        }

        /// <summary>
        /// Creates an AgentRotationTask and schedules it or forces its execution.
        /// Shortcut queue management function
        /// </summary>
        /// <param name="rotation">Position, towards which an agent should rotate</param>
        /// <param name="asap">true if the task should be executed as soon as possible, false if the task should be scheduled</param>
        public void RotateTowards(Vector3 rotation, bool asap = false)
        {
            AgentRotationTask rotationTask = new AgentRotationTask(rotation);
            ScheduleOrForce(rotationTask, asap);
        }

        public void RotateRelative(Vector3 to, bool left = true, bool asap = false)
        {
            AgentRotationTask rotationTask = new AgentRotationTask(Vector3.zero, to, left);
            ScheduleOrForce(rotationTask, asap);
        }


        public void RotateToBeRight(Vector3 from, bool asap = false)
        {
            Vector3 delta = (from - gameObject.transform.position).normalized;
            Vector3 cross = Vector3.Cross(delta, gameObject.transform.forward);

            if (cross == Vector3.zero)
            {
                Debug.Log("Target is straight ahead");
            }
            else if (cross.y > 0)
            {
                Debug.Log("Target is to the right");
            }
            else
            {
                Debug.Log("Target is to the left");
            }

            /*if (cross == Vector3.zero)
            {
                Debug.Log("Target is to the right");
            }
            else if (cross.y > 0)
            {
                Debug.Log("Target is behind");
            }
            else
            {
                Debug.Log("Target is in front");
            }*/

            Vector3 rightCross = Vector3.Cross(delta, gameObject.transform.right);

            AgentRotationTask rotationTask = new AgentRotationTask(rightCross);
        }

        public void RotateToBeLeft(Vector3 from, bool asap = false)
        {
            Vector3 delta = (from - gameObject.transform.position).normalized;
            Vector3 cross = Vector3.Cross(delta, -gameObject.transform.right);

            /*if (cross == Vector3.zero)
            {
                Debug.Log("Target is to the left");
            }
            else if (cross.y > 0)
            {
                Debug.Log("Target is in front");
            }
            else
            {
                Debug.Log("Target is behind");
            }*/

            AgentRotationTask rotationTask = new AgentRotationTask(cross);
        }

        /// <summary>
        /// Helper function for shortcut queue management functions.
        /// Schedule a task or force its execution depending on the flag
        /// </summary>
        /// <param name="task">Task to be scheduled or forced</param>
        /// <param name="force">Flag: true if the task's execution should be forced, false if the task should be scheduled</param>
        public void ScheduleOrForce(IAgentTask task, bool force)
        {
            if(force == true)
            {
                queue.ForceTask(task);
            }
            else
            {
                queue.AddTask(task);
            }
        }
    }
}
