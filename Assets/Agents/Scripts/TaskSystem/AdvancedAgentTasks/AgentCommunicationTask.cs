using UnityEngine;
// IEnumerator
using System.Collections;
// Tasks
using System.Collections.Generic;
using VirtualAgentsFramework.AgentTasks;
// Action
using System;
// TextMeshPro
using TMPro;

namespace VirtualAgentsFramework
{
    namespace AgentTasks
    {
        /// <summary>
        /// Define tasks for communication
        /// </summary>
        public class AgentCommunicationTask : IAgentTask
        {
            private Agent agent = null;
            private GameObject tableau = null;
            private string message = "";

            public event Action OnTaskFinished;

            // Constructor in case of a textual tableau as the means of communication
            public AgentCommunicationTask(GameObject tableau, string message)
            {
                this.tableau = tableau;
                this.message = message;
            }

            public void Execute(Agent agent)
            {
                this.agent = agent;
                // Image child + TMP child
                GameObject background = tableau.transform.GetChild(0).gameObject;
                RectTransform backgroundRT = background.GetComponent<RectTransform>();
                TextMeshProUGUI TMP_Text = tableau.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();

                TMP_Text.text = message;
                Debug.Log("TMP value: " + TMP_Text.GetPreferredValues().ToString());
                backgroundRT.sizeDelta = new Vector2(TMP_Text.GetPreferredValues().x + 10, backgroundRT.sizeDelta.y);
                Debug.Log("Communicated: " + message);
                if (message == "")
                    background.SetActive(false);
                else
                    background.SetActive(true);

                // Trigger the TaskFinished event
                agent.StartCoroutine(FinishTaskCoroutine(0.1f));
            }

            public void Update() { }

            private IEnumerator FinishTaskCoroutine(float waitingTime)
            {
                yield return new WaitForSeconds(waitingTime);
                // Trigger the TaskFinished event
                OnTaskFinished();
            }
        }
    }
}
