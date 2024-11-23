using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UIElements;
using WebSocketSharp;

[System.Serializable]
public class MotionData
{
    public float accelX;
    public float accelY;
    public float accelZ;
    public float gyroX;
    public float gyroY;
    public float gyroZ;
}

public class WebSocketClient : MonoBehaviour
{
    private Quaternion gyroQuaternion;
    private Vector3 position;
    private Vector3 velocity;

    private Vector3 lastRotationSpeed = Vector3.zero;
    private float maxRotationSpeed = 5.0f;

    private Vector3 initialRotation;

    private Vector3 lastGyroData;
    private Vector3 lastAccelData;

    private WebSocket webSocket;
    private Queue<string> messageQueue = new Queue<string>();


    // Zmienna do przechowywania odniesienia do obiektu
    public GameObject controlledObject; // Przypisz obiekt w Inspectorze

    void Start()
    {
        ConnectWebSocket();

        //initialRotation = transform.rotation.eulerAngles

        Input.gyro.enabled = true;
    }

    void Update()
    {
        // Przetwarzanie wiadomości z kolejki na głównym wątku
        while (messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue();
            HandleMessage(message);
        }
        lastGyroData = CorrectDrift(lastGyroData, 0.01f);
    }

    void ConnectWebSocket()
    {
        webSocket = new WebSocket("ws://localhost:8080/motion");

        webSocket.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connection opened.");
        };

        webSocket.OnMessage += (sender, e) =>
        {
            OnMessageReceived(e.Data);
        };

        webSocket.OnError += (sender, e) =>
        {
            Debug.LogError($"WebSocket Error: {e.Message}");
        };

        webSocket.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket connection closed.");
        };

        webSocket.Connect();
    }

    public void OnMessageReceived(string message)
    {
        // Dodaj przychodzącą wiadomość do kolejki do przetworzenia w Update
        messageQueue.Enqueue(message);
    }

    void HandleMessage(string message)
    {
        //Debug.Log($"Received raw message: {message}");

        try
        {
            if (message.StartsWith("{") && message.EndsWith("}"))
            {
                MotionData motionData = JsonUtility.FromJson<MotionData>(message);

                // Sprawdź, czy motionData zostało poprawnie sparsowane
                if (motionData != null)
                {
                   // Debug.Log($"Parsed Motion Data: {motionData.accelX}, {motionData.accelY}, {motionData.accelZ}, {motionData.gyroX}, {motionData.gyroY}, {motionData.gyroZ}");

                    // Wywołaj metodę do poruszania obiektem na podstawie danych
                    MoveSword(motionData.gyroX, motionData.gyroY, motionData.gyroZ,motionData.accelZ, Time.deltaTime);
                    RotateSword(motionData.gyroX, motionData.gyroY, motionData.gyroZ, Time.deltaTime);
                    //MoveSword(motionData.gyroZ, motionData.gyroY, motionData.gyroX, Time.deltaTime);
                    //MoveSword(motionData.gyroZ, 0, motionData.gyroX);

                }
                else
                {
                   // Debug.LogError("Parsed motionData is null.");
                }
            }
            else
            {
                //Debug.LogError("Received message is not a valid JSON object.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error parsing message: {ex.Message}");
            Debug.LogError($"Message content: {message}");
        }
    }


    private Vector3 lastValidPosition;

    void MoveSword(float gyroX, float gyroY, float gyroZ, float accelZ,float deltaTime)
    {
        // Skalowanie dla rotacji i translacji
        const float gyroPositionScale = 10.0f;
    
        // Mapping correct axes
        float mappedGyroX = -gyroX * gyroPositionScale; // Obrót wokół osi X
        float mappedGyroY = gyroY * gyroPositionScale;  // Obrót wokół osi Y
        float mappedGyroZ = gyroZ * gyroPositionScale;  // Obrót wokół osi Z
        float mappedAccel = accelZ;
    
        // Pozycjonowanie na podstawie żyroskopu
        Vector3 gyroPositionChange = new Vector3(mappedAccel, 0, mappedGyroZ) * gyroPositionScale * deltaTime;
    
        // Nowa pozycja miecza
        Vector3 newPosition = transform.position + gyroPositionChange;
    
        // Ograniczenia osiowe
        float maxX = 0.5f;
        float minX = -0.5f;
        float maxY = 0.5f;
        float minY = -0.5f;
        float maxZ = 0.5f;
        float minZ = -0.5f;

            // Ograniczanie dla osi X
            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

            // Ograniczanie dla osi Y (jeśli chcesz ograniczyć również ruch w osi Y)
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            // Ograniczanie dla osi Z
            newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

            // Jeśli pozycja się zmienia, zapisz ją jako ostatnią poprawną pozycję
            if (newPosition != transform.position)
            {
                lastValidPosition = newPosition;
            }

            // Aktualizacja pozycji miecza
            transform.position = newPosition;

            // Przywróć ostatnią poprawną pozycję przy powrocie
            transform.position = lastValidPosition;
        }

    void RotateSword(float gyroX, float gyroY, float gyroZ, float deltaTime)
    {
        //Scale for gyro
        const float gyroRotationScale = 20.0f;
        const float smoothingFactor = 0.8f;

        //Mapping to rotation axies 
        float mappedGyroX = -gyroZ * gyroRotationScale; // Obrót wokół osi X
        float mappedGyroY = gyroY * gyroRotationScale;  // Obrót wokół osi Y
        float mappedGyroZ = -gyroX * gyroRotationScale; // Obrót wokół osi Z

        Vector3 currentGyroRotation = new Vector3(mappedGyroX, mappedGyroY, mappedGyroZ);

        // Wygładzanie danych z żyroskopu
        Vector3 smoothedGyroRotation = Vector3.Slerp(lastGyroData, currentGyroRotation, smoothingFactor);
        lastGyroData = smoothedGyroRotation;

        // Obrót za pomocą Quaternion
        Quaternion gyroRotation = Quaternion.Euler(smoothedGyroRotation * deltaTime);

        // Aktualizacja rotacji obiektu
        transform.rotation = Quaternion.Slerp(transform.rotation, gyroRotation * transform.rotation, deltaTime * 80.0f);

    }

    //void MoveSword(float gyroX, float gyroY, float gyroZ, float accelX, float accelY, float accelZ, float deltaTime)
    // {
    //     float gyroScale = 20.0f;
    //     float gyroScaleSwing = 20.0f;
    //     float gyroScaleHorizontal = 20.0f;
    //     float gyroScaleAround = 40.0f;
    //
    //     // Re-map the gyroscope data to match the desired orientation
    //     float mappedGyroX = -gyroZ * gyroScaleSwing; // Unity X based on phone Z
    //     float mappedGyroY = gyroY * gyroScaleHorizontal; // Unity Y based on phone X
    //     float mappedGyroZ = -gyroX * gyroScaleAround; // Unity Z based on phone Y
    //
    //     Vector3 currentGyro = new Vector3(mappedGyroX, mappedGyroY, mappedGyroZ);
    //
    //     // Smoothing the gyroscope data
    //     float smoothingFactor = 0.8f;
    //     Vector3 smoothedGyro = Vector3.Slerp(lastGyroData, currentGyro, smoothingFactor);
    //     lastGyroData = smoothedGyro;
    //
    //     // Combine accelerometer and gyroscope data for more realistic rotation (using Quaternion)
    //     Quaternion gyroRotation = Quaternion.Euler(smoothedGyro * deltaTime);
    //
    //     // --- Smooth the accelerometer data ---
    //     Vector3 currentAccel = new Vector3(-accelX, accelY, accelZ);
    //
    //     // Apply smoothing to the accelerometer data (using similar approach to gyroscope)
    //     float accelSmoothingFactor = 0.5f; // You can adjust this value for more/less smoothing
    //     Vector3 smoothedAccel = Vector3.Lerp(lastAccelData, currentAccel, accelSmoothingFactor);
    //     lastAccelData = smoothedAccel;
    //
    //     // Normalize the accelerometer vector to avoid scaling issues
    //     smoothedAccel.Normalize();
    //
    //     // Convert accelerometer data to a quaternion rotation (from "up" to the accelerometer vector)
    //     Quaternion accelRotation = Quaternion.FromToRotation(Vector3.up, smoothedAccel);
    //
    //     // Final rotation as a combination of both accelerometer and gyroscope
    //     Quaternion finalRotation = accelRotation * gyroRotation;
    //
    //     // Update the object's rotation based on the calculated final rotation
    //     transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, deltaTime * 80.0f);
    //
    //     // Apply rotation around the Z-axis based on gyroZ data for more fine-tuned control
    //     float rotationAmount = smoothedGyro.z * deltaTime * gyroScale; // Z-axis rotation
    //     transform.Rotate(Vector3.forward, rotationAmount, Space.Self); // Rotate around the local Z-axis (Space.Self ensures local space)
    // }

    // void MoveSword(float gyroX, float gyroY, float gyroZ, float accelX, float accelY, float accelZ, float deltaTime)
    // {
    //     // Skalowanie danych z akcelerometru
    //     float accelScale = 5.0f; // Możesz dostosować ten współczynnik
    //     Vector3 currentAccel = new Vector3(-accelX, accelY, accelZ);
    //
    //     // Smoothing akcelerometru
    //     float accelSmoothingFactor = 0.5f;
    //     Vector3 smoothedAccel = Vector3.Lerp(lastAccelData, currentAccel, accelSmoothingFactor);
    //     lastAccelData = smoothedAccel;
    //
    //     // Normalizowanie akcelerometru
    //     smoothedAccel.Normalize();
    //
    //     // Oblicz ruch obiektu w przestrzeni w oparciu o dane z akcelerometru
    //     Vector3 moveAmount = smoothedAccel * accelScale * deltaTime;
    //
    //     // Zaktualizuj pozycję obiektu
    //     transform.position += moveAmount;
    //
    //     // Ogranicz ruch obiektu do granic kamery, tylko na osiach X i Y
    //     LimitMovementToCamera();
    // }
    //
    // void LimitMovementToCamera()
    // {
    //     // Pobierz kamerę
    //     Camera camera = Camera.main;
    //
    //     // Uzyskaj granice widoku kamery w przestrzeni świata
    //     float cameraHeight = camera.orthographicSize * 2.0f; // Dla kamer ortograficznych
    //     float cameraWidth = cameraHeight * camera.aspect;  // Dla kamer ortograficznych
    //
    //     // Przypisz maksymalne granice w przestrzeni kamery
    //     float minX = camera.transform.position.x - cameraWidth / 2;
    //     float maxX = camera.transform.position.x + cameraWidth / 2;
    //     float minY = camera.transform.position.y - cameraHeight / 2;
    //     float maxY = camera.transform.position.y + cameraHeight / 2;
    //
    //     // Ogranicz ruch obiektu do granic kamery, ale nie zmieniaj osi Z
    //     transform.position = new Vector3(
    //         Mathf.Clamp(transform.position.x, minX, maxX), // Ograniczamy tylko oś X
    //         Mathf.Clamp(transform.position.y, minY, maxY), // Ograniczamy tylko oś Y
    //         transform.position.z // Oś Z pozostaje bez zmian
    //     );
    // }

    // void MoveSword(float gyroX, float gyroY, float gyroZ, float deltaTime)
    // {
    //     float gyroScale = 20.0f;
    //     float gyroScaleSwing = 20.0f;
    //     float gyroScaleHorizontal = 20.0f;
    //     float gyroScaleAround = 40.0f;
    //
    //     // Re-map the gyroscope data to match the desired orientation
    //     float mappedGyroX = gyroZ * gyroScaleSwing; // Unity X based on phone Z
    //     float mappedGyroY = -gyroY * gyroScaleHorizontal; // Unity Y based on phone X
    //     float mappedGyroZ = gyroX * gyroScaleAround; // Unity Z based on phone Y
    //
    //     Vector3 currentGyro = new Vector3(mappedGyroX, mappedGyroY, mappedGyroZ);
    //
    //     // Smoothing the gyroscope data
    //     float smoothingFactor = 0.8f;
    //     Vector3 smoothedGyro = Vector3.Slerp(lastGyroData, currentGyro, smoothingFactor);
    //     lastGyroData = smoothedGyro;
    //
    //     // Calculate delta rotation (rotation around the local axes)
    //     Quaternion deltaRotation = new Quaternion(
    //         smoothedGyro.x * deltaTime / 2,
    //         smoothedGyro.y * deltaTime / 2,
    //         smoothedGyro.z * deltaTime / 2,
    //         0);
    //
    //     deltaRotation.w = 1.0f - (smoothedGyro.x * smoothedGyro.x + smoothedGyro.y * smoothedGyro.y + smoothedGyro.z * smoothedGyro.z) * (deltaTime * deltaTime) / 8.0f;
    //
    //     // Update the quaternion with the new delta rotation
    //     gyroQuaternion = deltaRotation * gyroQuaternion;
    //     gyroQuaternion.Normalize();
    //
    //     // Apply the updated rotation to the whole object
    //     transform.rotation = Quaternion.Slerp(transform.rotation, gyroQuaternion, deltaTime * 80.0f);
    //
    //     // Rotate around the Z-axis based on gyroZ data
    //     float rotationAmount = smoothedGyro.y * deltaTime * gyroScale; // Z-axis rotation
    //     transform.Rotate(Vector3.forward, rotationAmount, Space.Self); // Rotate around the local Z-axis (Space.Self ensures local space)
    // }
    // }    void MoveSword(float gyroX, float gyroY, float gyroZ, float deltaTime)
    // {
    //     float gyroScale = 20.0f;
    //     float gyroScaleSwing = 20.0f;
    //     float gyroScaleHorizontal = 20.0f;
    //    float gyroScaleAround = 40.0f;
    //
    //    // Re-map the gyroscope data to match the desired orientation
    //    float mappedGyroX = gyroZ * gyroScaleSwing; // Unity X based on phone Z
    //    float mappedGyroY = -gyroY * gyroScaleHorizontal; // Unity Y based on phone X
    //    float mappedGyroZ = gyroX * gyroScaleAround; // Unity Z based on phone Y
    //
    //    Vector3 currentGyro = new Vector3(mappedGyroX, mappedGyroY, mappedGyroZ);
    //
    //    // Smoothing the gyroscope data
    //    float smoothingFactor = 0.8f;
    //    Vector3 smoothedGyro = Vector3.Slerp(lastGyroData, currentGyro, smoothingFactor);
    //    lastGyroData = smoothedGyro;
    //
    //    // Calculate delta rotation (rotation around the local axes)
    //    Quaternion deltaRotation = new Quaternion(
    //        smoothedGyro.x * deltaTime / 2,
    //        smoothedGyro.y * deltaTime / 2,
    //        smoothedGyro.z * deltaTime / 2,
    //        0);
    //
    //    deltaRotation.w = 1.0f - (smoothedGyro.x * smoothedGyro.x + smoothedGyro.y * smoothedGyro.y + smoothedGyro.z * smoothedGyro.z) * (deltaTime * deltaTime) / 8.0f;
    //
    //    // Update the quaternion with the new delta rotation
    //    gyroQuaternion = deltaRotation * gyroQuaternion;
    //    gyroQuaternion.Normalize();
    //
    //    // Apply the updated rotation to the whole object
    //    transform.rotation = Quaternion.Slerp(transform.rotation, gyroQuaternion, deltaTime * 80.0f);
    //
    //    // Move along the Z-axis based on gyroZ data
    //    float moveAmount = smoothedGyro.z * deltaTime * gyroScale; // Z-axis movement
    //    transform.Translate(Vector3.forward * moveAmount, Space.Self); // Move relative to the object's own local space
    //}


    Vector3 CorrectDrift(Vector3 gyroData, float driftCorrectionFactor) { return gyroData * (1.0f - driftCorrectionFactor); }

    private void OnApplicationQuit()
    {
        // Zamykanie WebSocket przy zamykaniu aplikacji
        if (webSocket != null)
        {
            webSocket.Close();
        }
    }
}