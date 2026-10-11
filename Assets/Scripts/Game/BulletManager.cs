using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class BulletManager : MonoBehaviour
{
    [HideInInspector] public List<BulletController> BulletList = new List<BulletController>();
    [HideInInspector] public Pool<GameObject> pool; // ??

    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private Transform _bulletContainer;
    [SerializeField] private int _poolSize = 100;

    private void Awake()
    {
        Debug.Log("");
    }

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
        bulletController.gameObject.SetActive(false);
    }

    public void DisableAllBullets()
    {
        for (int i = 0; i < BulletList.Count; i++)
        {
            BulletList[i].gameObject.SetActive(false);
        }
    }

    private void InitializePool(GameObject poolPrefab, byte count)
    {
        NetworkClient.RegisterPrefab(_bulletPrefab.gameObject, SpawnHandler, UnspawnHandler);
        pool = new Pool<GameObject>(CreateNew, poolSize);
    }

    static GameObject SpawnHandler(SpawnMessage msg) => Get(msg.position, msg.rotation);

    static void UnspawnHandler(GameObject spawned) => Return(spawned);


    public static GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject next = pool.Get();

        // set position/rotation and set active
        next.transform.SetPositionAndRotation(position, rotation);
        next.SetActive(true);
        return next;
    }


    public static void Return(GameObject spawned)
    {
        // disable object
        spawned.SetActive(false);

        // move the object out of reach so OnTriggerEnter doesn't get called
        spawned.transform.position = new Vector3(0, -1000, 0);

        // add back to pool
        pool.Return(spawned);
    }
}



