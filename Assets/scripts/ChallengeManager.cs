using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Para cambiar de escena

public class ChallengeManager : MonoBehaviour
{
    public Text challengeText; // Texto UI para mostrar el reto
    public Text timerText; // Texto UI para mostrar el tiempo restante
    public GameObject losePanel; // El panel que aparecerá cuando el jugador pierda
    public Button retryButton; // Botón para reintentar

    private float timeRemaining = 30f;
    private bool challengeActive = true;
    private int objectsScored = 0; // Contador de objetos encestados
    private int totalObjectsToScore = 3; // Total de objetos necesarios para ganar

    void Start()
    {
        losePanel.SetActive(false); // Asegurarse de que el panel esté desactivado al inicio
        retryButton.onClick.AddListener(RestartChallenge); // Asignar la función al botón de reintento
        UpdateChallengeText();
    }

    void Update()
    {
        if (challengeActive)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = "Tiempo restante: " + Mathf.Ceil(timeRemaining).ToString() + "s";

            if (timeRemaining <= 0)
            {
                LoseChallenge(); // Llamar a la función cuando se acabe el tiempo
            }
        }
    }

    public void ObjectScored()
    {
        // Incrementar el contador de objetos encestados
        objectsScored++;
        UpdateChallengeText();

        // Verificar si se ha completado el reto con 3 objetos encestados
        if (objectsScored >= totalObjectsToScore)
        {
            challengeText.text = "¡Reto completado!";
            challengeActive = false;
            StartCoroutine(CompleteChallenge()); // Iniciar la corutina para esperar y cambiar de escena
        }
    }

    IEnumerator CompleteChallenge()
    {
        yield return new WaitForSeconds(3); // Esperar 3 segundos
        // Cargar la siguiente escena en el orden de Build Settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void LoseChallenge()
    {
        challengeActive = false;
        timerText.text = "¡Tiempo agotado!";
        challengeText.text = "Reto fallido";
        losePanel.SetActive(true); // Mostrar el panel de "Perdiste"
    }

    public void RestartChallenge()
    {
        // Reiniciar la escena actual para reintentar el desafío
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void UpdateChallengeText()
    {
        challengeText.text = "Objetos encestados: " + objectsScored + "/" + totalObjectsToScore;
    }
}