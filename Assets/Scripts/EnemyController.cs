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
    private float fallSpeed;//�������x

    [SerializeField]
    private float getItemLength;//�A�C�e�����擾�ł��鋗��

    private bool didPostLandingProcessing;//���n����̏������s�������ǂ���

    private bool gotItem;//�A�C�e�����擾�������ǂ���

    private float enemyhp = 100.0f;//Enemy�̗̑�

    private float lengthToNearItem;//�߂��̎g�p�\�A�C�e���܂ł̋���

    private Vector3 firstPos;//�����ʒu

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

        //�܂��A�C�e�����擾���Ă��Ȃ�������
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
            }
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
    /// �ł��߂��ɂ���G�̈ʒu����Ԃ�
    /// </summary>
    /// <returns></returns>
    private Transform FindNearEnemy()
    {
        //TODO:EnemyGenerator�̓G�̃��X�g�����ɁA�ł��߂��ɂ���G�������鏈��

        return null;//�i���j
    }

    /// <summary>
    /// �󂯎�����ʒu���̖ڕW�l�ɐݒ肷��
    /// </summary>
    /// <param name="targetTran"></param>
    private void SetTargetPosition(Vector3 targetPos)
    {
        //���������ɁAAI�̖ڕW�n�_��ݒ�
        agent.destination = targetPos;
    }

    /// <summary>
	/// ���g���ڒn���Ă�����true��Ԃ�
	/// </summary>
	/// <returns></returns>
	public bool CheckGrounded()
    {
        //ray�̏����ʒu�ƌ����i�p���j��ݒ�
        var ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);

        //ray�̒T�������i�����j��ݒ�
        var tolerance = 0.3f;

        //ray�̃q�b�g����ibool�^�j��Ԃ�
        return Physics.Raycast(ray, tolerance);
    }

    /// <summary>
    /// �A�C�e�����E��
    /// </summary>
    /// <param name="nearItemNo">�߂��̃A�C�e���̔ԍ�</param>
    /// <returns>�A�C�e�����E���I������true��Ԃ�</returns>
    public bool GetItem(int nearItemNo)
    {
        Debug.Log("Get");

        //�A�C�e�����E��
        GameData.instance.GetItem(nearItemNo, false);

        //true��Ԃ�
        return true;
    }

    /// <summary>
    /// �e������
    /// </summary>
    public void ShotBullet()
    {
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
                UpdateEnemyHp(-itemDataSO.itemDataList[1].attackPower, collision);
                break;

            //�×ܒe�Ȃ�
            case ("TearGasGrenade"):
                UpdateEnemyHp(-itemDataSO.itemDataList[2].attackPower, collision);
                StartCoroutine(AttackedByTearGasGrenade());
                break;

            //�i�C�t�Ȃ�
            case ("Knife"):
                UpdateEnemyHp(-itemDataSO.itemDataList[3].attackPower);
                break;

            //�o�b�g�Ȃ�
            case ("Bat"):
                UpdateEnemyHp(-itemDataSO.itemDataList[4].attackPower);
                break;

            //�A�T���g�Ȃ�
            case ("Assault"):
                UpdateEnemyHp(-itemDataSO.itemDataList[5].attackPower, collision);
                break;

            //�V���b�g�K���Ȃ�
            case ("Shotgun"):
                UpdateEnemyHp(-itemDataSO.itemDataList[6].attackPower, collision);
                break;

            //�X�i�C�p�[�Ȃ�
            case ("Sniper"):
                UpdateEnemyHp(-itemDataSO.itemDataList[7].attackPower, collision);
                break;
        }
    }

    /// <summary>
    /// Enemy�̗̑͂��X�V����
    /// </summary>
    private void UpdateEnemyHp(float updateValue, Collision collision = null)
    {
        //Enemy�̗̑͂�0�ȏ�100�ȉ��ɐ������Ȃ���A�X�V����
        enemyhp = Mathf.Clamp(enemyhp + updateValue, 0f, 100f);

        //null�G���[���
        if (collision != null)
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
        //Enemy�̓������~�߂�
        agent.enabled = false;

        //5.0�b�ԁA�������~�ߑ�����
        yield return new WaitForSeconds(5.0f);

        //Enemy�̊������ĊJ����
        agent.enabled = true;
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
