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

    [SerializeField] public GameObject bulletPrefab; // 子弹预制体
    public Transform firePoint; // 子弹的发射位置

    private float fireCoolDown; // 冷却
    private float skillCoolDown; // 冷却
    [SerializeField] private float fireCoolTime;
    [SerializeField] private float skillCoolTime;

    [SerializeField] private int BulletMaxNum; // 子弹数
    private int BulletNowNum;

    private Animator anim;

    [SerializeField] private Transform cursor;

    // 引用TextMeshPro的变量
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI ammoCursorText; // 鼠标旁的子弹数量显示

    // 文本跟随光标的移动速度
    [SerializeField] private float textMoveSpeed = 0.5f;

    [SerializeField] private Animator player;

    [SerializeField] private Light2D gunLight; // 引用灯光

    [SerializeField] private AudioDefination audioShoot; //射击音效
    [SerializeField] private AudioDefination audioReload; //换弹音效

    void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
        BulletNowNum = BulletMaxNum;

        gunLight.enabled = false; // 开始时禁用灯光

        // 初始化显示子弹数量
        UpdateAmmoText();
    }

    void Update()
    {
        ArrowRotation();
        if (Input.GetMouseButton(0) && fireCoolDown < 0 && BulletNowNum > 0) // 当玩家按下鼠标左键时，双枪发射子弹
        {
            fireCoolDown = fireCoolTime;
            BulletNowNum--;
            Shoot();

            audioShoot.PlayAudioClip(); //音效
        }
        else if (Input.GetMouseButtonUp(1) && fireCoolDown < 0 && BulletNowNum > 0&&skillCoolDown<0) //鼠标右键松开释放技能
        {
            fireCoolDown = fireCoolTime;
            skillCoolDown = skillCoolTime;
            player.SetTrigger("skill");
            for(int i=1;i<=4*BulletNowNum;i++)
            {
                //当前武器的技能发射子弹
                float angle =(90f- i * (180f / (4 * BulletNowNum)));
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                Instantiate(bulletPrefab, firePoint.position, rotation);
               
               
            }
            BulletNowNum = 0;
            audioShoot.PlayAudioClip(); //音效

            //调用抖动
            GameController.camShake.Shake();

            // 激活灯光
            gunLight.enabled = true;

            // 启动协程禁用灯光
            StartCoroutine(DisableLightAfterDelay(0.2f)); //灯光保持0.2秒
        }
        fireCoolDown -= Time.deltaTime;
        skillCoolDown -= Time.deltaTime;
        AnimationControllers();
        UpdateAmmoText(); // 更新显示子弹数量
    }

    private void ArrowRotation()
    {
        Vector3 direction = (cursor.position - player_transform.position).normalized;

        // 计算武器的目标位置
        Vector3 weaponPosition = player_transform.position + direction * distanceFromPlayer;

        transform.position = weaponPosition; // 设置武器的位置

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void Shoot()
    {
        // 实例化子弹预制体并设置发射位置，对玩家来说有一定的夹角
        float angle = 10f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Instantiate(bulletPrefab, firePoint.position, rotation);
        angle = -10f;
        rotation = Quaternion.Euler(0, 0, angle);
        Instantiate(bulletPrefab, firePoint.position, rotation);
        //调用抖动
        GameController.camShake.Shake();
        // 激活灯光
        gunLight.enabled = true;

        // 启动协程禁用灯光
        StartCoroutine(DisableLightAfterDelay(0.2f)); //灯光保持0.2秒
    }
    private IEnumerator DisableLightAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gunLight.enabled = false; // 禁用灯光
    }

    private void AnimationControllers()
    {
        anim.SetInteger("BulletNowNum", BulletNowNum);
    }

    public void ReplaceBullet()
    {
        BulletNowNum = BulletMaxNum;
        UpdateAmmoText(); // 更新子弹数量显示
    }

    // 更新TextMeshPro显示子弹数量
    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = "00" + BulletNowNum.ToString() + "/00" + BulletMaxNum.ToString();
        }
        if (ammoCursorText != null)
        {
            // 获取靶心在摄像机中的坐标
            Vector3 screenPosition = cam.WorldToScreenPoint(cursor.transform.position);
            // 更新靶心旁边的子弹数量文本
            ammoCursorText.text = BulletNowNum.ToString();
            // 使用插值平滑移动文本
            Vector3 targetPosition = screenPosition + new Vector3(15f, -15f, 0f);

            ammoCursorText.transform.position = Vector3.Lerp(ammoCursorText.transform.position, targetPosition, textMoveSpeed * Time.deltaTime);
        }


    }

    private void PlayReloadSound()
    {
        audioReload.PlayAudioClip();
    }
}
