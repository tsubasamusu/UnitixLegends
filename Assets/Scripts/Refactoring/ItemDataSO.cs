using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//Serializable�������g�p

namespace yamap 
{
    //�A�Z�b�g���j���[�ŁuCreate ItemDataSO_yamap�v��I������ƁA�uItemDataSO�v���쐬�ł���
    [CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ItemDataSO_yamap")]
    public class ItemDataSO : ScriptableObject 
    {
        /// <summary>
        /// �A�C�e���̖��O
        /// </summary>
        public enum ItemName {
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
        /// �A�C�e���̎��
        /// </summary>
        public enum ItemType 
        {
            Missile,//��ѓ���
            HandWeapon,//�ߐڕ���
            Bullet,//�e
            Recovery//��
        }

        /// <summary>
        /// �A�C�e���̃f�[�^���Ǘ�����
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
            public float interval;//�A�ˊԊu
            public float timeToExplode;//���j�E�K�X�����܂ł̎���
            public int bulletCount;//�e�̐�
            public int maxBulletCount;//��x�ɏ����ł���e�̍ő吔
            public bool enemyCanUse;//Enemy���g�p�ł��邩�ǂ���
            public Sprite sprite;//Sprite
            [Header("�����Ă�����̃v���t�@�u")]
            public ItemDetail itemPrefab;//�����Ă�����̃v���t�@�u
            public int itemNo;//�A�C�e���̔ԍ�
            public ItemType itemType;//�A�C�e���̎��
            [Header("�U�����ɐ�������v���t�@�u")]
            public WeaponBase weaponPrefab;//�e�Ƌߐڕ���̃v���t�@�u
            public SeName seName;//���ʉ��̖��O
            public GameObject effectPrefab;//�G�t�F�N�g�̃v���t�@�u
        }

        public List<ItemData> itemDataList = new List<ItemData>();//�A�C�e���f�[�^�̃��X�g
    }
}