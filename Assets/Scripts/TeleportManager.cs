using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public static TeleportManager Instance;
    public GameObject player;
    private GameObject lastTeleportPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void DisableTeleportPoint(GameObject teleportPoint)
    {
        if (lastTeleportPoint != null)
        {
            lastTeleportPoint.SetActive(true);
        }
        lastTeleportPoint = teleportPoint;
        teleportPoint.SetActive(false);

#if UNITY_EDITOR
        player.GetComponent<CardboardSimulator>().UpdatePlayerPositionSimulator();
#endif
    }
}
