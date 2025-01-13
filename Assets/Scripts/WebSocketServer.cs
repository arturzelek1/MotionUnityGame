using System;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebSocketServerScript : MonoBehaviour
{
    private WebSocketServer wssPort8080;
    private WebSocketServer wssPort8081;
    private bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            // Inicjalizacja dwóch serwerów WebSocket
            wssPort8080 = new WebSocketServer("ws://192.168.0.107:8080");
            wssPort8080.Start();
            Debug.Log("Serwer WebSocket uruchomiony na porcie 8080");

            wssPort8081 = new WebSocketServer("ws://192.168.0.107:8081");
            wssPort8081.Start();
            Debug.Log("Serwer WebSocket uruchomiony na porcie 8081");


            isRunning = true;  // Serwer jest uruchomiony
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Debug.Log("Bład podczas uruchumiania serwera" + e.Message);

        }

        // Update is called once per frame
        void Update()
        {
            // Możesz dodać logikę gry lub inne operacje w tej metodzie
        }

        // Zatrzymanie serwerów po zakończeniu gry
        void OnApplicationQuit()
        {
            if (isRunning)
            {
                if (wssPort8080 != null)
                {
                    wssPort8080.Stop();
                    Debug.Log("Serwer WebSocket na porcie 8082 zatrzymany");
                }
                if (wssPort8081 != null)
                {
                    wssPort8081.Stop();
                    Debug.Log("Serwer WebSocket na porcie 8083 zatrzymany");
                }
                isRunning = false;  // Zmieniamy stan na "zatrzymany"
            }
            else
            {
                Debug.Log("Serwer już zatrzymany");
            }
        }
    }
}

// Klasa odpowiadająca za obsługę połączeń na porcie 8080
public class MotionBehavior8080 : WebSocketBehavior
{
    // Lista aktywnych klientów
    private static List<WebSocket> clients = new List<WebSocket>();

    // Akcja wywoływana, gdy klient nawiąże połączenie
    protected override void OnOpen()
    {
        clients.Add(this.Context.WebSocket);
        Debug.Log("Klient połączony na porcie 8080");
    }

    // Akcja wywoływana, gdy klient wyśle wiadomość
    protected override void OnMessage(MessageEventArgs e)
    {
        try
        {
            string message = e.Data;
            Debug.Log("Odebrano wiadomość od klienta na porcie 8080: " + message);

            var data = JsonUtility.FromJson<WebSocketData>(message);

            if (data.type == "motion" || data.type == "rotation" || data.type == "GameRotation")
            {
                Debug.Log("Odebrane dane ruchu na porcie 8080: " + message);

                // Wysyłanie wiadomości do wszystkich klientów połączonych na porcie 8080
                SendToAllClients(message);
            }
            else
            {
                Debug.Log("Odebrano inny typ wiadomości na porcie 8080: " + data.type);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Błąd przy parsowaniu wiadomości na porcie 8080: " + ex.Message);
        }
    }

    // Akcja wywoływana, gdy klient zakończy połączenie
    protected override void OnClose(CloseEventArgs e)
    {
        clients.Remove(this.Context.WebSocket);
        Debug.Log("Klient rozłączony na porcie 8080");
    }

    // Metoda do wysyłania wiadomości do wszystkich klientów
    private void SendToAllClients(string message)
    {
        foreach (var client in clients)
        {
            // Sprawdzanie, czy klient jest połączony (czy połączenie jest otwarte)
            if (client.ReadyState == WebSocketState.Open)
            {
                client.Send(message);
            }
        }
    }

}

// Klasa odpowiadająca za obsługę połączeń na porcie 8081
public class MotionBehavior8081 : WebSocketBehavior
{
    // Lista aktywnych klientów
    private static List<WebSocket> clients = new List<WebSocket>();

    protected override void OnOpen()
    {
        clients.Add(this.Context.WebSocket);
        Debug.Log("Klient połączony na porcie 8081");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        try
        {
            string message = e.Data;
            Debug.Log("Odebrano wiadomość od klienta na porcie 8081: " + message);

            var data = JsonUtility.FromJson<WebSocketData>(message);

            if (data.type == "motion" || data.type == "rotation" || data.type == "GameRotation")
            {
                Debug.Log("Odebrane dane ruchu na porcie 8081: " + message);

                SendToAllClients(message);
            }
            else
            {
                Debug.Log("Odebrano inny typ wiadomości na porcie 8081: " + data.type);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Błąd przy parsowaniu wiadomości na porcie 8081: " + ex.Message);
        }
    }

    protected override void OnClose(CloseEventArgs e)
    {
        clients.Remove(this.Context.WebSocket);
        Debug.Log("Klient rozłączony na porcie 8081");
    }

    private void SendToAllClients(string message)
    {
        foreach (var client in clients)
        {
            // Sprawdzanie, czy klient jest połączony (czy połączenie jest otwarte)
            if (client.ReadyState == WebSocketState.Open)
            {
                client.Send(message);
            }
        }
    }

}

// Struktura danych
[System.Serializable]
public class WebSocketData
{
    public string type;
    public string data; // lub odpowiedni typ w zależności od danych
}
