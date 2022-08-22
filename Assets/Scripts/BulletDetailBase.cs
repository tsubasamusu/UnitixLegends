using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �C���^�t�F�[�X
//https://naoyu.dev/%E3%80%90unity%E3%80%91%E3%82%AF%E3%83%A9%E3%82%B9%E8%A8%AD%E8%A8%88%E3%81%AB%E3%81%8A%E3%81%91%E3%82%8B%E3%82%A4%E3%83%B3%E3%82%BF%E3%83%BC%E3%83%95%E3%82%A7%E3%82%A4%E3%82%B9%E3%81%AE%E5%BD%B9/



public enum BulletOwnerType {
    Player,
    Enemy
}

/// <summary>
/// �o���b�g�ɃA�^�b�`����
/// </summary>
public class BulletDetailBase : MonoBehaviour
{
    private float attackPower;

    [SerializeField]
    Rigidbody rb;

    // �e�̏��L��
    private BulletOwnerType bulletOwnerType;

    public BulletOwnerType BulletOwnerType { get => bulletOwnerType; set => bulletOwnerType = value; }

    public virtual void SetUpBulletDetail(float attackPower, BulletOwnerType bulletOwnerType, Vector3 direction) {
        this.attackPower = attackPower;

        BulletOwnerType = bulletOwnerType;

        // SE �G�t�F�N�g�Đ� ���イ�e�Ƃ�


        rb.AddForce(direction);
    }


    public virtual float GetAttackPower() {

        return attackPower;
    }



    public virtual void TriggerBullet() {

        // �擾���̏���


        // SE �G�t�F�N�g�Đ�

        // �j��

    }
}
