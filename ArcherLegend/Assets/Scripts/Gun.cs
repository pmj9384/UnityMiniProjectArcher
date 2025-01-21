using UnityEngine;
using UnityEngine.Pool; // Object Pool API 사용

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading,
    }

    public State GunState { get; private set; }
    public GunData gundata;

    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint;     // 총알 발사 위치

    private AudioSource audioSource;
    public ParticleSystem muzzleEffect;
    public ParticleSystem shellEffect;

    private float lastFireTime;
    private int currentAmmo;

    public float cooldownTime = 1f; // 쿨다운 시간
    public VirtualJoyStick joystick; 

    private IObjectPool<GameObject> bulletPool; 

    public float targetingRange = 50f;  // 타겟팅 범위
    public LayerMask targetLayer;       // 타겟을 찾을 레이어

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // 오브젝트 풀 초기화
        bulletPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(bulletPrefab),  // 새 총알 생성
            actionOnGet: bullet => bullet.SetActive(true),  // 풀에서 꺼낼 때 활성화
            actionOnRelease: bullet => bullet.SetActive(false),  // 반환 시 비활성화
            actionOnDestroy: bullet => Destroy(bullet),  // 풀의 크기 초과 시 제거
            collectionCheck: false,  // 컬렉션 크기 검사 비활성화
            maxSize: 50  // 최대 오브젝트 수
        );
    }

    private void OnEnable()
    {
        GunState = State.Ready;
        lastFireTime = 0f;
    }

    private void Update()
    {
        Fire();
    }

    public void Fire()
    {
        Vector2 joystickInput = joystick.Input;

        if (joystickInput.sqrMagnitude > 0.01f)
        {
            // 조이스틱 입력이 있는 경우 발사하지 않음
            return;
        }

        // 쿨다운 확인 및 자동 발사
        if (GunState == State.Ready && Time.time >= lastFireTime + cooldownTime)
        {
            lastFireTime = Time.time;
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        // 타겟팅된 적을 찾음
        Transform target = FindTarget();

        // 만약 적이 발견되면 해당 방향으로 총알 발사
        if (target != null)
        {
            Vector3 direction = (target.position - firePoint.position).normalized;
            FireAtTarget(direction);
        }
        else
        {
            // 타겟이 없으면 정면으로 발사
            FireAtTarget(firePoint.forward);
        }
    }

    private Transform FindTarget()
    {
        // 레이캐스트로 타겟 찾기
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, targetingRange, targetLayer))
        {
            return hit.transform;  // 타겟이 발견되면 해당 Transform 반환
        }

        return null;  // 타겟이 없으면 null 반환
    }

    private void FireAtTarget(Vector3 direction)
    {
        // Object Pool에서 총알 가져오기
        GameObject bullet = bulletPool.Get();
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);

        // 총알 스크립트 설정
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Launch(direction, bulletPool); // 풀을 함께 전달
        }
        
        // 발사 이펙트와 사운드 처리 (옵션)
        if (muzzleEffect != null)
        {
            muzzleEffect.Play();
        }

        if (audioSource != null)
        {
            audioSource.Play();  // 총소리 (예시)
        }
    }
}
