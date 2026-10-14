using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class BulletManager : MonoBehaviour
{
    [HideInInspector] public List<BulletController> BulletList = new List<BulletController>();

    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private Transform _bulletContainer;
    [SerializeField] private int _poolSize = 100;

    private void Start()
    {
        CreateBullets();
    }

    private void CreateBullets()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            BulletController bulletController = Instantiate(_bulletPrefab, _bulletContainer);
            bulletController.SetManager(this);
            bulletController.gameObject.SetActive(false);

            BulletList.Add(bulletController);
        }
    }

    public BulletController GetUniqueBullet(PlayerController playerController, int magSize)
    {
        if (CountBullets(playerController) >= magSize) return null;

        for (int i = 0; i < BulletList.Count; i++)
        {
            if (!BulletList[i].gameObject.activeInHierarchy)
            {
                return BulletList[i];
            }
        }

        return null; // is needed?
    }

    public int CountBullets(PlayerController playerController)
    {
        int count = 0;

        for (int i = 0; i < BulletList.Count; i++)
        {
            if (BulletList[i].gameObject.activeInHierarchy && BulletList[i].BulletOwner == playerController)
            {
                count++;
            }
        }


        return count;
    }

    public void DisableBullet(BulletController bulletController)
    {
        NetworkServer.UnSpawn(bulletController.gameObject);
        bulletController.gameObject.SetActive(false);
        bulletController.BulletOwner = null;
    }

    public void DisableAllBullets()
    {
        for (int i = 0; i < BulletList.Count; i++)
        {
            DisableBullet(BulletList[i]);
        }
    }
}


