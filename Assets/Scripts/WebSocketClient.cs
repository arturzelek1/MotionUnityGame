using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using WebSocketSharp;
/*
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

[System.Serializable]
public class RotationData
{
    public List<float> quaternion;
}

[System.Serializable]
public class GameRotationData
{
    public List<float> quaternion;
}

public class WebSocketClient : MonoBehaviour
{
    private UnityEngine.Vector3 position;
    private UnityEngine.Quaternion initialRotation;
    private UnityEngine.Vector3 lastGyroData;
    
    private WebSocket webSocket;
    private Queue<string> messageQueue = new Queue<string>();
    public GameObject controlledObject;

    void Start()
    {
        ConnectWebSocket();
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
        webSocket = new WebSocket("ws://localhost:8080/");

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
        messageQueue.Enqueue(message);
    }

    void HandleMessage(string message)
    {
        try
        {
            if (message.StartsWith("{") && message.EndsWith("}"))
            {
                MotionData motionData = JsonUtility.FromJson<MotionData>(message);
                RotationData rotationData = JsonUtility.FromJson<RotationData>(message);
                GameRotationData gameRotationData = JsonUtility.FromJson<GameRotationData>(message);

                if (motionData != null)
                {
                    Debug.Log("Otrzymano dane MotionData");
                    HandleMotionData(motionData);
                }
                if(rotationData !=null)
                {
                    Debug.Log("Otrzymano dane RotationData");
                    HandleRotationData(rotationData);
                }
                if (gameRotationData != null)
                {
                    Debug.Log("Otrzymano dane GameRotationData");
                    HandleGameRotationData(gameRotationData);
                }
                else
                {
                }
            }
            else
            {
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error parsing message: {ex.Message}");
            Debug.LogError($"Message content: {message}");
        }
    }
    void HandleMotionData(MotionData motionData)
    {
        Debug.Log($"Motion Data: {motionData.accelX}, {motionData.accelY}, {motionData.accelZ}, {motionData.gyroX}, {motionData.gyroY}, {motionData.gyroZ}");
    }
    private UnityEngine.Quaternion initialOffsetQuaternion = UnityEngine.Quaternion.identity;

    private UnityEngine.Quaternion initialOffsetYRotation = UnityEngine.Quaternion.Euler( 0, 90 ,0 );
    private UnityEngine.Quaternion initialOffsetZRotation = UnityEngine.Quaternion.Euler(0,0,-90);
    
    void HandleRotationData(RotationData rotationData)
    {
        if (rotationData.quaternion != null && rotationData.quaternion.Count == 4)
        {
            UnityEngine.Quaternion phoneQuaternion = new UnityEngine.Quaternion(-rotationData.quaternion[0], rotationData.quaternion[1], rotationData.quaternion[2], rotationData.quaternion[3]);
            
            UnityEngine.Quaternion adjustedQuaternion = initialOffsetYRotation * initialOffsetZRotation * phoneQuaternion;
            
            transform.localRotation = adjustedQuaternion;
        }
        else
        {
            Debug.LogWarning("Niepoprawny quaternion lub brak danych.");
        } 
            Debug.Log($"Rotation Data: {string.Join(", ", rotationData.quaternion)}"); 
    }

    void HandleGameRotationData(GameRotationData gameRotationData)
    {
        Debug.Log($"Game Rotation Data: {string.Join(", ", gameRotationData.quaternion)}");
    }


    void rotateObject(float deltTime, List<float> quaternion)
    {
        // Sprawdzenie, czy lista quaternionów nie jest pusta
        if (quaternion != null && quaternion.Count == 4)
        {
            // Tworzymy quaternion z listy (zakładając, że jest to 4-elementowa lista)
            UnityEngine.Quaternion rotation = new UnityEngine.Quaternion(quaternion[0], quaternion[1], quaternion[2], quaternion[3]);

            // Przykład: Rotacja obiektu w grze
            transform.rotation = UnityEngine.Quaternion.Slerp(transform.rotation, rotation, deltTime);
        }
        else
        {
            Debug.LogWarning("Niepoprawny quaternion lub brak danych.");
        }
    }
    UnityEngine.Vector3 CorrectDrift(UnityEngine.Vector3 gyroData, float driftCorrectionFactor) { return gyroData * (1.0f - driftCorrectionFactor); }

    private void OnApplicationQuit()
    {
        // Zamykanie WebSocket przy zamykaniu aplikacji
        if (webSocket != null)
        {
            webSocket.Close();
        }
    }
}*/