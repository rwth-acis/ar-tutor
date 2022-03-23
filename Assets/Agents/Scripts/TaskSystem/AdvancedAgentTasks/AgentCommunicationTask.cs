using UnityEngine;
// IEnumerator
using System.Collections;
// Tasks
using System.Collections.Generic;
using VirtualAgentsFramework.AgentTasks;
// TextMeshPro
using TMPro;

namespace VirtualAgentsFramework
{
    namespace AgentTasks
    {
        /// <summary>
        /// Define tasks for communication
        /// </summary>
        public class AgentCommunicationTask : AgentLazyTask
        {
            private Agent agent = null;
            private GameObject tableau = null;
            private string message = "";

            // Constructor in case of a textual tableau as the means of communication
            public AgentCommunicationTask(GameObject tableau, string message)
            {
                this.tableau = tableau;
                this.message = message;
            }

            public override void Execute(Agent agent)
            {
                this.agent = agent;
                tableau.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = message;
                Debug.Log("Communicated: " + message);
                if (message == "")
                    tableau.SetActive(false);
                else
                    tableau.SetActive(true);
                // Trigger the TaskFinished event
                FinishTask();
            }
        }
    }
}
