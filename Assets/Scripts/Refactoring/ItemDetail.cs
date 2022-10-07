using UnityEngine;

namespace yamap 
{
    public class ItemDetail : MonoBehaviour 
    {
        ItemDataSO.ItemData itemData;//�A�C�e���̃f�[�^

        /// <summary>
        /// ItemDetail�̏����ݒ���s��
        /// </summary>
        /// <param name="itemData">�A�C�e���̃f�[�^</param>
        public void SetUpItemDetail(ItemDataSO.ItemData itemData) 
        {
            //�A�C�e���̃f�[�^���擾
            this.itemData = itemData;
        }

        /// <summary>
        /// �A�C�e���̖��O���擾����
        /// </summary>
        /// <returns>�A�C�e���̖��O</returns>
        public ItemDataSO.ItemName GetItemName() 
        {
            //�A�C�e���̃f�[�^����ɁA�A�C�e���̖��O��Ԃ�
            return itemData.itemName;
        }

        /// <summary>
        /// �U���͂��擾����
        /// </summary>
        /// <returns>�U����</returns>
        public float GetAttackPower() 
        {
            //�A�C�e���̃f�[�^����ɁA�A�C�e���̍U���͂�Ԃ�
            return itemData.attackPower;
        }
    }
}