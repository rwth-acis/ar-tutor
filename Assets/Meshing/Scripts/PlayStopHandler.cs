using UnityEngine;
using UnityEngine.UI;

public class PlayStopHandler : MonoBehaviour
{
    bool play = true;

    [SerializeField]
    GameObject m_UI;

    [SerializeField]
    Sprite m_PlaySprite;

    [SerializeField]
    Sprite m_StopSprite;

    [SerializeField]
    NavMeshManager navMeshManager;

    public GameObject ui
    {
        get => m_UI;
        set => m_UI = value;
    }

    public Sprite playSprite
    {
        get => m_PlaySprite;
        set => m_PlaySprite = value;
    }

    public Sprite stopSprite
    {
        get => m_StopSprite;
        set => m_StopSprite = value;
    }

    public void TogglePlayStop()
    {
        if (play == true)
        {
            navMeshManager.PlayAgentTasks();
            m_UI.GetComponent<Image>().sprite = stopSprite;
        }
        else
        {
            navMeshManager.StopAgentTasks();
            m_UI.GetComponent<Image>().sprite = playSprite;
        }
        //m_UI.SetActive(!m_UI.activeSelf);
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
