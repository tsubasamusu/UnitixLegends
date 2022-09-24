using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap 
{
    public class PlayerHealth : MonoBehaviour 
    {
        private UIManager uiManager;//UIManager

        private float playerHp = 100.0f;//Playerの体力

        public float PlayerHp//PlayerHp変数用のプロパティ
        {
            get { return playerHp; }//外部からは取得処理のみ可能に
        }

        private int bandageCount;//包帯の数

        public int BandageCount//bandageCount変数用のプロパティ
        {
            get { return bandageCount; }//外部からは取得処理のみ可能に
        }

        private int medicinalPlantscount;//薬草の数

        public int MedicinalPlantsCount//medicinalPlantscount変数用のプロパティ
        {
            get { return medicinalPlantscount; }//外部からは取得処理のみ可能に
        }

        private int syringeCount;//注射器の数

        public int SyringeCount//syringeCount変数用のプロパティ
        {
            get { return syringeCount; }//外部からは取得処理のみ可能に
        }

        /// <summary>
        /// PlayerHealthの初期設定を行う
        /// </summary>
        /// <param name="uiManager">UIManager</param>
        public void SetUpHealth(UIManager uiManager) 
        {
            //UIManagerを取得
            this.uiManager = uiManager;
        }

        /// <summary>
        /// 他のコライダーに触れた際に呼び出される
        /// </summary>
        /// <param name="hit">接触相手</param>
        private void OnCollisionEnter(Collision hit) 
        {
            //接触相手がアイテムなら
            if (hit.gameObject.TryGetComponent(out ItemDetail itemDetail)) 
            {
                //PlayerのHpを更新する
                UpdatePlayerHp(itemDetail.GetAttackPower(), hit.gameObject);
            }
        }

        /// <summary>
        /// PlayerのHpを更新する
        /// </summary>
        /// <param name="updateValue">Hpの更新量</param>
        /// <param name="gameObject">触れた相手</param>
        public void UpdatePlayerHp(float updateValue, GameObject gameObject = null) 
        {
            //攻撃を受けた際の処理なら
            if (updateValue < 0.0f) 
            {
                //被弾した際の視界の処理を行う
                StartCoroutine(uiManager.AttackEventHorizon());
            }

            //体力用スライダーを更新
            uiManager.UpdateHpSliderValue(playerHp, updateValue);

            //playerHpに0以上100以下の値まで代入されるように制限する
            playerHp = Mathf.Clamp(playerHp + updateValue, 0, 100);

            //nullエラー回避
            if (gameObject != null) 
            {
                //引数で受け取ったゲームオブジェクトを消す
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 催涙弾による攻撃を受けた場合の処理
        /// </summary>
        private void AttackedByTearGasGrenade() 
        {
            //視界を5.0秒間暗くする
            StartCoroutine(uiManager.SetEventHorizonBlack(5.0f));
        }

        /// <summary>
        /// 回復アイテムの所持数を更新する
        /// </summary>
        /// <param name="itemName">アイテムの名前</param>
        /// <param name="updateValue">所持数の更新量</param>
        public void UpdateRecoveryItemCount(ItemDataSO.ItemName itemName, int updateValue) 
        {
            //ItemNameから更新する回復アイテムの個数と最大値を取得
            //TODO:なんでrefを付ける？
            ref int recoveryItemCount = ref GetRecoveryItemCountRef(itemName);

            //回復アイテムの最大所持数を取得
            int maxCount = GetRecoveryItemMaxCount(itemName); 

            //回復アイテムの所持数を更新
            recoveryItemCount = Mathf.Clamp(recoveryItemCount + updateValue, 0, maxCount);
        }

        //TODO:なんのための変数？なぜ外？
        int x = 0;

        /// <summary>
        /// 指定した回復アイテムの所持数を参照戻しで取得する
        /// </summary>
        /// <param name="itemName">回復アイテムの名前</param>
        /// <returns>その回復アイテムの所持数</returns>
        private ref int GetRecoveryItemCountRef(ItemDataSO.ItemName itemName) 
        {
            //アイテムの名前に応じて処理を変更
            switch(itemName) 
            {
                case ItemDataSO.ItemName.Bandage: return ref bandageCount;
                case ItemDataSO.ItemName.MedicinalPlants: return ref medicinalPlantscount;
                case ItemDataSO.ItemName.Syringe: return ref syringeCount;
                default: return ref x;
            }
        }

        /// <summary>
        /// 指定したアイテムの最大数の取得
        /// </summary>
        /// <param name="itemName">アイテムの名前</param>
        /// <returns>指定したアイテムの最大数</returns>
        private int GetRecoveryItemMaxCount(ItemDataSO.ItemName itemName) 
        {
            //指定されたアイテムの最大数を返す
            return ItemManager.instance.GetItemData(itemName).maxBulletCount;
        }

        /// <summary>
        /// 指定した回復アイテムの最大数の取得
        /// </summary>
        /// <param name="itemName">回復アイテムの名前</param>
        /// <returns>指定した回復アイテムの最大数</returns>
        public int GetRecoveryItemCount(ItemDataSO.ItemName itemName) 
        {
            //受け取った回復アイテムの名前に応じて処理を変更
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