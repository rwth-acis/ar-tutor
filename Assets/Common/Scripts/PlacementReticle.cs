using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class PlacementReticle : MonoBehaviour
{
    [SerializeField]
    bool m_SnapToMesh;

    public bool snapToMesh
    {
        get => m_SnapToMesh;
        set => m_SnapToMesh = value;
    }

    [SerializeField]
    ARRaycastManager m_RaycastManager;

    public ARRaycastManager raycastManager
    {
        get => m_RaycastManager;
        set => m_RaycastManager = value;
    }

    [SerializeField]
    GameObject m_ReticlePrefab;

    public GameObject reticlePrefab
    {
        get => m_ReticlePrefab;
        set => m_ReticlePrefab = value;
    }

    [SerializeField]
    Slider widthSlider;

    [SerializeField]
    Slider lengthSlider;

    [SerializeField]
    GameObject m_InteractableAreaLabelPrefab;

    public GameObject interactableAreaLabelPrefab
    {
        get => m_InteractableAreaLabelPrefab;
        set => m_InteractableAreaLabelPrefab = value;
    }

    [SerializeField]
    bool m_DistanceScale;

    public bool distanceScale
    {
        get => m_DistanceScale;
        set => m_DistanceScale = value;
    }

    [SerializeField]
    Transform m_CameraTransform;

    public Transform cameraTransform
    {
        get => m_CameraTransform;
        set => m_CameraTransform = value;
    }

    GameObject m_SpawnedReticle;
    GameObject m_SpawnedInteractiveAreaLabel;
    CenterScreenHelper m_CenterScreen;
    TrackableType m_RaycastMask;
    float m_CurrentDistance;
    float m_CurrentNormalizedDistance;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    const float k_MinScaleDistance = 0.0f;
    const float k_MaxScaleDistance = 1.0f;
    const float k_ScaleMod = 1.0f;

    private void OnEnable()
    {
        // Register to event manager events
        EventManager.OnSXStatusChanged += ChangeReticleVisibility;
    }

    private void OnDisable()
    {
        // Unregister from event manager events
        EventManager.OnSXStatusChanged -= ChangeReticleVisibility;
    }

    void Start()
    {
        m_CenterScreen = CenterScreenHelper.Instance;
        if (m_SnapToMesh)
        {
            m_RaycastMask = TrackableType.PlaneEstimated;
        }
        else
        {
            m_RaycastMask = TrackableType.PlaneWithinPolygon;
        }

        m_SpawnedReticle = Instantiate(m_ReticlePrefab);
        m_SpawnedReticle.SetActive(false);

        m_SpawnedInteractiveAreaLabel = Instantiate(m_InteractableAreaLabelPrefab);
        m_SpawnedInteractiveAreaLabel.SetActive(false);
    }

    void Update()
    {
        if (m_RaycastManager.Raycast(m_CenterScreen.GetCenterScreen(), s_Hits, m_RaycastMask))
        {
            Pose hitPose = s_Hits[0].pose;
            m_SpawnedReticle.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            m_SpawnedReticle.SetActive(true);

            m_SpawnedInteractiveAreaLabel.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
            m_SpawnedInteractiveAreaLabel.SetActive(true);
        }

        if (m_DistanceScale)
        {
            m_CurrentDistance = Vector3.Distance(m_SpawnedReticle.transform.position, m_CameraTransform.position);
            m_CurrentNormalizedDistance = ((Mathf.Abs(m_CurrentDistance - k_MinScaleDistance)) / (k_MaxScaleDistance - k_MinScaleDistance))+k_ScaleMod;
            m_SpawnedReticle.transform.localScale = new Vector3(m_CurrentNormalizedDistance, m_CurrentNormalizedDistance, m_CurrentNormalizedDistance);
        }
    }

    public Transform GetReticlePosition()
    {
        return m_SpawnedReticle.transform;
    }

    public Vector3 GetSmartAreaScale()
    {
        return m_SpawnedInteractiveAreaLabel.transform.localScale;
    }

    public void ChangeInteractableAreaLabelWidth()
    {
        m_SpawnedInteractiveAreaLabel.transform.localScale = new Vector3(m_SpawnedInteractiveAreaLabel.transform.localScale.x, m_SpawnedInteractiveAreaLabel.transform.localScale.y, 0.01f * widthSlider.value);
    }

    public void ChangeInteractableAreaLabelLength()
    {
        m_SpawnedInteractiveAreaLabel.transform.localScale = new Vector3(0.01f * lengthSlider.value, m_SpawnedInteractiveAreaLabel.transform.localScale.y, m_SpawnedInteractiveAreaLabel.transform.localScale.z);
    }

    public void ChangeReticleVisibility(bool play)
    {
        m_SpawnedReticle.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = play;
        m_SpawnedReticle.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().enabled = play;

        m_SpawnedInteractiveAreaLabel.GetComponent<MeshRenderer>().enabled = play;
    }
}
