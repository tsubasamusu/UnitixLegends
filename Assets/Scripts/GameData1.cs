using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using DG.Tweening;//DOTween���g�p

namespace yamap {
    public class GameData : MonoBehaviour {

        public static GameData instance;//�C���X�^���X

        [SerializeField]
        private float fallSpeed;//�������x

        public float FallSpeed//fallSpeed�ϐ��p�̃v���p�e�B
        {
            get { return fallSpeed; }//�O������͎擾�����݂̂��\��
        }

        private int killCount;//Player���|�����G�̐�

        public int KillCount//killCount�ϐ��p�̃v���p�e�B
        {
            get { return killCount; }//�擾����
            set { killCount = value; }//�ݒ菈��
        }

        /// <summary>
        /// Start���\�b�h���O�ɌĂяo�����i�ȉ��A�V���O���g���ɕK�{�̋L�q�j
        /// </summary>
        private void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }
    }
}