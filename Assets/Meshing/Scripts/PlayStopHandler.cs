﻿using UnityEngine;
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
        // If the agent is not there while interactions are being played
        if (!navMeshManager.IsAgentSet() && play == false)
        {
            play = true;
            m_UI.GetComponent<Image>().sprite = playSprite;
            EventManager.PostStatement("system", "pressed", "stop");
            return;
        }

        // If the agent is not there, tasks can not be controlled
        if (!navMeshManager.IsAgentSet())
            return;

        if (play == true)
        {
            navMeshManager.PlayAgentTasks();
            m_UI.GetComponent<Image>().sprite = stopSprite;
            EventManager.PostStatement("user", "pressed", "play");
        }
        else
        {
            navMeshManager.StopAgentTasks();
            m_UI.GetComponent<Image>().sprite = playSprite;
            EventManager.PostStatement("user", "pressed", "stop");
        }
        //m_UI.SetActive(!m_UI.activeSelf);
        play = !play;
    }
}
