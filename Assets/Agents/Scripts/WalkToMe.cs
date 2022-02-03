using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;

using VirtualAgentsFramework;
using VirtualAgentsFramework.AgentTasks;

public class WalkToMe : MonoBehaviour
{
    [SerializeField] Agent agent;
    [SerializeField] NavMeshSurface surface;
    // Start is called before the first frame update
    void Start()
    {
        // Update the NavMesh
        surface.BuildNavMesh();
        // Schedule a task for the assigned agent to walk to this game object
        agent.WalkTo(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
