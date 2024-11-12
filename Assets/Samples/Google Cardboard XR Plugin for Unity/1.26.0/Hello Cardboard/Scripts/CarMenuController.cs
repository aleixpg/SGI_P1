using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CarMenuController : MonoBehaviour
{
    [SerializeField] private Canvas menuCanvas;
    [SerializeField] private Transform playerTransform;
    private GameObject selectedCar;

    private void Start()
    {
        playerTransform = Camera.main.transform;
        menuCanvas.gameObject.SetActive(false); // Menú desactivado al inicio
    }

    
    // Llama a este método cuando se selecciona el coche
    public void ShowMenu(GameObject car)
    {
        selectedCar = car;
        menuCanvas.transform.position = playerTransform.position + playerTransform.forward * 2; // Coloca el menú frente al jugador
        menuCanvas.transform.LookAt(playerTransform);
        menuCanvas.transform.Rotate(0, 180, 0);
        menuCanvas.gameObject.SetActive(true);
    }
    public void Delete()
    {
        selectedCar.gameObject.SetActive(false);
    }
    // Método para cerrar el menú
    public void CloseMenu()
    {
        menuCanvas.gameObject.SetActive(false);
        selectedCar = null;
    }

    // Métodos de los botones para cambiar el tamaño
    public void IncreaseSize()
    {
        if (selectedCar != null)
        {
            selectedCar.transform.localScale += Vector3.one * 0.1f;

        }
    }

    public void DecreaseSize()
    {
        if (selectedCar != null)
        {
            selectedCar.transform.localScale -= Vector3.one * 0.1f;
        }
    }

    // Métodos de los botones para rotar el coche
    public void RotateRight()
    {
        if (selectedCar != null)
        {
            selectedCar.transform.Rotate(Vector3.up, -10);
        }
    }

    public void RotateLeft()
    {
        if (selectedCar != null)
        {
            selectedCar.transform.Rotate(Vector3.up, 10);
        }
    }
}
