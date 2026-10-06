using UnityEngine;
using Mirror;
using System.Collections.Generic;

public class ServerManager : NetworkManager
{
    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private Transform _bulletContainer;
    public List<BulletController> BulletList = new List<BulletController>();
    [SerializeField] private int _magSize = 1;
    [SerializeField] private float _bulletSize = 3f;
    [SerializeField] private float _bulletSpread = 0f;
    [SerializeField] private int _bulletBounces = 0;
    [SerializeField] private float _shootCooldown = 0.25f;

    
    public override void OnStartServer()
    {
        CreateAndUpdateBullets();
    }

    private void CreateAndUpdateBullets()
    {
        Debug.Log("CreatedBullets");

        for (int i = 0; i < (_magSize - BulletList.Count); i++)
        {
            BulletController bulletController = Instantiate(_bulletPrefab, _bulletContainer);
            BulletList.Add(bulletController);
            NetworkServer.Spawn(bulletController.gameObject);
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
