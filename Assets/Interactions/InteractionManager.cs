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
            // Checks whether the agent has matching abilities. Potentially returns respective interaction interfaces (capacities) of the agent
            //TODO Pluralize
            var abilitySatisfied = abilities.CheckAbility(interaction.requiredAbility.flag);
            var affordanceSatisfied = CheckAffordance(smartObjectInstance.smartObject, interaction);
            if (abilitySatisfied && affordanceSatisfied)
            {
                Debug.Log("Interaction stage 3");
                interaction.Perform(agent, smartObjectInstance);
                Debug.Log("Interaction stage 4");
            }
        }
    }

    // Checks whether the smart object affords what is required by the interaction
    // Considers provided interfaces of the smart object as well as the state it is in
    public static bool CheckAffordance (SmartObject smartObject, Interaction interaction)
    {
        if(smartObject.CheckState())
            return false;
        else if(smartObject.CheckAffordance(interaction.requiredAffordance))
            return true;
        else
        return false;
    }
}
