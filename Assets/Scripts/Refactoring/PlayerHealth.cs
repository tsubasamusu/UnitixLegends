using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap {

    public class PlayerHealth : MonoBehaviour {

        //[SerializeField]
        private UIManager uiManager;//UIManager

        //[SerializeField]
        //private StormController stormController;//StormController

        //[SerializeField]
        //private GameManager gameManager;//GameManager

        //[SerializeField]
        //private ItemManager itemManager;//ItemManager

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
        /// �Q�[���J�n����ɌĂяo�����
        /// </summary>
        public void SetUpHealth(UIManager uiManager) {
            this.uiManager = uiManager;

            //�X�g�[���ɂ��_���[�W���󂯂邩�ǂ����̒������J�n
            //StartCoroutine(CheckStormDamage());
        }

        /// <summary>
        /// ���̃R���C�_�[�ɐG�ꂽ�ۂɌĂяo�����
        /// </summary>
        /// <param name="hit">�G�ꂽ����</param>
        private void OnCollisionEnter(Collision hit) {
            if (hit.gameObject.TryGetComponent(out ItemDetail itemDetail)) {
                UpdatePlayerHp(itemDetail.GetAttackPower(), hit.gameObject);
            }
        }

        ///// <summary>
        ///// �X�g�[���ɂ��_���[�W���󂯂邩�ǂ������ׂ�  =>  GameManager �ŊĎ�
        ///// </summary>
        ///// <returns>�҂�����</returns>
        //private IEnumerator CheckStormDamage() {
        //    //��̔���
        //    bool skyFlag = false;

        //    //�Q�[���I����Ԃł͂Ȃ��Ȃ�A�J��Ԃ����
        //    while (PlayerHp > 0) {
        //        //Player�����u���ɂ��炸
        //        while (!stormController.CheckEnshrine(transform.position)) {
        //            //��̔��肪true�Ȃ�
        //            if (skyFlag) {

        //                //���V��ɕύX
        //                stormController.ChangeSkyBox(PlayerStormState.InStorm);

        //                //��̔����false������
        //                skyFlag = false;
        //            }

        //            //Player��Hp������������
        //            UpdatePlayerHp(-stormController.StormDamage);

        //            //1�b�҂�
        //            yield return new WaitForSeconds(1f);
        //        }

        //        //��̔��肪false�Ȃ�
        //        if (!skyFlag) {
        //            //�����ɕύX
        //            stormController.ChangeSkyBox(PlayerStormState.OutStorm);

        //            //��̔����true������
        //            skyFlag = true;
        //        }

        //        //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
        //        yield return null;
        //    }
        //}

        /// <summary>
        /// Player��Hp���X�V
        /// </summary>
        /// <param name="updateValue">Hp�̍X�V��</param>
        /// <param name="gameObject">�G�ꂽ����</param>
        public void UpdatePlayerHp(float updateValue, GameObject gameObject = null) {
            //Debug.Log(updateValue);

            //�U�����󂯂��ۂ̏����Ȃ�
            if (updateValue < 0.0f) {
                //��e�����ۂ̎��E�̏������s��
                StartCoroutine(uiManager.AttackEventHorizon());
            }

            //�̗͗p�X���C�_�[���X�V
            uiManager.UpdateHpSliderValue(playerHp, updateValue);

            //playerHp��0�ȏ�100�ȉ��̒l�܂ő�������悤�ɐ�������
            playerHp = Mathf.Clamp(playerHp + updateValue, 0, 100);

            //null�G���[���
            if (gameObject != null) {
                //�����Ŏ󂯎�����Q�[���I�u�W�F�N�g������
                Destroy(gameObject);
            }

            ////Player�̗̑͂�0�ɂȂ�����
            //if (playerHp == 0.0f) {
            //    //�Q�[���I�[�o�[���o���s��
            //    StartCoroutine(gameManager.MakeGameOver());
            //}
        }

        /// <summary>
        /// �×ܒe�ɂ��U�����󂯂��ꍇ�̏���
        /// </summary>
        private void AttackedByTearGasGrenade() {
            //���E��5.0�b�ԈÂ�����
            StartCoroutine(uiManager.SetEventHorizonBlack(5.0f));
        }

        /// <summary>
        /// �񕜃A�C�e���̏��������X�V����
        /// </summary>
        /// <param name="itemName">�A�C�e���̖��O</param>
        /// <param name="updateValue">�������̍X�V��</param>
        public void UpdateRecoveryItemCount(ItemDataSO.ItemName itemName, int updateValue) {

            // ItemName ����X�V����񕜃A�C�e���̌��ƍő�l���擾
            ref int recoveryItemCount = ref GetRecoveryItemCountRef(itemName);
            int maxCount = GetRecoveryItemMaxCount(itemName); 

            // �X�V
            recoveryItemCount = Mathf.Clamp(recoveryItemCount + updateValue, 0, maxCount);

            ////�A�C�e���̖��O�ɉ����ď�����ύX
            //switch (itemName) {
            //    //��тȂ�
            //    case ItemDataSO.ItemName.Bandage:
            //        bandageCount = Mathf.Clamp(bandageCount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.Bandage).maxBulletCount);
            //        break;

            //    //�򑐂Ȃ�
            //    case ItemDataSO.ItemName.MedicinalPlants:
            //        medicinalPlantscount = Mathf.Clamp(medicinalPlantscount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.MedicinalPlants).maxBulletCount);
            //        break;

            //    //���ˊ�Ȃ�
            //    case ItemDataSO.ItemName.Syringe:
            //        syringeCount = Mathf.Clamp(syringeCount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.Syringe).maxBulletCount);
            //        break;
            //}
        }

        int x = 0;

        /// <summary>
        /// �w�肵���񕜃A�C�e���̏��������Q�Ɩ߂��Ŏ擾����
        /// </summary>
        /// <param name="itemName">�񕜃A�C�e���̖��O</param>
        /// <returns>���̉񕜃A�C�e���̏�����</returns>
        private ref int GetRecoveryItemCountRef(ItemDataSO.ItemName itemName) {
            Debug.Log(itemName);
            //�A�C�e���̖��O�ɉ����ď�����ύX
            switch(itemName) {
                case ItemDataSO.ItemName.Bandage: return ref bandageCount;
                case ItemDataSO.ItemName.MedicinalPlants: return ref medicinalPlantscount;
                case ItemDataSO.ItemName.Syringe: return ref syringeCount;
                default: return ref x;
            }
        }

        /// <summary>
        /// �w�肵���A�C�e���̍ő吔�̎擾
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        private int GetRecoveryItemMaxCount(ItemDataSO.ItemName itemName) {
            return ItemManager.instance.GetItemData(itemName).maxBulletCount;
        }

        /// <summary>
        /// �w�肵���A�C�e���̍ő吔�̎擾
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public int GetRecoveryItemCount(ItemDataSO.ItemName itemName) {
            return itemName switch {
                ItemDataSO.ItemName.Bandage => BandageCount,
                ItemDataSO.ItemName.MedicinalPlants => MedicinalPlantsCount,
                ItemDataSO.ItemName.Syringe => SyringeCount,
                _ => 0,
            };
        }
    }
}