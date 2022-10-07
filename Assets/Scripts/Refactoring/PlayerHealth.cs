using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap 
{
    public class PlayerHealth : MonoBehaviour 
    {
        private UIManager uiManager;//UIManager

        private float playerHp = 100.0f;//Player�̗̑�

        public float PlayerHp//PlayerHp�ϐ��p�̃v���p�e�B
        {
            get { return playerHp; }//�O������͎擾�����̂݉\��
        }

        private int bandageCount;//��т̐�

        public int BandageCount//bandageCount�ϐ��p�̃v���p�e�B
        {
            get { return bandageCount; }//�O������͎擾�����̂݉\��
        }

        private int medicinalPlantscount;//�򑐂̐�

        public int MedicinalPlantsCount//medicinalPlantscount�ϐ��p�̃v���p�e�B
        {
            get { return medicinalPlantscount; }//�O������͎擾�����̂݉\��
        }

        private int syringeCount;//���ˊ�̐�

        public int SyringeCount//syringeCount�ϐ��p�̃v���p�e�B
        {
            get { return syringeCount; }//�O������͎擾�����̂݉\��
        }

        /// <summary>
        /// PlayerHealth�̏����ݒ���s��
        /// </summary>
        /// <param name="uiManager">UIManager</param>
        public void SetUpHealth(UIManager uiManager) 
        {
            //UIManager���擾
            this.uiManager = uiManager;
        }

        /// <summary>
        /// ���̃R���C�_�[�ɐG�ꂽ�ۂɌĂяo�����
        /// </summary>
        /// <param name="hit">�ڐG����</param>
        private void OnCollisionEnter(Collision hit) 
        {
            //�ڐG���肪�A�C�e���Ȃ�
            if (hit.gameObject.TryGetComponent(out ItemDetail itemDetail)) 
            {
                //Player��Hp���X�V����
                UpdatePlayerHp(itemDetail.GetAttackPower(), hit.gameObject);
            }
        }

        /// <summary>
        /// Player��Hp���X�V����
        /// </summary>
        /// <param name="updateValue">Hp�̍X�V��</param>
        /// <param name="gameObject">�G�ꂽ����</param>
        public void UpdatePlayerHp(float updateValue, GameObject gameObject = null) 
        {
            //�U�����󂯂��ۂ̏����Ȃ�
            if (updateValue < 0.0f) 
            {
                //��e�����ۂ̎��E�̏������s��
                StartCoroutine(uiManager.AttackEventHorizon());
            }

            //�̗͗p�X���C�_�[���X�V
            uiManager.UpdateHpSliderValue(playerHp, updateValue);

            //playerHp��0�ȏ�100�ȉ��̒l�܂ő�������悤�ɐ�������
            playerHp = Mathf.Clamp(playerHp + updateValue, 0, 100);

            //null�G���[���
            if (gameObject != null) 
            {
                //�����Ŏ󂯎�����Q�[���I�u�W�F�N�g������
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// �×ܒe�ɂ��U�����󂯂��ꍇ�̏���
        /// </summary>
        private void AttackedByTearGasGrenade() 
        {
            //���E��5.0�b�ԈÂ�����
            StartCoroutine(uiManager.SetEventHorizonBlack(5.0f));
        }

        /// <summary>
        /// �񕜃A�C�e���̏��������X�V����
        /// </summary>
        /// <param name="itemName">�A�C�e���̖��O</param>
        /// <param name="updateValue">�������̍X�V��</param>
        public void UpdateRecoveryItemCount(ItemDataSO.ItemName itemName, int updateValue) 
        {
            //ItemName����X�V����񕜃A�C�e���̌��ƍő�l���擾
            //TODO:�Ȃ��ref��t����H
            ref int recoveryItemCount = ref GetRecoveryItemCountRef(itemName);

            //�񕜃A�C�e���̍ő及�������擾
            int maxCount = GetRecoveryItemMaxCount(itemName); 

            //�񕜃A�C�e���̏��������X�V
            recoveryItemCount = Mathf.Clamp(recoveryItemCount + updateValue, 0, maxCount);
        }

        //TODO:�Ȃ�̂��߂̕ϐ��H�Ȃ��O�H
        int x = 0;

        /// <summary>
        /// �w�肵���񕜃A�C�e���̏��������Q�Ɩ߂��Ŏ擾����
        /// </summary>
        /// <param name="itemName">�񕜃A�C�e���̖��O</param>
        /// <returns>���̉񕜃A�C�e���̏�����</returns>
        private ref int GetRecoveryItemCountRef(ItemDataSO.ItemName itemName) 
        {
            //�A�C�e���̖��O�ɉ����ď�����ύX
            switch(itemName) 
            {
                case ItemDataSO.ItemName.Bandage: return ref bandageCount;
                case ItemDataSO.ItemName.MedicinalPlants: return ref medicinalPlantscount;
                case ItemDataSO.ItemName.Syringe: return ref syringeCount;
                default: return ref x;
            }
        }

        /// <summary>
        /// �w�肵���A�C�e���̍ő吔�̎擾
        /// </summary>
        /// <param name="itemName">�A�C�e���̖��O</param>
        /// <returns>�w�肵���A�C�e���̍ő吔</returns>
        private int GetRecoveryItemMaxCount(ItemDataSO.ItemName itemName) 
        {
            //�w�肳�ꂽ�A�C�e���̍ő吔��Ԃ�
            return ItemManager.instance.GetItemData(itemName).maxBulletCount;
        }

        /// <summary>
        /// �w�肵���񕜃A�C�e���̍ő吔�̎擾
        /// </summary>
        /// <param name="itemName">�񕜃A�C�e���̖��O</param>
        /// <returns>�w�肵���񕜃A�C�e���̍ő吔</returns>
        public int GetRecoveryItemCount(ItemDataSO.ItemName itemName) 
        {
            //�󂯎�����񕜃A�C�e���̖��O�ɉ����ď�����ύX
            return itemName switch 
            {
                ItemDataSO.ItemName.Bandage => BandageCount,
                ItemDataSO.ItemName.MedicinalPlants => MedicinalPlantsCount,
                ItemDataSO.ItemName.Syringe => SyringeCount,
                _ => 0,
            };
        }
    }
}