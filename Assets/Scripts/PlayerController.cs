using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public event Action OnTakeDamageEvent;
    public event Action<int, int> OnUpdateBulletCountEvent;

    [SerializeField] private PlayerUpgradableData[] _playerUpgradableDataList;
    [SerializeField] private BulletController _bulletPrefab;
    [SerializeField] private GameObject _bulletContainerPrefab;
    
    [Header("Ship Sprites")]
    [SerializeField] private Sprite _maxHPSprite;
    [SerializeField] private Sprite _highHPSprite;
    [SerializeField] private Sprite _mediumHPSprite;
    [SerializeField] private Sprite _lowHPSprite;

    [Header("General Stats")]
    [SerializeField] private float _movementSpeed = 5f;
    public float MaxHealth = 100f;

    [Header("Gun/Bullet Stats")]
    [SerializeField] private int _magSize = 1;
    [SerializeField] private float _bulletSize = 3f;
    [SerializeField] private float _bulletSpread = 0f;
    [SerializeField] private int _bulletBounces = 0;
    [SerializeField] private float _shootCooldown = 0.25f;

    [HideInInspector] public float ReloadTimer;
    public float Health { get; private set; }
    private GameManager _gameManager;
    private GameObject _bulletContainer;
    private Transform _enemySpawnTransform;
    private SpriteRenderer _spriteRenderer;
    private List<BulletController> _bulletList;
    private float _clampMin;
    private float _clampMax;
    private int _availableBulletCount = 0;

    private void Awake()
    {
        _bulletList = new List<BulletController>();
        _bulletContainer = Instantiate(_bulletContainerPrefab);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        _enemySpawnTransform = GameObject.Find("EnemySpawn").transform;

        CreateAndUpdateBullets();
    }

    private void Start()
    {
        _clampMin = _enemySpawnTransform.position.x;
        _clampMax = -_enemySpawnTransform.position.x;

        Health = MaxHealth;

        OnUpdateBulletCountEvent?.Invoke(_availableBulletCount, _bulletList.Count);

        foreach (BulletController bullet in _bulletList)
        {
            bullet.OnDisableBulletEvent += OnDisableBulletEventHandler;
        }
    }

    private void Update()
    {
        // _uiManager.UpdateMagText(_availableBulletCount, _bulletList.Count);

        if (_gameManager.isGamePaused)
        {

            return;
        }

        float inputDirection = Input.GetAxisRaw("Horizontal");
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + _movementSpeed * Time.deltaTime * inputDirection, _clampMin, _clampMax), transform.position.y, transform.position.z);

        ReloadTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && ReloadTimer >= _shootCooldown)
        {
            Shoot();
            ReloadTimer = 0f;
        }
    }

    public void OnDisableBulletEventHandler()
    {
        RecountAvailableBullets();
        OnUpdateBulletCountEvent?.Invoke(_availableBulletCount, _bulletList.Count);
    }

    public void TakeDamage(float damageAmt)
    {
        Health -= damageAmt;
        UpdateShipState();

        OnTakeDamageEvent?.Invoke(); // shortcut for checking if it isnt null
    }

    public void ResetPlayerAndBullets()
    {
        foreach (BulletController bullet in _bulletList)
        {
            if (bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(false);
            }
        }

        transform.position = new Vector3 (0, -0.8f, 0);

        RecountAvailableBullets();
    }

    public void ChangeStatsFromUpgrade(int upgradeIndex)
    {
        PlayerUpgradableData playerUpgrade = _playerUpgradableDataList[upgradeIndex];

        _movementSpeed += playerUpgrade.MovementSpeedToAdd;
        MaxHealth += playerUpgrade.MaxHealthToAdd;
        
        _bulletSize += playerUpgrade.BulletSizeToAdd;
        _bulletSpread += playerUpgrade.BulletSpreadToAdd;
        _bulletBounces += playerUpgrade.BulletBouncesToAdd;
        _shootCooldown += playerUpgrade.ShootCooldownToAdd;
        _magSize += playerUpgrade.MagSizeToAdd;

        CreateAndUpdateBullets();
    }

    private void UpdateShipState()
    {
        if (Health > (0.80 * MaxHealth)) _spriteRenderer.sprite = _maxHPSprite;
        else if (Health > (0.6 * MaxHealth)) _spriteRenderer.sprite = _highHPSprite;
        else if (Health > (0.3 * MaxHealth)) _spriteRenderer.sprite = _mediumHPSprite;
        else if (Health > (0 * MaxHealth)) _spriteRenderer.sprite = _lowHPSprite;
        else
        {
            _spriteRenderer.sprite = _lowHPSprite;
            Debug.Log("Game Over"); // replace with actual game over eventually or something.
        }
    }

    private void CreateAndUpdateBullets()
    {

        for (int i = 0; i < (_magSize - _bulletList.Count); i++)
        {
            BulletController bulletController = Instantiate(_bulletPrefab, _bulletContainer.transform);
            _bulletList.Add(bulletController);
        }

        foreach (BulletController bullet in _bulletList)
        {
            bullet.transform.localScale = new Vector3(_bulletSize, _bulletSize, _bulletSize);
            bullet.bulletSpread =  _bulletSpread;
            bullet.bulletBounces = _bulletBounces;
        }

        RecountAvailableBullets();
    }

    private void Shoot()
    {
        foreach (BulletController bullet in _bulletList)
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.gameObject.SetActive(true);
                bullet.transform.position = new Vector3 (transform.position.x, transform.position.y + 0.3f + _bulletList[0].transform.localScale.y / 80f, transform.position.z);
                break;
            }
        }

        RecountAvailableBullets();
    }

    private void RecountAvailableBullets()
    {
        _availableBulletCount = 0;

        foreach (var bullet in _bulletList) // can use var instead of BulletController etc. this is same as foreach under shoot method
        {
            if (!bullet.gameObject.activeInHierarchy)
            {
                _availableBulletCount++;
            }
        }

        OnUpdateBulletCountEvent?.Invoke(_availableBulletCount, _bulletList.Count);
    }
}
