using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;//Serializable�������g�p

namespace yamap {

    //�A�Z�b�g���j���[�ŁuCreate ItemDataSO�v��I������ƁA�uItemDataSO�v���쐬�ł���
    [CreateAssetMenu(fileName = "ItemDataSO", menuName = "Create ItemDataSO")]
    public class ItemDataSO : ScriptableObject {
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


        public enum ItemType {
            FireArms,
            HandWeapon,
            ThrowingWeapon,
            Bullet
        }

        /// <summary>
        /// �A�C�e���̃f�[�^���Ǘ�����N���X
        /// </summary>
        [Serializable]
        public class ItemData {
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
            public bool isNotBullet;//�e�̃A�C�e���ł͂Ȃ����ǂ���
            public bool isFirearms;//�e�Ί킩�ǂ���
            public bool isHandWeapon;//�ߐڕ��킩�ǂ���
            public bool isThrowingWeapon;//�������킩�ǂ���
            public bool isMissile;
            public Sprite sprite;//Sprite
            public GameObject prefab;//�v���t�@�u
            public Rigidbody bulletPrefab;//�e�̃v���t�@�u
            public ItemType itemType;
            public BulletDetailBase bulletDetailPrefab;

            public int BulletCount
            {
                get => bulletCount;
                set => bulletCount = Math.Clamp(bulletCount + value, 0, maxBulletCount);
            }
        }

        public List<ItemData> itemDataList = new List<ItemData>();//�A�C�e���f�[�^�̃��X�g
    }
}