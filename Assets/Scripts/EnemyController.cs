using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshAgent���g�p

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;//NavMeshAgent

    [SerializeField]
    private Animator animator;//Animator

    [SerializeField]
    private ItemDataSO itemDataSO;//ItemDataSO

    [SerializeField, Header("�˒�����")]
    private float range;//�˒�����

    [SerializeField]
    private float stoppingDistance;//�G�Ƃ̋���

    [SerializeField]
    private float getItemLength;//�A�C�e�����擾�ł��鋗��

    [SerializeField,Header("���u�ւ̈ړ��w�����o�������鎞��")]
    private float instructionTime;//���u�ւ̈ړ��w�����o�������鎞��

    [SerializeField]
    private Transform enemyWeaponTran;//Enemy��������\����ʒu

    private bool didPostLandingProcessing;//���n����̏������s�������ǂ���

    private bool gotItem;//�A�C�e�����擾�������ǂ���

    private bool stopFlag;//�������~���邩�ǂ���

    private bool goToEnshrine;//���u�Ɉړ����邩�ǂ���

    private float enemyhp = 100f;//Enemy�̗̑�

    private float timer;//����p�̌o�ߎ���

    private float stormTimer;//�X�g�[���̒��ɂ���Ԃ̌o�ߎ���

    private Vector3 firstPos;//�����ʒu

    private ItemDataSO.ItemData usedItemData;//�g�p���Ă���A�C�e���̃f�[�^

    private UIManager uiManager;//UIManager

    private EnemyGenerator enemyGenerator;//EnemyGenerator

    private StormController stormController;//StormController

    private PlayerHealth playerHealth;//PlayerHealth

    private Transform shotBulletTran;//�e�𐶐�����ʒu

    private Transform playerTran;//Player�̈ʒu

    private GameObject usedItemObj;//�g�p���Ă���A�C�e���̃I�u�W�F�N�g

    private int nearItemNo;//�ł��߂��ɂ���g�p�\�A�C�e���̔ԍ�

    private int myNo;//�������g�̔ԍ�

    public int MyNo//myNo�ϐ��p�̃v���p�e�B
    {
        set { myNo = value; }//�O������͐ݒ菈���݂̂��\��
    }

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //NavMeshAgent�𖳌���
        agent.enabled = false;

        //�o�ߎ��Ԃ��v��
        StartCoroutine(MeasureTime());

        //UIManager���擾
        if (!GameObject.Find("UIManager").TryGetComponent(out uiManager))
        {
            //�����
            Debug.Log("UIManager�̎擾�Ɏ��s");
        }

        //EnemyGenerator���擾
        if(!GameObject.Find("EnemyGenerator").TryGetComponent(out enemyGenerator))
        {
            //�����
            Debug.Log("EnemyGenerator�̎擾�Ɏ��s");
        }

        //StormController���擾
        if (!GameObject.Find("Storm").TryGetComponent(out stormController))
        {
            //�����
            Debug.Log("StormController�̎擾�Ɏ��s");
        }

        //PlayerHealth���擾
        if (!GameObject.Find("Player").TryGetComponent(out playerHealth))
        {
            //�����
            Debug.Log("PlayerHealth�̎擾�Ɏ��s");
        }

        //Player�̈ʒu�����擾
        if (!GameObject.Find("Player").TryGetComponent(out playerTran))
        {
            //�����
            Debug.Log("Player�̈ʒu���̎擾�Ɏ��s");
        }

        //���ˈʒu���擾
        shotBulletTran = transform.GetChild(3).transform;
    }

    /// <summary>
    /// ��莞�Ԃ��ƂɌĂяo�����
    /// </summary>
    private void FixedUpdate()//�S�Ă̒[���œ����ړ����x�ɂ��邽�߂�FixedUpdate���\�b�h���g��
    {
        //�ڒn���Ă��Ȃ�������
        if (!CheckGrounded())
        {
            //��������
            transform.Translate(0, -GameData.instance.FallSpeed, 0);
        }
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�����E�ɍs���Ă��܂�����
        if (transform.position.y <= -1f)
        {
            //�����
            Debug.Log("Enemy�������E�ɗ���");

            //���g�̍��W��(0,0,0)�ɐݒ�
            transform.position = Vector3.zero;
        }

        //�ڒn���Ă��Ȃ��Ȃ�
        if (!CheckGrounded())
        {
            //�ȉ��̏������s��Ȃ�
            return;
        }

        //�܂����n����̏������s���Ă��Ȃ��Ȃ�
        if (!didPostLandingProcessing)
        {
            //NavMeshAgent��L����
            agent.enabled = true;

            //��~������0�ɐݒ�
            agent.stoppingDistance = 0f;

            //�A�j���[�V�����̐�����J�n
            StartCoroutine(ControlAnimation());

            //���n����̏���������������Ԃɐ؂�ւ���
            didPostLandingProcessing = true;
        }

        //��~��ԂȂ�
        if(stopFlag)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //���u�ւ̈ړ��w�����o�Ă���Ȃ�
        if (goToEnshrine)
        {
            //�����֌�����
            SetTargetPosition(Vector3.zero);

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //���u�O�ɂ���Ȃ�
        if (!stormController.CheckEnshrine(transform.position))
        {
            //�X�g�[���ɂ���Ԃ̌o�ߎ��Ԃ��v��
            stormTimer+=Time.deltaTime;

            //�o�ߎ��Ԃ���莞�Ԃ𒴂�����
            if(stormTimer>=(100f/playerHealth.StormDamage))
            {
                //����
                KillMe();
            }


            //��莞�ԁA���u�ւ̈ړ��w���o��
            StartCoroutine(GoToEnshrine());

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�܂��A�C�e�����擾���Ă��Ȃ��Ȃ�
        if(!gotItem)
        {
            //���������A�C�e���̃��X�g�̗v�f��0�Ȃ�
            if (GameData.instance.generatedItemTranList.Count <= 0)//null�G���[���
            {
                //�����
                Debug.Log("�A�C�e������������܂���");

                //�ȍ~�̏������s��Ȃ�
                return;
            }

            //�ł��߂��ɂ���g�p�\�A�C�e���̔ԍ���ݒ�
            SetNearItemNo();

            //�ڕW�n�_��ݒ�
            SetTargetPosition(GameData.instance.generatedItemTranList[nearItemNo].position);

            //�A�C�e�����擾�ł��鋗���܂ŋ߂Â�����
            if (GetLengthToNearItem(nearItemNo) <= getItemLength)
            {
                //�A�C�e�����擾���A�A�C�e�����擾�ς݂̏�Ԃɐ؂�ւ���
                gotItem = GetItem(nearItemNo);

                //��~������ݒ�
                agent.stoppingDistance = stoppingDistance;
            }

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�g�p�ł��Ȃ��A�C�e�����E���Ă��܂�����
        if(!usedItemData.enemyCanUse)
        {
            //�A�C�e�����܂��擾���Ă��Ȃ���Ԃɐ؂�ւ���
            gotItem = false;

            //��~������0�ɐݒ�
            agent.stoppingDistance = 0f;

            //�g�p���Ă���A�C�e���̃I�u�W�F�N�g������
            Destroy(usedItemObj);

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //Enemy��Player�����݂��Ă��Ȃ�������
        if (enemyGenerator.generatedEnemyList.Count <= 0 || playerTran.gameObject == null)//null�G���[���
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�G�̈ʒu��ڕW�n�_�ɐݒ�
        SetTargetPosition(GetNearEnemyPos());

        //�ː���ɓG��������
        if (CheckEnemy()) 
        {
            //�ˌ�����
            ShotBullet(usedItemData);
        }
    }

    /// <summary>
    /// ��莞�ԁA���u�֌�����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator GoToEnshrine()
    {
        //���u�Ɍ������悤�w�����o��
        goToEnshrine = true;

        //��莞�ԁA�ړ��w�����o��������
        yield return new WaitForSeconds(instructionTime);

        //�ړ��w��������
        goToEnshrine = false;
    }

    /// <summary>
    /// �ł��߂��ɂ���G�̈ʒu���擾����
    /// </summary>
    /// <returns>�ł��߂��ɂ���G�̈ʒu</returns>
    private Vector3 GetNearEnemyPos()
    {
        //�ł��߂��ɂ���G�̈ʒu��Player�̈ʒu�����ɓo�^
        Vector3 nearPos = playerTran. position;

        //��������Enemy�̈ʒu���̃��X�g�̗v�f�������J��Ԃ�
        for (int i = 0; i < enemyGenerator.generatedEnemyList.Count; i++)
        {
            //�J��Ԃ������Ŏ擾�����G��������������
            if (i==myNo)
            {
                //���̌J��Ԃ������Ɉڂ�
                continue;
            }

            //�J��Ԃ������Ŏ擾�����G�����S���Ă�����
            if (enemyGenerator.generatedEnemyList[i]==null)//null�G���[���
            {
                //���̌J��Ԃ������Ɉڂ�
                continue;
            }

            //���X�g��i�Ԃ̗v�f�̍��W��pos�ɓo�^
            Vector3 pos = enemyGenerator.generatedEnemyList[i].transform.position;

            //���o�^�����v�f�ƁAfor���œ����v�f�́AmyPos�Ƃ̋������r
            if (Vector3.Scale((pos - transform.position), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - transform.position), new Vector3(1, 0, 1)).magnitude)
            {
                //nearPos���ēo�^
                nearPos = pos;
            }
        }

        //nearPos��Ԃ�
        return nearPos;
    }

    /// <summary>
    /// �ł��߂��ɂ���g�p�\�A�C�e���̔ԍ���ݒ肷��
    /// </summary>
    private void SetNearItemNo()
    {
        //�A�C�e���̔ԍ�
        int itemNo = 0;

        //���X�g��0�Ԃ̗v�f�̍��W��nearPos�ɉ��ɓo�^
        Vector3 nearPos = GameData.instance.generatedItemTranList[0].position;

        //���X�g�̗v�f�������J��Ԃ�
        for (int i = 0; i < GameData.instance.generatedItemTranList.Count; i++)
        {
            //�J��Ԃ������Ō������A�C�e�����g�p�s��������
            if (!GameData.instance.generatedItemDataList[i].enemyCanUse)
            {
                //�ȍ~�̏����͍s�킸�ɁA���̌J��Ԃ��Ɉڂ�
                continue;
            }

            //���X�g��i�Ԃ̗v�f�̍��W��pos�ɓo�^
            Vector3 pos = GameData.instance.generatedItemTranList[i].position;

            //���o�^�����v�f�ƁAfor���œ����v�f�́AmyPos�Ƃ̋������r
            if (Vector3.Scale((pos - transform.position), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - transform.position), new Vector3(1, 0, 1)).magnitude)
            {
                //Player�̍ł��߂��ɂ���A�C�e���̔ԍ���i�œo�^
                itemNo = i;

                //nearPos���ēo�^
                nearPos = pos;
            }
        }

        //�ł��߂��ɂ���g�p�\�A�C�e���̔ԍ���ݒ�
        nearItemNo= itemNo;
    }

    /// <summary>
    /// �ł��߂��ɂ���g�p�\�A�C�e���Ƃ̋������擾
    /// </summary>
    /// <param name="nearItemNo">�ł��߂��ɂ���g�p�\�A�C�e���̔ԍ�</param>
    /// <returns>�ł��߂��ɂ���g�p�\�A�C�e���Ƃ̋���</returns>
    private float GetLengthToNearItem(int nearItemNo)
    {
        //�ł��߂��ɂ���g�p�\�A�C�e���Ƃ̋�����Ԃ�
        return Vector3.Scale((GameData.instance.generatedItemTranList[nearItemNo].position - transform.position), new Vector3(1, 0, 1)).magnitude;
    }

    /// <summary>
    /// �ڕW�n�_��ݒ肷��
    /// </summary>
    /// <param name="targetPos">�ڕW�n�_</param>
    private void SetTargetPosition(Vector3 targetPos)
    {
        //���������ɁAAI�̖ڕW�n�_��ݒ�
        agent.destination = targetPos;
    }

    /// <summary>
	/// �ڒn������s��
	/// </summary>
	/// <returns>�ڒn���Ă�����true</returns>
	private bool CheckGrounded()
    {
        //ray�̏����ʒu�ƌ����i�p���j��ݒ�
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        //ray�̒T�������i�����j��ݒ�
        float tolerance = 0.2f;

        //ray�̃q�b�g����ibool�^�j��Ԃ�
        return Physics.Raycast(ray, tolerance);
    }

    /// <summary>
    /// �ː���ɓG�����邩�ǂ������ׂ�
    /// </summary>
    /// <returns>�ː���ɓG��������true</returns>
    private bool CheckEnemy()
    {
        //�����̏����ʒu�ƌ����i�p���j��ݒ�
        Ray ray = new Ray(enemyWeaponTran.position, enemyWeaponTran.forward);

        //���������ɂ�������Ȃ�������
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, range))
        {
            //false��Ԃ��A�ȍ~�̏������s��Ȃ�
            return false;
        }

        //������Player��Enemy�ɓ���������
        if (hitInfo.transform.gameObject.CompareTag("Player") || hitInfo.transform.gameObject.CompareTag("Enemy"))
        {
            //true��Ԃ�
            return true;
        }

        //Player��Enemy�ȊO�Ɍ���������������false��Ԃ�
        return false;
    }

    /// <summary>
    /// �A�C�e�����E��
    /// </summary>
    /// <param name="nearItemNo">�߂��̃A�C�e���̔ԍ�</param>
    /// <returns>�A�C�e�����E���I������true��Ԃ�</returns>
    public bool GetItem(int nearItemNo)
    {
        //�g�p����A�C�e���̃f�[�^��ݒ�
        usedItemData=GameData.instance.generatedItemDataList[nearItemNo];

        //�擾�����A�C�e����z�u
        usedItemObj= Instantiate(GameData.instance.generatedItemDataList[nearItemNo].prefab, enemyWeaponTran);

        //�A�C�e�����E��
        GameData.instance.GetItem(nearItemNo, false);

        //true��Ԃ�
        return true;
    }

    /// <summary>
    /// �o�ߎ��Ԃ��v������
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator MeasureTime()
    {
        //�������[�v
        while (true)
        {
            //�o�ߎ��Ԃ��v��
            timer += Time.deltaTime;

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }

    /// <summary>
    /// �ˌ�����
    /// </summary>
    /// <param name="itemData">�g�p����A�C�e���̃f�[�^</param>
    private void ShotBullet(ItemDataSO.ItemData itemData)
    {
        //�o�ߎ��Ԃ��A�ˊԊu��菬�����Ȃ�
        if (timer < itemData.interval)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�e�𐶐�
        Rigidbody bulletRb = Instantiate(itemData.bulletPrefab, shotBulletTran.position,Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0));

        //���������e�̐e�����g�ɐݒ�
        bulletRb.transform.parent = transform;

        //�e�𔭎�
        bulletRb.AddForce(enemyWeaponTran.forward * itemData.shotSpeed);

        //�o�ߎ��Ԃ�������
        timer = 0;

        //���˂����e��3.0�b��ɏ���
        Destroy(bulletRb.gameObject, 3.0f);
    }

    /// <summary>
    /// ���̃R���C�_�[�ɐG�ꂽ�ۂɌĂяo�����
    /// </summary>
    /// <param name="collision">�G�ꂽ����</param>
    private void OnCollisionEnter(Collision collision)
    {
        //�G�ꂽ�Q�[���I�u�W�F�N�g�̃^�O�ɉ����ď�����ύX
        switch (collision.gameObject.tag)
        {
            //��֒e�Ȃ�
            case "Grenade":
                UpdateEnemyHp(-itemDataSO.itemDataList[1].attackPower, collision,false);
                break;

            //�×ܒe�Ȃ�
            case "TearGasGrenade":
                UpdateEnemyHp(-itemDataSO.itemDataList[2].attackPower, collision,false);
                StartCoroutine(AttackedByTearGasGrenade());
                break;

            //�i�C�t�Ȃ�
            case "Knife":
                UpdateEnemyHp(-itemDataSO.itemDataList[3].attackPower,collision,false);
                break;

            //�o�b�g�Ȃ�
            case "Bat":
                UpdateEnemyHp(-itemDataSO.itemDataList[4].attackPower,collision,false);
                break;

            //�A�T���g�Ȃ�
            case "Assault":
                UpdateEnemyHp(-itemDataSO.itemDataList[5].attackPower, collision, true);
                break;

            //�V���b�g�K���Ȃ�
            case "Shotgun":
                UpdateEnemyHp(-itemDataSO.itemDataList[6].attackPower, collision, true);
                break;

            //�X�i�C�p�[�Ȃ�
            case ("Sniper"):
                UpdateEnemyHp(-itemDataSO.itemDataList[7].attackPower, collision, true);
                break;
        }
    }

    /// <summary>
    /// Enemy��Hp���X�V
    /// </summary>
    /// <param name="updateValue">Enemy��Hp�̍X�V��</param>
    /// <param name="collision">�Փˑ���</param>
    /// <param name="destoryFlag">�Փˑ�����������ǂ���</param>
    private void UpdateEnemyHp(float updateValue, Collision collision,bool destoryFlag)
    {
        //�������������e���e�����Ȃ�
        if(collision.transform.parent==transform)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�U�����Ă������肪Player���ǂ���
        bool isPlayer = false;

        //Enemy�̗̑͂�0�ȏ�100�ȉ��ɐ������Ȃ���A�X�V����
        enemyhp = Mathf.Clamp(enemyhp + updateValue, 0f, 100f);

        //�Փˑ���̐e��PlayerTran�Ȃ�
        if(collision.transform.parent.gameObject.CompareTag("PlayerTran"))
        {
            //�U�����Ă��������Player�ɐݒ�
            isPlayer = true;

            //�t���[�g�\���𐶐�
            StartCoroutine(uiManager.GenerateFloatingMessage(Mathf.Abs(updateValue).ToString("F0"), Color.yellow));
        }

        //�Փˑ���������Ƃ����w���Ȃ�
        if (destoryFlag)
        {
            //�G�ꂽ���������
            Destroy(collision.gameObject);
        }

        //Enemy�̗̑͂�0�ɂȂ�����
        if (enemyhp == 0.0f)
        {
            //�U�����Ă������肪Player�Ȃ�
            if(isPlayer)
            {
                //Player���|�����G�̐���1��������
                GameData.instance.KillCount++;
            }

            //����
            KillMe();
        }
    }

    /// <summary>
    /// ���ʍۂ̏���
    /// </summary>
    private void KillMe()
    {
        //���g���Q�[���I�u�W�F�N�g���Ə���
        Destroy(gameObject);
    }

   /// <summary>
   /// �×ܒe���󂯂��ۂ̏���
   /// </summary>
   /// <returns>�҂�����</returns>
    private IEnumerator AttackedByTearGasGrenade()
    {
        //��~��Ԃɂ���
        stopFlag = true;

        //�ڕW�n�_�����g�̍��W�ɐݒ�
        SetTargetPosition(transform.position);

        //5.0�b�ԁA�������~�ߑ�����
        yield return new WaitForSeconds(5.0f);

        //��~��Ԃ���������
        stopFlag = false;
    }

    /// <summary>
    /// �A�j���[�V�����𐧌䂷��
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator ControlAnimation()
    {
        //�����ɌJ��Ԃ�
        while (true)
        {
            //�����ʒu��ݒ�
            firstPos = transform.position;

            //0.1�b�҂�
            yield return new WaitForSeconds(0.1f);

            //���݈ʒu��ݒ�
            Vector3 currentPos = transform.position;

            //���x���擾
            float velocity = (currentPos - firstPos).magnitude /0.1f;

            //����
            animator.SetBool("MovePrevious", velocity > 0.1f);

            //�������Ȃ�
            animator.SetBool("Idle", velocity <= 0.1f);

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }
}
