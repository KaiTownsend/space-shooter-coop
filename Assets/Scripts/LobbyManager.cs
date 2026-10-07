using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TMP_Text _serverLogText;

    private ServerManager _serverManager;

    private void Start()
    {
        _serverManager = (ServerManager)NetworkManager.singleton;
        _serverManager.OnJoinedLobbyEvent += OnJoinedLobbyEventHandler;
        _serverManager.OnPlayerCountChangedEvent += OnPlayerCountChangedEventHandler;
        _startGameButton.onClick.AddListener(StartGame);

        _startGameButton.gameObject.SetActive(false);
        _serverLogText.text = "Host or join a game using the HUD";
    }

    // ServerManager survives the scene change but this script doesn't,
    // so unsubscribe or the events will call into a destroyed object
    private void OnDestroy()
    {
        _serverManager.OnJoinedLobbyEvent -= OnJoinedLobbyEventHandler;
        _serverManager.OnPlayerCountChangedEvent -= OnPlayerCountChangedEventHandler;
    }

    private void OnJoinedLobbyEventHandler()
    {
        // The host is a client too, but only the host can start the game
        if (NetworkServer.active)
        {
            _startGameButton.gameObject.SetActive(true);
        }
        else
        {
            _serverLogText.text = "Connected! Waiting for host to start...";
        }
    }

    // Only called on the server, so only the host sees the player count
    private void OnPlayerCountChangedEventHandler(int playerCount)
    {
        _serverLogText.text = $"Players in lobby: {playerCount}";
    }

    private void StartGame()
    {
        // Moves the host and every connected client to the Main scene
        NetworkManager.singleton.ServerChangeScene("Main");
    }
}
