using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TMP_Text _serverLogText;

    private void Start()
    {
        _startGameButton.onClick.AddListener(StartGame);
    }

    private void Update()
    {
        // Only the host is allowed to start the game
        _startGameButton.gameObject.SetActive(NetworkServer.active);

        if (NetworkServer.active)
        {
            _serverLogText.text = $"Players in lobby: {NetworkServer.connections.Count}";
        }
        else if (NetworkClient.isConnected)
        {
            _serverLogText.text = "Connected! Waiting for host to start...";
        }
        else
        {
            _serverLogText.text = "Host or join a game using the HUD";
        }
    }

    private void StartGame()
    {
        // Moves the host and every connected client to the Main scene
        NetworkManager.singleton.ServerChangeScene("Main");
    }
}
