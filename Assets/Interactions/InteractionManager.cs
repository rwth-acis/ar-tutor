using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualAgentsFramework;

public static class InteractionManager
{
    public static void AttemptInteraction (Agent agent, AgentAbilities abilities, Interaction interaction, SmartObject smartObject)
    {
        if (agent != null && interaction != null && smartObject != null)
        {
            // Checks whether the agent has matching abilities. Potentially returns respective interaction interfaces (capacities) of the agent
            //TODO Pluralize
            var abilitySatisfied = abilities.CheckAbility(interaction.requiredAbility.flag);
            var affordanceSatisfied = CheckAffordance(smartObject, interaction);
            if (abilitySatisfied && affordanceSatisfied)
            {
                interaction.Perform(agent, smartObject);
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
