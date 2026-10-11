using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class BulletManager : NetworkBehaviour
{
    [HideInInspector] public List<BulletController> BulletList = new List<BulletController>();

    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private Transform _bulletContainer;
    private int _poolSize = 100;

    public override void OnStartServer()
    {
        base.OnStartServer();

        CreateBullets();
    }

    [Server]
    private void CreateBullets()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            BulletController bulletController = Instantiate(_bulletPrefab, _bulletContainer);
            bulletController.gameObject.SetActive(false);

            BulletList.Add(bulletController);
        }
    }
}
