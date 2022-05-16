using UnityEngine;

public class PlayStopHandler : MonoBehaviour
{
    bool play = true;

    [SerializeField]
    GameObject m_UI;

    [SerializeField]
    NavMeshManager navMeshManager;

    public GameObject ui
    {
        get => m_UI;
        set => m_UI = value;
    }

    public void TogglePlayStop()
    {
        if (play == true)
        {
            navMeshManager.PlayAgentTasks();
        }
        else
        {
            navMeshManager.StopAgentTasks();
        }
        m_UI.SetActive(!m_UI.activeSelf);
        play = !play;
    }

    public void StopAgent()
    {
        if (play == true)
        {
            TogglePlayStop();
        }
    }
}
