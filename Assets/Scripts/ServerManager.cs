using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System;

public class ServerManager : NetworkManager
{
    public event Action OnJoinedLobbyEvent;
    public event Action<int> OnPlayerCountChangedEvent;

    [SerializeField] private Transform _bulletContainer;
    public List<BulletController> BulletList = new List<BulletController>();
    [SerializeField] private int _magSize = 1;
    [SerializeField] private float _bulletSize = 3f;
    [SerializeField] private float _bulletSpread = 0f;
    [SerializeField] private int _bulletBounces = 0;
    [SerializeField] private float _shootCooldown = 0.25f;
    private BulletController _bulletPrefab;
    private List<GameObject> prefabs;

    
    public override void OnStartServer()
    {
        prefabs = singleton.spawnPrefabs;
        _bulletPrefab = prefabs[0].GetComponent<BulletController>();
    }

    // We join in the Lobby, so don't call base (it would spawn the player).
    // Mirror spawns it for us when the host switches to the Main scene.
    public override void OnClientConnect()
    {
        OnJoinedLobbyEvent?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        OnPlayerCountChangedEvent?.Invoke(NetworkServer.connections.Count);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        OnPlayerCountChangedEvent?.Invoke(NetworkServer.connections.Count);
    }

    private void CreateAndUpdateBullets()
    {
        for (int i = 0; i < (_magSize - BulletList.Count); i++)
        {
            BulletController bulletController = Instantiate(_bulletPrefab, _bulletContainer);
            BulletList.Add(bulletController);
            NetworkServer.Spawn(bulletController.gameObject);
            bulletController.gameObject.SetActive(false);
        }

        foreach (BulletController bullet in BulletList)
        {
            bullet.transform.localScale = new Vector3(_bulletSize, _bulletSize, _bulletSize);
            bullet.bulletSpread =  _bulletSpread;
            bullet.bulletBounces = _bulletBounces;
        }

        // RecountAvailableBullets();
    }
}
