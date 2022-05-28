using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_IOS
using UnityEngine.XR.ARKit;
#endif // UNITY_IOS
using UnityEngine.XR.ARSubsystems;

public class ClassificationPlacementManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_FloorPrefabs;

    public List<GameObject> floorPrefabs
    {
        get => m_FloorPrefabs;
        set => m_FloorPrefabs = value;
    }

    [SerializeField]
    List<GameObject> m_TablePrefabs;

    public List<GameObject> tablePrefabs
    {
        get => m_TablePrefabs;
        set => m_TablePrefabs = value;
    }

    [SerializeField]
    List<GameObject> m_WallPrefabs;

    public List<GameObject> wallPrefabs
    {
        get => m_WallPrefabs;
        set => m_WallPrefabs = value;
    }

    [SerializeField]
    MeshClassificationManager m_ClassificationManager;

    public MeshClassificationManager classificationManager
    {
        get => m_ClassificationManager;
        set => m_ClassificationManager = value;
    }

    [SerializeField]
    PlacementReticle m_Reticle;

    public PlacementReticle reticle
    {
        get => m_Reticle;
        set => m_Reticle = value;
    }

    [SerializeField]
    GameObject m_FloorUI;

    public GameObject floorUI
    {
        get => m_FloorUI;
        set => m_FloorUI = value;
    }

    [SerializeField]
    GameObject m_FloorScrollableUI;

    public GameObject floorScrollableUI
    {
        get => m_FloorScrollableUI;
        set => m_FloorScrollableUI = value;
    }

    [SerializeField]
    GameObject m_WallUI;

    public GameObject wallUI
    {
        get => m_WallUI;
        set => m_WallUI = value;
    }

    [SerializeField]
    GameObject m_TableUI;

    public GameObject tableUI
    {
        get => m_TableUI;
        set => m_TableUI = value;
    }

    [SerializeField]
    GameObject m_WallSubmenu;

    public GameObject wallSubmenu
    {
        get => m_WallSubmenu;
        set => m_WallSubmenu = value;
    }

    [SerializeField]
    GameObject m_WallScrollableUI;

    public GameObject wallScrollableUI
    {
        get => m_WallScrollableUI;
        set => m_WallScrollableUI = value;
    }

    [SerializeField]
    Transform m_ARCameraTransform;

    public Transform arCameraTransform
    {
        get => m_ARCameraTransform;
        set => m_ARCameraTransform = value;
    }

    bool m_ShowingSelectionUI;
    GameObject m_SpawnedObject;
    Ease m_TweenEase = Ease.OutQuart;

    const float k_TweenTime = 0.4f;

    [SerializeField]
    bool m_SubmenuActive;

    public bool submenuActive
    {
        get => m_SubmenuActive;
        set => m_SubmenuActive = value;
    }

    [SerializeField]
    Transform m_SmartEnvironmentTransform;

    public Transform smartEnvironmentTransform
    {
        get => m_SmartEnvironmentTransform;
        set => m_SmartEnvironmentTransform = value;
    }

    void Update()
    {
#if UNITY_IOS
        if ((m_ClassificationManager.currentClassification == ARMeshClassification.Table ||
            m_ClassificationManager.currentClassification == ARMeshClassification.Floor ||
            m_ClassificationManager.currentClassification == ARMeshClassification.Wall) && m_SubmenuActive == false) // If the submenu is not active
        {
            m_WallSubmenu.SetActive(false);
            switch (m_ClassificationManager.currentClassification)
            {
                case ARMeshClassification.Floor:
                m_FloorScrollableUI.SetActive(true);
                m_FloorUI.SetActive(true);
                m_WallScrollableUI.SetActive(false);
                m_WallUI.SetActive(false);
                m_TableUI.SetActive(false);
                break;

                case ARMeshClassification.Wall:

                m_WallScrollableUI.SetActive(true);
                m_WallUI.SetActive(true);
                m_FloorScrollableUI.SetActive(false);
                m_FloorUI.SetActive(false);
                m_TableUI.SetActive(false);
                break;

                case ARMeshClassification.Table:
                m_TableUI.SetActive(true);
                m_FloorScrollableUI.SetActive(false);
                m_FloorUI.SetActive(false);
                m_WallScrollableUI.SetActive(false);
                m_WallUI.SetActive(false);
                break;
            }
        }
        else
        {
            if (m_SubmenuActive == true) {
                m_WallSubmenu.SetActive(true);
            }
            else {
                m_WallSubmenu.SetActive(false);
            }
            m_FloorScrollableUI.SetActive(false);
            m_FloorUI.SetActive(false);
            m_WallScrollableUI.SetActive(false);
            m_WallUI.SetActive(false);
            m_TableUI.SetActive(false);
        }
#endif
    }

    public void PlaceFloorObject(int indexToPlace)
    {
        m_SpawnedObject = Instantiate(m_FloorPrefabs[indexToPlace], m_Reticle.GetReticlePosition().position, m_Reticle.GetReticlePosition().rotation);
        m_SpawnedObject.transform.localScale = Vector3.zero;
        // look at device but stay 'flat'
        m_SpawnedObject.transform.LookAt(m_ARCameraTransform, Vector3.up);
        m_SpawnedObject.transform.rotation = Quaternion.Euler(0, m_SpawnedObject.transform.eulerAngles.y, 0);

        // Agent's scale
        if (indexToPlace == 0)
            m_SpawnedObject.transform.DOScale(new Vector3(0.75f, 0.75f, 0.75f), k_TweenTime).SetEase(m_TweenEase);
        else if(indexToPlace != 3)
            m_SpawnedObject.transform.DOScale(Vector3.one, k_TweenTime).SetEase(m_TweenEase);
        else
            // Scale the prefab according to the slider input
            m_SpawnedObject.transform.DOScale(m_Reticle.GetSmartAreaScale(), k_TweenTime).SetEase(m_TweenEase);
    }

    // The Agent won't instantiate with this method for some reason, that's why it's a duplicate 
    public GameObject PlaceFloorObject2(int indexToPlace)
    {
        m_SpawnedObject = Instantiate(m_FloorPrefabs[indexToPlace], smartEnvironmentTransform);
        m_SpawnedObject.transform.position = m_Reticle.GetReticlePosition().position;
        m_SpawnedObject.transform.rotation = m_Reticle.GetReticlePosition().rotation;
        m_SpawnedObject.transform.localScale = Vector3.zero;
        // look at device but stay 'flat'
        m_SpawnedObject.transform.LookAt(m_ARCameraTransform, Vector3.up);
        m_SpawnedObject.transform.rotation = Quaternion.Euler(0, m_SpawnedObject.transform.eulerAngles.y, 0);

        if(indexToPlace != 3)
            m_SpawnedObject.transform.DOScale(Vector3.one, k_TweenTime).SetEase(m_TweenEase);
        else
            // Scale the prefab according to the slider input
            m_SpawnedObject.transform.DOScale(m_Reticle.GetSmartAreaScale(), k_TweenTime).SetEase(m_TweenEase);
        return m_SpawnedObject;
    }

    public void PlaceWallObject(int indexToPlace)
    {
        m_SpawnedObject = Instantiate(m_WallPrefabs[indexToPlace], smartEnvironmentTransform);
        m_SpawnedObject.transform.position = m_Reticle.GetReticlePosition().position;
        m_SpawnedObject.transform.rotation = m_Reticle.GetReticlePosition().rotation;
        m_SpawnedObject.transform.localScale = Vector3.zero;

        if(indexToPlace != 3)
            m_SpawnedObject.transform.DOScale(Vector3.one, k_TweenTime).SetEase(m_TweenEase);
        else
            // Scale the prefab according to the slider input
            m_SpawnedObject.transform.DOScale(m_Reticle.GetSmartAreaScale(), k_TweenTime).SetEase(m_TweenEase);
    }

    public GameObject PlaceWallObject2(int indexToPlace)
    {
        m_SpawnedObject = Instantiate(m_WallPrefabs[indexToPlace], smartEnvironmentTransform);
        m_SpawnedObject.transform.position = m_Reticle.GetReticlePosition().position;
        m_SpawnedObject.transform.rotation = m_Reticle.GetReticlePosition().rotation;
        m_SpawnedObject.transform.localScale = Vector3.zero;

        if(indexToPlace != 3)
            m_SpawnedObject.transform.DOScale(Vector3.one, k_TweenTime).SetEase(m_TweenEase);
        else
            // Scale the prefab according to the slider input
            m_SpawnedObject.transform.DOScale(m_Reticle.GetSmartAreaScale(), k_TweenTime).SetEase(m_TweenEase);
        return m_SpawnedObject;
    }

    public GameObject PlaceWallObject(GameObject prefab)
    {
        m_SpawnedObject = Instantiate(prefab, smartEnvironmentTransform);
        m_SpawnedObject.transform.position = m_Reticle.GetReticlePosition().position;
        m_SpawnedObject.transform.rotation = m_Reticle.GetReticlePosition().rotation;

        m_SpawnedObject.transform.localScale = Vector3.zero;
        m_SpawnedObject.transform.DOScale(Vector3.one, k_TweenTime).SetEase(m_TweenEase);
        return m_SpawnedObject;
    }

    public void PlaceTableObject(int indexToPlace)
    {
        m_SpawnedObject = Instantiate(m_TablePrefabs[indexToPlace], m_Reticle.GetReticlePosition().position, m_Reticle.GetReticlePosition().rotation);
        m_SpawnedObject.transform.localScale = Vector3.zero;
        // look at device but stay 'flat'
        m_SpawnedObject.transform.LookAt(m_ARCameraTransform, Vector3.up);
        m_SpawnedObject.transform.rotation = Quaternion.Euler(0, m_SpawnedObject.transform.eulerAngles.y, 0);

        m_SpawnedObject.transform.DOScale(Vector3.one, k_TweenTime).SetEase(m_TweenEase);
    }

    public GameObject PlaceTableObject2(int indexToPlace)
    {
        m_SpawnedObject = Instantiate(m_TablePrefabs[indexToPlace], m_Reticle.GetReticlePosition().position, m_Reticle.GetReticlePosition().rotation);
        m_SpawnedObject.transform.localScale = Vector3.zero;
        // look at device but stay 'flat'
        m_SpawnedObject.transform.LookAt(m_ARCameraTransform, Vector3.up);
        m_SpawnedObject.transform.rotation = Quaternion.Euler(0, m_SpawnedObject.transform.eulerAngles.y, 0);

        m_SpawnedObject.transform.DOScale(Vector3.one, k_TweenTime).SetEase(m_TweenEase);
        return m_SpawnedObject;
    }
}
