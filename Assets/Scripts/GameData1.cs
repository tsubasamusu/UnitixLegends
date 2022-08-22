using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using DG.Tweening;//DOTween���g�p

namespace yamap {
    public class GameData : MonoBehaviour {

        public static GameData instance;//�C���X�^���X

        public List<ItemDataSO.ItemData> playerItemList = new List<ItemDataSO.ItemData>();//Player���������Ă���A�C�e���̃��X�g

        private bool isFull;//Player�̏��L�������e�I�[�o�[���ǂ���

        public bool IsFull//isFull�ϐ��p�̃v���p�e�B
        {
            get => isFull;
            set => isFull = value;//�O������͎擾�����݂̂��\��
        }

        private int killCount;//Player���|�����G�̐�

        public int KillCount//killCount�ϐ��p�̃v���p�e�B
        {
            get { return killCount; }//�擾����
            set { killCount = value; }//�ݒ菈��
        }

        private int selectedItemNo = 1;//�g�p���Ă���A�C�e���̔ԍ�

        public int SelectedItemNo//useItemNo�ϐ��p�̃v���p�e�B
        {
            get { return selectedItemNo; }//�O������͎擾�����̂݉\��
            set { selectedItemNo = value; }
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

        /// <summary>
        /// PlayerItemList�̎w�肵���ԍ��̗v�f���󂢂Ă��邩�ǂ����𒲂ׂ�
        /// </summary>
        /// <param name="elementNo">�v�f�̔ԍ�</param>
        /// <returns> PlayerItemList�̎w�肵���ԍ��̗v�f���󂢂Ă�����true��Ԃ�</returns>
        public bool CheckTheElement(int elementNo) {
            //PlayerItemList�̎w�肵���ԍ��̗v�f���󂢂Ă�����true��Ԃ�
            return playerItemList[elementNo].itemName == ItemDataSO.ItemName.None ? true : false;
        }

        /// <summary>
        /// ���e�I�[�o�[���ǂ������ׂ�
        /// </summary>
        public bool CheckIsFull() {
            //���ɋ��e�I�[�o�[�̏�ԂƂ��ēo�^����
            //isFull = true;

            //Player���������Ă���A�C�e���̃��X�g�̗v�f�̐������J��Ԃ�
            for (int i = 0; i < playerItemList.Count; i++) {
                //i�Ԗڂ̗v�f����ł͂Ȃ��Ȃ�
                if (!CheckTheElement(i)) {
                    //���e�I�[�o�[
                    //isFull = false;
                    return true;
                }
            }
            // �`�F�b�N���ʂ�����A�󂪂���
            return false;
        }
    }
}