using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportPoint : MonoBehaviour
{
    public UnityEvent OnTeleport;
    public UnityEvent OnTeleportExit;
    public UnityEvent OnTeleportEnter;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void OnPointerClickXR()
    {
        ExecuteTeleport();
        OnTeleport?.Invoke();
        TeleportManager.Instance.DisableTeleportPoint(gameObject);
    }

    public void OnPointerEnterXR()
    {
        OnTeleportEnter?.Invoke();
    }

    public void OnPointerExitXR()
    {
        OnTeleportExit?.Invoke();
    }

    private void ExecuteTeleport()
    {
        GameObject player = TeleportManager.Instance.player;
        player.transform.position = transform.position;
        Camera camera = player.GetComponentInChildren<Camera>();
        float rotY = transform.rotation.eulerAngles.y - camera.transform.localEulerAngles.y;
        player.transform.rotation = Quaternion.Euler(0, rotY, 0);
    }
}
