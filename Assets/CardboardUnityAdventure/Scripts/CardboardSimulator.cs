using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardboardSimulator : MonoBehaviour
{
    public bool UseCardboardSimulator = true;

    [SerializeField] private float horizontalSpeed = 0.5f;
    [SerializeField] private float verticalSpeed = 0.5f;
    [SerializeField] private float rotationX = 0.0f;
    [SerializeField] private float rotationY = 0.0f;
    private Camera cam;

    private Vector2 mouseDelta; // Stores mouse movement deltas

    void Start()
    {
#if UNITY_EDITOR
        cam = Camera.main;
#endif
    }

    void Update()
    {
#if UNITY_EDITOR
        if (!UseCardboardSimulator)
            return;

        if (Mouse.current.leftButton.isPressed) // New Input System for mouse button
        {
            mouseDelta = Mouse.current.delta.ReadValue(); // Get mouse delta values
            float mouseX = mouseDelta.x * horizontalSpeed * Time.deltaTime;
            float mouseY = mouseDelta.y * verticalSpeed * Time.deltaTime;

            rotationY += mouseX;
            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -45, 45);
            cam.transform.localEulerAngles = new Vector3(rotationX, rotationY, 0.0f);
        }
#endif
    }

    public void UpdatePlayerPositionSimulator()
    {
        rotationX = 0;
        rotationY = cam.transform.localEulerAngles.y;
    }
}
