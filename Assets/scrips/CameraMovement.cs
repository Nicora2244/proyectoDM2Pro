using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        // Variables para la entrada del teclado
        float moveHorizontal = Input.GetAxis("Horizontal"); // A y D
        float moveVertical = Input.GetAxis("Vertical");     // W y S

        // Movimiento basado en el input
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Aplicar el movimiento a la cámara
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }
}
