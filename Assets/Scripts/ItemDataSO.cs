using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//�A�Z�b�g���j���[�ŁuCreate ItemDataSO�v��I������ƁA�uItemDataSO�v���쐬�ł���
[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    /// <summary>
    /// �A�C�e���̖��O
    /// </summary>
    public enum ItemName
    {
        Grenade,//��֒e
        TearGasGrenade,//�×ܒe
        Knife,//�i�C�t
        Bat,//�o�b�g
        Assault,//�A�T���g���C�t��
        Shotgun,//�V���b�g�K��
        Sniper,//�X�i�C�p�[���C�t��
        Bandage,//���
        MedicinalPlants,//��
        Syringe,//���ˊ�
        AssaultBullet,//�A�T���g�p�e
        ShotgunBullet,//�V���b�g�K���p�e
        SniperBullet//�X�i�C�p�[�p�e
    }

    /// <summary>
    /// �A�C�e���̃f�[�^���Ǘ�����N���X
    /// </summary>
    [Serializable]
    public class ItemData
    {
        public ItemName itemName;//�A�C�e���̖��O
        public float restorativeValue;//�񕜗�
        public float occurrence;//�o���m��
        public float shotSpeed;//���ˑ��x
        public float attackPower;//�U����
        public float reloadTime;//�����[�h����
        public float interval;//�A�ˊԊu
        public float timeToExplode;//���j�E�K�X�����܂ł̎���
        public Sprite sprite;//Sprite
    }

    public List<ItemData> itemDataList=new List<ItemData>();//�A�C�e���f�[�^�̃��X�g
}
