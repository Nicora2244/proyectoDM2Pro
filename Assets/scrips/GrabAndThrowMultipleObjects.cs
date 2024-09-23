using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabAndThrowMultipleObjects : MonoBehaviour
{
    public Transform holdPoint;
    public float throwForce = 500f;

    private Rigidbody objectRb;
    private bool isHolding = false;
    private Camera mainCamera;
    private GameObject currentObject; // Referencia al objeto actual que se está agarrando
    private TrailRenderer trailRenderer; // Trail Renderer del objeto actual
    private Vector3 initialMousePosition; // Posición inicial del mouse al agarrar
    private bool mouseMoved = false;      // Indica si el mouse se ha movido

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isHolding)
            {
                TryGrabObject();
                // Guardamos la posición inicial del mouse al agarrar el objeto
                initialMousePosition = Input.mousePosition;
                mouseMoved = false; // Reiniciar indicador de movimiento del mouse
            }
        }

        // Verificar si el mouse se ha movido
        if (isHolding && Input.mousePosition != initialMousePosition)
        {
            mouseMoved = true;
        }

        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            ThrowObject();
        }
    }

    void TryGrabObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Detectar si se hace clic sobre un objeto con collider
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Comprobar si es uno de los objetos que pueden ser lanzados (tennis_ball, frisbee, gold bar)
            if (hitObject.CompareTag("Throwable"))
            {
                currentObject = hitObject;
                GrabObject();
            }
        }
    }

    void GrabObject()
    {
        objectRb = currentObject.GetComponent<Rigidbody>();
        trailRenderer = currentObject.GetComponent<TrailRenderer>();

        // Desactivar la física mientras lo tienes
        objectRb.isKinematic = true;
        currentObject.transform.position = holdPoint.position;
        currentObject.transform.parent = holdPoint;
        isHolding = true;

        if (trailRenderer != null)
        {
            trailRenderer.enabled = false; // Desactivar estela mientras lo tienes
        }
    }

    void ThrowObject()
    {
        // Soltar el objeto
        currentObject.transform.parent = null;
        objectRb.isKinematic = false;

        if (mouseMoved)
        {
            // Si el mouse se movió, lanzar el objeto
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 launchDirection;
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                launchDirection = (hit.point - holdPoint.position).normalized;
            }
            else
            {
                launchDirection = holdPoint.forward;
            }

            // Aplicar la fuerza para lanzar
            objectRb.AddForce(launchDirection * throwForce);
        }
        // Si el mouse no se movió, el objeto caerá
        isHolding = false;

        // Activar el Trail Renderer si tiene uno
        if (trailRenderer != null)
        {
            trailRenderer.enabled = true;
        }

        currentObject = null; // Limpiar la referencia al objeto actual
    }
}