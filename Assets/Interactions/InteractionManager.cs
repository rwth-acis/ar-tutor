using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAgentsFramework;

public static class InteractionManager
{
    public static void AttemptInteraction (Agent agent, AgentAbilities abilities, Interaction interaction, SmartObjectInstance smartObjectInstance)
    {
        Debug.Log("Interaction stage 1");
        if (agent != null && interaction != null && smartObjectInstance != null)
        {
            Debug.Log("Interaction stage 2");
            // Checks whether the agent has matching abilities.
			// TODO potentially, this method should return respective interaction interfaces (capacities) of the agent
            var abilitySatisfied = abilities.CheckAbilities(interaction.requiredAbilities);
            // Checks whether the Smart Object instance has matching affordances.
			// TODO potentially, this method should return respective interaction interfaces (capacities) of the Smart Object instance
            var affordanceSatisfied = smartObjectInstance.CheckAffordances(interaction.requiredAffordances);
            if (abilitySatisfied && affordanceSatisfied)
            {
                Debug.Log("Interaction stage 3");
                interaction.Perform(agent, smartObjectInstance);
                Debug.Log("Interaction stage 4");
            }
        }
    }
}
