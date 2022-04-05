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
        /// Define rotation tasks for making the agents rotate towards a certain point in space
        /// </summary>
        public class AgentRotationTask : IAgentTask
        {
            private Agent agent;
            private NavMeshAgent navMeshAgent;
            private ThirdPersonCharacter thirdPersonCharacter;
            private Animator animator;
            private Vector3 rotation;

            private float curSpeed;
            private Vector3 previousRotation;
            private const float damping = 20;

            Vector3 targetPosition, targetPoint, direction;
            Quaternion lookRotation;
            float turnAmount;

            private const float frameTimer = 100; //TODO make this obsolete using subtasks
            private float frameCount = 0;

            public event Action OnTaskFinished;

            // For checking where the target position is relative to the agent
            bool checkLeftRight = false;
            Vector3 from;
            bool left;


            public AgentRotationTask(Vector3 rotation, Vector3? from = null, bool? left = null)
            {
                this.rotation = rotation;
                if (from != null)
                {
                    this.checkLeftRight = true;
                    this.from = from ?? Vector3.zero;
                    Debug.Log("From: " + from.ToString());
                    this.left = left ?? false;
                }
            }

            public void Execute(Agent agent)
            {
                this.agent = agent;
                navMeshAgent = agent.GetComponent<NavMeshAgent>();
                thirdPersonCharacter = agent.GetComponent<ThirdPersonCharacter>();
                animator = agent.GetComponent<Animator>();

                if (this.checkLeftRight == true)
                {
                    Vector3 delta = (from - agent.gameObject.transform.position).normalized;
                    Vector3 cross = -Vector3.Cross(delta, agent.gameObject.transform.forward); // - because of the Unity's left-handed coordinate system

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
                    Debug.Log("cross: " + cross.ToString());

                    if (left == true)
                    {
                        cross = -Vector3.Cross(delta, -agent.gameObject.transform.right);
                        Debug.Log("cross left: " + cross.ToString());
                    }
                    else
                    {
                        cross = -Vector3.Cross(delta, agent.gameObject.transform.right);
                        Debug.Log("cross right: " + cross.ToString());
                    }

                    this.rotation = cross * 5; // Magnification needed for the current rotation task implementation to function properly
                }
            }

            public void Update()
            {
                // Calculate actual speed
                Vector3 curRotation = agent.transform.rotation.eulerAngles - previousRotation;
                curSpeed = curRotation.magnitude / Time.deltaTime;
                previousRotation = agent.transform.rotation.eulerAngles;

                if(curSpeed > 0f)
                {
                    targetPosition = rotation;
                    targetPoint = new Vector3(targetPosition.x, agent.transform.position.y, targetPosition.z);
                    direction = (targetPoint - agent.transform.position).normalized;
                    lookRotation = Quaternion.LookRotation(direction);

                    turnAmount = Mathf.Atan2(targetPoint.x, targetPoint.z);
                    agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, lookRotation, 1);
                    animator.SetFloat("Turn", -turnAmount * curSpeed / damping, 0.1f, Time.deltaTime);
                    /*navMeshAgent.SetDestination(targetPoint);
                    if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                    {
                        thirdPersonCharacter.Move(navMeshAgent.desiredVelocity, false, false);
                    }*/
                    //Debug.Log(curSpeed);
                }
                else
                {
                    animator.SetFloat("Turn", 0f, 0.1f, Time.deltaTime);
                    // Trigger the TaskFinished event
                    if(frameCount == frameTimer) //TODO replace this with a waiting subtask for the same amount of time as the Animator's dampTime
                    {
                        OnTaskFinished();
                    }
                    else
                    {
                        frameCount++;
                    }
                    //agent.StartCoroutine(FinishAnimation());
                }
            }

            private IEnumerator FinishAnimation()
            {
                animator.SetFloat("Turn", 0f, 0.1f, Time.deltaTime);
                // Wait for the current animation to finish
                Debug.Log(animator.IsInTransition(0));
                Debug.Log(animator.GetCurrentAnimatorStateInfo(0));
                while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Grounded"))
                {
                    yield return null;
                }
                Debug.Log("task finished");
                OnTaskFinished();
            }
        }
    }
}
