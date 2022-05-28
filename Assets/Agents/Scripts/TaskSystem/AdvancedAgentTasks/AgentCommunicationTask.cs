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
// Button
using UnityEngine.UI;

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
            private string target = "";
            private string source = "";
            private bool done = false;

            public event Action OnTaskFinished;

            // Constructor in case of a textual tableau as the means of communication
            public AgentCommunicationTask(GameObject tableau, string target, string source, bool done)
            {
                this.tableau = tableau;
                this.target = target;
                this.source = "(" + source + ")";
                this.done = done;
            }

            public void Execute(Agent agent)
            {
                this.agent = agent;
                // Image child + TMP child
                GameObject background = tableau.transform.GetChild(0).gameObject;
                RectTransform backgroundRT = background.GetComponent<RectTransform>();
                TextMeshProUGUI TMP_target = tableau.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI TMP_source = tableau.transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                GameObject button = tableau.transform.GetChild(0).transform.GetChild(2).gameObject;

                TMP_target.text = target;
                TMP_source.text = source;
                //Debug.Log("TMP value: " + TMP_Text.GetPreferredValues().ToString());
                //backgroundRT.sizeDelta = new Vector2(TMP_Text.GetPreferredValues().x + 10, backgroundRT.sizeDelta.y);
                Debug.Log("Communicated: " + target);
                /*
                if (target == "")
                    background.SetActive(false);
                else
                */
                    background.SetActive(true);

                // Trigger the TaskFinished event
                //agent.StartCoroutine(FinishTaskCoroutine(0.1f));
                if (done == true)
                {
                    button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Done";
                }
                else
                {
                    button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Show me";
                }
                button.GetComponent<Button>().onClick.AddListener(delegate { FinishTask(); });
            }

            private void FinishTask()
            {
                tableau.transform.GetChild(0).gameObject.SetActive(false);
                // Trigger the TaskFinished event
                OnTaskFinished();
            }

            public void Update() { }

            private IEnumerator FinishTaskCoroutine(float waitingTime)
            {
                yield return new WaitForSeconds(waitingTime);
                // Set background inactive
                tableau.transform.GetChild(0).gameObject.SetActive(false);
                // Trigger the TaskFinished event
                OnTaskFinished();
            }
        }
    }
}
