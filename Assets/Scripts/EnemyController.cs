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
    private float fallSpeed;//�������x

    private bool componentFlag;//�R���|�[�l���g�֘A�̏������s�������ǂ����̔��f

    private float enemyhp = 100.0f;//Enemy�̗̑�

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
        if(!CheckGrounded())
        {
            //��������
            transform.Translate(0,-fallSpeed, 0);
        }
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�ڒn���Ă��Ȃ��Ȃ�
        if(!CheckGrounded())
        {
            //�ȉ��̏������s��Ȃ�
            return;
        }

        //�܂��R���|�[�l���g�̏������s���Ă��Ȃ��Ȃ�
        if(!componentFlag)
        {
            //NavMeshAgent��L����
            agent.enabled=true;

            //�R���|�[�l���g�̏���������������Ԃɐ؂�ւ���
            componentFlag=true;
        }
    }

    /// <summary>
    /// �ł��߂��ɂ���g�p�\�A�C�e���̈ʒu����Ԃ�
    /// </summary>
    /// <returns></returns>
    public Transform FindNearItem()
    {
        //TODO:GameData�̃A�C�e���̃��X�g�����ɁA�ł��߂��ɂ���U���\�A�C�e���������鏈��

        return null;//�i���j
    }

    /// <summary>
    /// �ł��߂��ɂ���G�̈ʒu����Ԃ�
    /// </summary>
    /// <returns></returns>
    public Transform FindNearEnemy()
    {
        //TODO:EnemyGenerator�̓G�̃��X�g�����ɁA�ł��߂��ɂ���G�������鏈��

        return null ;//�i���j
    }

    /// <summary>
    /// �󂯎�����ʒu���̖ڕW�l�ɐݒ肷��
    /// </summary>
    /// <param name="targetTran"></param>
    public void SetTargetPosition(Transform targetTran)
    {
        //���������ɁAAI�̖ڕW�n�_��ݒ�
        agent.destination = targetTran.position;
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
    /// �A�C�e�����E���I������true��Ԃ�
    /// </summary>
    /// <returns></returns>
    public bool GetItem()
    {
        //TODO:�A�C�e�����E������

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
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        //�G�ꂽ�Q�[���I�u�W�F�N�g�̃^�O�ɉ����ď�����ύX
        switch (collision.gameObject.tag)
        {
            case ("Grenade"):
                UpdateEnemyHp(-30.0f, collision);
                break;
            case ("TearGasGrenade"):
                UpdateEnemyHp(0, collision);
                StartCoroutine( AttackedByTearGasGrenade());
                break;
            case ("Knife"):
                UpdateEnemyHp(-100.0f, collision);
                break;
            case ("Bat"):
                UpdateEnemyHp(-50.0f, collision);
                break;
            case ("Assault"):
                UpdateEnemyHp(-1.0f, collision);
                break;
            case ("Sniper"):
                UpdateEnemyHp(-80.0f, collision);
                break;
            case ("Shotgun"):
                UpdateEnemyHp(-30.0f, collision);
                break;
        }
    }

    /// <summary>
    /// Enemy�̗̑͂��X�V����
    /// </summary>
    private void UpdateEnemyHp(float updateValue, Collision collision)
    {
        //Enemy�̗̑͂�0�ȏ�100�ȉ��ɐ������Ȃ���A�X�V����
        enemyhp = Mathf.Clamp(enemyhp + updateValue, 0.0f, 100.0f);

        //null�G���[���
        if(collision != null)
        {
            //�G�ꂽ���������
            Destroy(collision.gameObject);
        }

        //Enemy�̗̑͂�0�ɂȂ�����
        if(enemyhp==0.0f)
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
}
