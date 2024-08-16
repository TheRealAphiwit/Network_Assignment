using UnityEngine;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;

    private void OnGUI()
    {
        if (GUILayout.Button("Host"))
        {
            networkManager.StartHost();
        }

        if (GUILayout.Button("Join"))
        {
            networkManager.StartClient();
        }

        if (GUILayout.Button("Quit"))
        {
            Application.Quit();
        }
    }
}
