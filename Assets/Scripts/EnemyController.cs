using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//NavMeshAgent���g�p

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;//NavMeshAgent

    [SerializeField]
    private float gravity;//�d��

    private bool componentFlag;//�R���|�[�l���g�֘A�̏������s�������ǂ����̔��f

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
    private void FixedUpdate()
    {
        //�ڒn���Ă��Ȃ�������
        if(!CheckGrounded())
        {
            //�d�͂𐶐�
            transform.Translate(0,-gravity, 0);
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
    /// �ł��߂��ɂ���U���\�A�C�e���̈ʒu����Ԃ�
    /// </summary>
    /// <returns></returns>
    public Transform FindNearAggressiveItem()
    {
        return null;//�i���j
    }

    /// <summary>
    /// �ł��߂��ɂ���G�̈ʒu����Ԃ�
    /// </summary>
    /// <returns></returns>
    public Transform FindNearEnemy()
    {
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
}
