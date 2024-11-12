using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointerManager : MonoBehaviour
{
    public static CameraPointerManager Instance;
    [SerializeField] private GameObject pointer;
    [Range(0,1)]
    [SerializeField] private float disPointerObject = 0.95f;
    private const float _maxDistance = 10;
    private GameObject _gazedAtObject = null;

    private readonly string interactableTag = "Interactable";
    private float scaleSize = 0.02f;
    [HideInInspector]
    public Vector3 hitPoint;

    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        GazeManager.Instance.OnGazeSelection += OnGazeSelection;
    }

    private void OnGazeSelection()
    {
        
       _gazedAtObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
    }
    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void Update()
    {
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed
        // at.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance))
        {
            hitPoint = hit.point;
            // GameObject detected in front of the camera.
            if (_gazedAtObject != hit.transform.gameObject)
            {
                // New GameObject.
                _gazedAtObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
                _gazedAtObject = hit.transform.gameObject;
                _gazedAtObject.SendMessage("OnPointerEnterXR", null, SendMessageOptions.DontRequireReceiver);
                GazeManager.Instance.StartGazeSelection();
            }
            if (hit.transform.CompareTag(interactableTag))
            {
                PointerOnGaze(hit.point);
            }
            else
            {
                PointerOutGaze(hit.point);
            }
        }
        else
        {
            // No GameObject detected in front of the camera.
            Vector3 defaultPointerPosition = transform.position + transform.forward * _maxDistance;
            _gazedAtObject?.SendMessage("OnPointerExitXR", null, SendMessageOptions.DontRequireReceiver);
            _gazedAtObject = null;
            PointerOutGaze(defaultPointerPosition);
        }

        // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            _gazedAtObject?.SendMessage("OnPointerClickXR", null, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void PointerOnGaze(Vector3 hitPoint)
    {
        // Calculate the scale of the pointer based on the distance between the camera and the hit point.
        float scaleFactor = scaleSize * Vector3.Distance(transform.position, hitPoint);
        // Limit the scale of the pointer.
        pointer.transform.localScale = Vector3.one * scaleFactor;
        //pointer.transform.localScale = Vector3.one * Mathf.Clamp(scaleFactor, 0.1f, 0.5f);
        // Calculate the position of the pointer between the camera and the hit point.
        pointer.transform.parent.position = CalculatePointerPosition(transform.position, hitPoint, disPointerObject);

    }

    private void PointerOutGaze(Vector3 targetPosition)
    {
        // Posiciona el puntero en una posición intermedia entre la cámara y el punto proporcionado.
        float distanceBetween = Vector3.Distance(transform.position, targetPosition);
        Vector3 pointerPosition = CalculatePointerPosition(transform.position, targetPosition, disPointerObject);

        // Ajustar el tamaño del puntero según la distancia.
        float scaleFactor = scaleSize * distanceBetween;
        pointer.transform.localScale = Vector3.one * scaleFactor;

        // Aplicar la posición y orientación del puntero.
        pointer.transform.parent.position = pointerPosition;
        pointer.transform.parent.parent.rotation = transform.rotation;

        // Cancelar la selección de mirada si no hay un objeto mirado.
        GazeManager.Instance.CancelGazeSelection();
    }

    // Calculate the position of the pointer between the camera and the hit point of the ray at a certain distance.
    private Vector3 CalculatePointerPosition(Vector3 p0, Vector3 p1, float distance)
    {
        float x = p0.x + (p1.x - p0.x) * distance;
        float y = p0.y + (p1.y - p0.y) * distance;
        float z = p0.z + (p1.z - p0.z) * distance;

        return new Vector3(x, y, z);
    }
}
