using UnityEngine;

namespace yamap 
{
    public class ItemDetail : MonoBehaviour 
    {
        ItemDataSO.ItemData itemData;//アイテムのデータ

        /// <summary>
        /// ItemDetailの初期設定を行う
        /// </summary>
        /// <param name="itemData">アイテムのデータ</param>
        public void SetUpItemDetail(ItemDataSO.ItemData itemData) 
        {
            //アイテムのデータを取得
            this.itemData = itemData;
        }

        /// <summary>
        /// アイテムの名前を取得する
        /// </summary>
        /// <returns>アイテムの名前</returns>
        public ItemDataSO.ItemName GetItemName() 
        {
            //アイテムのデータを基に、アイテムの名前を返す
            return itemData.itemName;
        }

        /// <summary>
        /// 攻撃力を取得する
        /// </summary>
        /// <returns>攻撃力</returns>
        public float GetAttackPower() 
        {
            //アイテムのデータを基に、アイテムの攻撃力を返す
            return itemData.attackPower;
        }
    }
}