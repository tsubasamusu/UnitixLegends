using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//Serializable�������g�p

//�A�Z�b�g���j���[�ŁuCreate ItemDataSO�v��I������ƁA�uItemDataSO�v���쐬�ł���
[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    /// <summary>
    /// �A�C�e���̖��O
    /// </summary>
    public enum ItemName
    {
        None,//���҂ł��Ȃ�
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
        [Range(0.0f, 100.0f)]
        public float restorativeValue;//�񕜗�
        [Range(0.0f, 100.0f)]
        public float attackPower;//�U����
        public float shotSpeed;//���ˑ��x
        public float reloadTime;//�����[�h����
        public float interval;//�A�ˊԊu
        public float timeToExplode;//���j�E�K�X�����܂ł̎���
        public int bulletCount;//�e�̐�
        public bool enemyCanUse;//Enemy���g�p�ł��邩�ǂ���
        public bool isNotBullet;//�e�̃A�C�e���ł͂Ȃ����ǂ���
        public bool isFirearms;//�e�Ί킩�ǂ���
        public Sprite sprite;//Sprite
        public GameObject prefab;//�v���t�@�u
        public Rigidbody bulletPrefab;//�e�̃v���t�@�u
    }

    public List<ItemData> itemDataList=new List<ItemData>();//�A�C�e���f�[�^�̃��X�g
}
