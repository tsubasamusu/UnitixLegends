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

    [SerializeField]
    private UIManager uiManager;//UIManager

    [SerializeField]
    private float fallSpeed;//�������x

    [SerializeField, Header("�˒�����")]
    private float range;//�˒�����

    [SerializeField]
    private float getItemLength;//�A�C�e�����擾�ł��鋗��

    [SerializeField]
    private Transform enemyWeapon;//Enemy��������\����ʒu

    private bool didPostLandingProcessing;//���n����̏������s�������ǂ���

    private bool gotItem;//�A�C�e�����擾�������ǂ���

    private bool stopFlag;//�������~���邩�ǂ���

    private float enemyhp = 100.0f;//Enemy�̗̑�

    private Vector3 firstPos;//�����ʒu

    private ItemDataSO.ItemData usedItemdata;//�g�p���Ă���A�C�e���̃f�[�^

    private int nearItemNo;//�ł��߂��ɂ���g�p�\�A�C�e���̔ԍ�

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //NavMeshAgent�𖳌���
        agent.enabled = false;
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
            transform.Translate(0, -fallSpeed, 0);
        }
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
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

        //�܂��A�C�e�����擾���Ă��Ȃ��Ȃ�
        if(!gotItem)
        {
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
                agent.stoppingDistance = 5f;
            }

            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�ː���ɓG��������
        if (CheckEnemy()) 
        {
            //�ˌ�����
            ShotBullet();
        }
       
    }

    /// <summary>
    /// �ł��߂��ɂ���g�p�\�A�C�e���̔ԍ���ݒ肷��
    /// </summary>
    private void SetNearItemNo()
    {
        //null�G���[���
        if (GameData.instance.generatedItemTranList.Count <= 0)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

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
        float tolerance = 0.3f;

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
        Ray ray = new Ray(enemyWeapon.position, enemyWeapon.forward);

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
        usedItemdata=GameData.instance.generatedItemDataList[nearItemNo];

        //�擾�����A�C�e����z�u
        Instantiate(GameData.instance.generatedItemDataList[nearItemNo].prefab, enemyWeapon);

        //�A�C�e�����E��
        GameData.instance.GetItem(nearItemNo, false);

        //true��Ԃ�
        return true;
    }

    /// <summary>
    /// �e������
    /// </summary>
    private void ShotBullet()
    {
        Debug.Log("����");
        //TODO:�e��������
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
            case ("Grenade"):
                UpdateEnemyHp(-itemDataSO.itemDataList[1].attackPower, collision,false);
                break;

            //�×ܒe�Ȃ�
            case ("TearGasGrenade"):
                UpdateEnemyHp(-itemDataSO.itemDataList[2].attackPower, collision,false);
                StartCoroutine(AttackedByTearGasGrenade());
                break;

            //�i�C�t�Ȃ�
            case ("Knife"):
                UpdateEnemyHp(-itemDataSO.itemDataList[3].attackPower,collision,false);
                break;

            //�o�b�g�Ȃ�
            case ("Bat"):
                UpdateEnemyHp(-itemDataSO.itemDataList[4].attackPower,collision,false);
                break;

            //�A�T���g�Ȃ�
            case ("Assault"):
                UpdateEnemyHp(-itemDataSO.itemDataList[5].attackPower, collision, true);
                break;

            //�V���b�g�K���Ȃ�
            case ("Shotgun"):
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
        //Enemy�̗̑͂�0�ȏ�100�ȉ��ɐ������Ȃ���A�X�V����
        enemyhp = Mathf.Clamp(enemyhp + updateValue, 0f, 100f);

        //�Փˑ���̐e��PlayerTran�Ȃ�
        if(collision.transform.parent.gameObject.CompareTag("PlayerTran"))
        {
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
            //���S�������s��
            WasKilled();
        }
    }

    /// <summary>
    /// �×ܒe���󂯂��ۂ̏���
    /// </summary>
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
    /// ���S����
    /// </summary>
    private void WasKilled()
    {
        //TODO:���S����
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
