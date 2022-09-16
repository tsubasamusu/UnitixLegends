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
        /// ゲーム開始直後に呼び出される
        /// </summary>
        public void SetUpHealth(UIManager uiManager) {
            this.uiManager = uiManager;

            //ストームによるダメージを受けるかどうかの調査を開始
            //StartCoroutine(CheckStormDamage());
        }

        /// <summary>
        /// 他のコライダーに触れた際に呼び出される
        /// </summary>
        /// <param name="hit">触れた相手</param>
        private void OnCollisionEnter(Collision hit) {
            if (hit.gameObject.TryGetComponent(out ItemDetail itemDetail)) {
                UpdatePlayerHp(itemDetail.GetAttackPower(), hit.gameObject);
            }
        }

        ///// <summary>
        ///// ストームによるダメージを受けるかどうか調べる  =>  GameManager で監視
        ///// </summary>
        ///// <returns>待ち時間</returns>
        //private IEnumerator CheckStormDamage() {
        //    //空の判定
        //    bool skyFlag = false;

        //    //ゲーム終了状態ではないなら、繰り返される
        //    while (PlayerHp > 0) {
        //        //Playerが安置内におらず
        //        while (!stormController.CheckEnshrine(transform.position)) {
        //            //空の判定がtrueなら
        //            if (skyFlag) {

        //                //悪天候に変更
        //                stormController.ChangeSkyBox(PlayerStormState.InStorm);

        //                //空の判定にfalseを入れる
        //                skyFlag = false;
        //            }

        //            //PlayerのHpを減少させる
        //            UpdatePlayerHp(-stormController.StormDamage);

        //            //1秒待つ
        //            yield return new WaitForSeconds(1f);
        //        }

        //        //空の判定がfalseなら
        //        if (!skyFlag) {
        //            //快晴に変更
        //            stormController.ChangeSkyBox(PlayerStormState.OutStorm);

        //            //空の判定にtrueを入れる
        //            skyFlag = true;
        //        }

        //        //次のフレームへ飛ばす（実質、Updateメソッド）
        //        yield return null;
        //    }
        //}

        /// <summary>
        /// PlayerのHpを更新
        /// </summary>
        /// <param name="updateValue">Hpの更新量</param>
        /// <param name="gameObject">触れた相手</param>
        public void UpdatePlayerHp(float updateValue, GameObject gameObject = null) {
            //Debug.Log(updateValue);

            //攻撃を受けた際の処理なら
            if (updateValue < 0.0f) {
                //被弾した際の視界の処理を行う
                StartCoroutine(uiManager.AttackEventHorizon());
            }

            //体力用スライダーを更新
            uiManager.UpdateHpSliderValue(playerHp, updateValue);

            //playerHpに0以上100以下の値まで代入されるように制限する
            playerHp = Mathf.Clamp(playerHp + updateValue, 0, 100);

            //nullエラー回避
            if (gameObject != null) {
                //引数で受け取ったゲームオブジェクトを消す
                Destroy(gameObject);
            }

            ////Playerの体力が0になったら
            //if (playerHp == 0.0f) {
            //    //ゲームオーバー演出を行う
            //    StartCoroutine(gameManager.MakeGameOver());
            //}
        }

        /// <summary>
        /// 催涙弾による攻撃を受けた場合の処理
        /// </summary>
        private void AttackedByTearGasGrenade() {
            //視界を5.0秒間暗くする
            StartCoroutine(uiManager.SetEventHorizonBlack(5.0f));
        }

        /// <summary>
        /// 回復アイテムの所持数を更新する
        /// </summary>
        /// <param name="itemName">アイテムの名前</param>
        /// <param name="updateValue">所持数の更新量</param>
        public void UpdateRecoveryItemCount(ItemDataSO.ItemName itemName, int updateValue) {

            // ItemName から更新する回復アイテムの個数と最大値を取得
            ref int recoveryItemCount = ref GetRecoveryItemCountRef(itemName);
            int maxCount = GetRecoveryItemMaxCount(itemName); 

            // 更新
            recoveryItemCount = Mathf.Clamp(recoveryItemCount + updateValue, 0, maxCount);

            ////アイテムの名前に応じて処理を変更
            //switch (itemName) {
            //    //包帯なら
            //    case ItemDataSO.ItemName.Bandage:
            //        bandageCount = Mathf.Clamp(bandageCount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.Bandage).maxBulletCount);
            //        break;

            //    //薬草なら
            //    case ItemDataSO.ItemName.MedicinalPlants:
            //        medicinalPlantscount = Mathf.Clamp(medicinalPlantscount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.MedicinalPlants).maxBulletCount);
            //        break;

            //    //注射器なら
            //    case ItemDataSO.ItemName.Syringe:
            //        syringeCount = Mathf.Clamp(syringeCount + updateValue, 0, ItemManager.instance.GetItemData(ItemDataSO.ItemName.Syringe).maxBulletCount);
            //        break;
            //}
        }

        int x = 0;

        /// <summary>
        /// 指定した回復アイテムの所持数を参照戻しで取得する
        /// </summary>
        /// <param name="itemName">回復アイテムの名前</param>
        /// <returns>その回復アイテムの所持数</returns>
        private ref int GetRecoveryItemCountRef(ItemDataSO.ItemName itemName) {
            Debug.Log(itemName);
            //アイテムの名前に応じて処理を変更
            switch(itemName) {
                case ItemDataSO.ItemName.Bandage: return ref bandageCount;
                case ItemDataSO.ItemName.MedicinalPlants: return ref medicinalPlantscount;
                case ItemDataSO.ItemName.Syringe: return ref syringeCount;
                default: return ref x;
            }
        }

        /// <summary>
        /// 指定したアイテムの最大数の取得
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        private int GetRecoveryItemMaxCount(ItemDataSO.ItemName itemName) {
            return ItemManager.instance.GetItemData(itemName).maxBulletCount;
        }

        /// <summary>
        /// 指定したアイテムの最大数の取得
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