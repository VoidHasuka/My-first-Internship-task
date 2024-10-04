using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
public class Player_Arrow : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Transform player_transform;
    private float distanceFromPlayer = 1f;

    [SerializeField] public GameObject bulletPrefab; // �ӵ�Ԥ����
    public Transform firePoint; // �ӵ��ķ���λ��

    private float fireCoolDown; // ��ȴ
    private float skillCoolDown; // ��ȴ
    [SerializeField] private float fireCoolTime;
    [SerializeField] private float skillCoolTime;

    [SerializeField] private int BulletMaxNum; // �ӵ���
    private int BulletNowNum;

    private Animator anim;

    [SerializeField] private Transform cursor;

    // ����TextMeshPro�ı���
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI ammoCursorText; // ����Ե��ӵ�������ʾ

    // �ı���������ƶ��ٶ�
    [SerializeField] private float textMoveSpeed = 0.5f;

    [SerializeField] private Animator player;

    [SerializeField] private Light2D gunLight; // ���õƹ�

    [SerializeField] private AudioDefination audioShoot; //�����Ч
    [SerializeField] private AudioDefination audioReload; //������Ч

    void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
        BulletNowNum = BulletMaxNum;

        gunLight.enabled = false; // ��ʼʱ���õƹ�

        // ��ʼ����ʾ�ӵ�����
        UpdateAmmoText();
    }

    void Update()
    {
        ArrowRotation();
        if (Input.GetMouseButton(0) && fireCoolDown < 0 && BulletNowNum > 0) // ����Ұ���������ʱ��˫ǹ�����ӵ�
        {
            fireCoolDown = fireCoolTime;
            BulletNowNum--;
            Shoot();

            audioShoot.PlayAudioClip(); //��Ч
        }
        else if (Input.GetMouseButtonUp(1) && fireCoolDown < 0 && BulletNowNum > 0&&skillCoolDown<0) //����Ҽ��ɿ��ͷż���
        {
            fireCoolDown = fireCoolTime;
            skillCoolDown = skillCoolTime;
            player.SetTrigger("skill");
            for(int i=1;i<=4*BulletNowNum;i++)
            {
                //��ǰ�����ļ��ܷ����ӵ�
                float angle =(90f- i * (180f / (4 * BulletNowNum)));
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Instantiate(bulletPrefab, firePoint.position, rotation);
               
               
            }
            BulletNowNum = 0;
            audioShoot.PlayAudioClip(); //��Ч

            //���ö���
            GameController.camShake.Shake();

            // ����ƹ�
            gunLight.enabled = true;

            // ����Э�̽��õƹ�
            StartCoroutine(DisableLightAfterDelay(0.2f)); //�ƹⱣ��0.2��
        }
        fireCoolDown -= Time.deltaTime;
        skillCoolDown -= Time.deltaTime;
        AnimationControllers();
        UpdateAmmoText(); // ������ʾ�ӵ�����
    }

    private void ArrowRotation()
    {
        Vector3 direction = (cursor.position - player_transform.position).normalized;

        // ����������Ŀ��λ��
        Vector3 weaponPosition = player_transform.position + direction * distanceFromPlayer;

        transform.position = weaponPosition; // ����������λ��

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Shoot()
    {
        // ʵ�����ӵ�Ԥ���岢���÷���λ�ã��������˵��һ���ļн�
        float angle = 10f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Instantiate(bulletPrefab, firePoint.position, rotation);
        angle = -10f;
        rotation = Quaternion.Euler(0, 0, angle);
        Instantiate(bulletPrefab, firePoint.position, rotation);
        //���ö���
        GameController.camShake.Shake();
        // ����ƹ�
        gunLight.enabled = true;

        // ����Э�̽��õƹ�
        StartCoroutine(DisableLightAfterDelay(0.2f)); //�ƹⱣ��0.2��
    }
    private IEnumerator DisableLightAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gunLight.enabled = false; // ���õƹ�
    }

    private void AnimationControllers()
    {
        anim.SetInteger("BulletNowNum", BulletNowNum);
    }

    public void ReplaceBullet()
    {
        BulletNowNum = BulletMaxNum;
        UpdateAmmoText(); // �����ӵ�������ʾ
    }

    // ����TextMeshPro��ʾ�ӵ�����
    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = "00" + BulletNowNum.ToString() + "/00" + BulletMaxNum.ToString();
        }
        if (ammoCursorText != null)
        {
            // ��ȡ������������е�����
            Vector3 screenPosition = cam.WorldToScreenPoint(cursor.transform.position);
            // ���°����Աߵ��ӵ������ı�
            ammoCursorText.text = BulletNowNum.ToString();
            // ʹ�ò�ֵƽ���ƶ��ı�
            Vector3 targetPosition = screenPosition + new Vector3(15f, -15f, 0f);

            ammoCursorText.transform.position = Vector3.Lerp(ammoCursorText.transform.position, targetPosition, textMoveSpeed * Time.deltaTime);
        }


    }

    private void PlayReloadSound()
    {
        audioReload.PlayAudioClip();
    }
}
