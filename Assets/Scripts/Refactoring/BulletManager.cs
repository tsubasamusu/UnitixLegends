using System.Collections;//IEnumeratorを使用
using UnityEngine;

namespace yamap 
{
    public class BulletManager : MonoBehaviour 
    {
        private Transform mainCamera;//メインカメラ

        private Transform temporaryObjectContainerTran;//一時的にゲームオブジェクトを収容するTransform

        private PlayerController playerController;//PlayerController

        private float timer;//経過時間

        private bool stopFlag;//重複処理を防ぐ

        private int grenadeBulletCount;//手榴弾の残りの数

        public int GrenadeBulletCount//grenadeBulletCount変数用のプロパティ
        {
            get { return grenadeBulletCount; }//外部からは取得処理のみを可能に
        }

        private int tearGasGrenadeBulletCount;//催涙弾の残りの数

        public int TearGasGrenadeBulletCount//tearGasGrenadeBulletCount変数用のプロパティ
        {
            get { return tearGasGrenadeBulletCount; }//外部からは取得処理のみを可能に
        }

        private int assaultBulletCount;//アサルト用弾の残弾数

        public int AssaultBulletCount//assaultBulletCount変数用のプロパティ
        {
            get { return assaultBulletCount; }//外部からは取得処理のみを可能に
        }

        private int sniperBulletCount;//スナイパー用弾の残弾数

        public int SniperBulletCount//sniperBulletCount変数用のプロパティ
        {
            get { return sniperBulletCount; }//外部からは取得処理のみを可能に
        }

        private int shotgunBulletCount;//ショットガン用弾の残弾数

        public int ShotgunBulletCount//shotgunBulletCount変数用のプロパティ
        {
            get { return shotgunBulletCount; }//外部からは取得処理のみを可能に
        }

        /// <summary>
        /// BulletManagerの初期設定を行う
        /// </summary>
        /// <param name="playerController">PlayerController</param>
        /// <param name="temporaryObjectContainerTran">一時的にゲームオブジェクトを収容するTransform</param>
        public void SetUpBulletManager(PlayerController playerController, Transform temporaryObjectContainerTran) 
        {
            //メインカメラの位置情報を取得
            mainCamera = Camera.main.transform;

            //PlayerControllerを取得
            this.playerController = playerController;

            //一時的にゲームオブジェクトを収容するTransformを取得
            this.temporaryObjectContainerTran = temporaryObjectContainerTran;

            //経過時間の計測を開始
            StartCoroutine(MeasureTime());
        }

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        private void Update() 
        {
            //Bulletを発射する向きをカメラの向きに合わせる
            transform.eulerAngles = new Vector3(mainCamera.eulerAngles.x, mainCamera.eulerAngles.y, transform.eulerAngles.z);
        }

        /// <summary>
        /// 経過時間1を計測する
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator MeasureTime() 
        {
            //無限ループ
            while (true) 
            {
                //経過時間を計測
                timer += Time.deltaTime;

                //次のフレームへ飛ばす（実質、Updateメソッド）
                yield return null;
            }
        }

        /// <summary>
        /// 残弾数を更新する
        /// </summary>
        /// <param name="itemName">アイテムの名前</param>
        /// <param name="updateValue">残弾数の変更量</param>
        public void UpdateBulletCount(ItemDataSO.ItemName itemName, int updateValue) 
        {
            //ItemNameから更新する弾の個数を取得
            ref int useBulletCount = ref GetUseBulletCount(itemName);//TODO:なぜ「ref」を付けているのか？

            //ItemNameから弾の最大数を取得
            int maxBulletCount = GetUseMaxBulletCount(itemName);

            //弾数を更新する
            useBulletCount = Mathf.Clamp(useBulletCount + updateValue, 0, maxBulletCount);
        }

        //TODO:なぜ、わざわざここでこんな変数を宣言しているのか？
        int x = 0;

        /// <summary>
        /// 弾の数を取得して参照戻しする
        /// </summary>
        /// <param name="itemName">アイテム名</param>
        /// <returns>弾の数</returns>
        private ref int GetUseBulletCount(ItemDataSO.ItemName itemName) 
        {
            //受け取ったアイテムの名前に応じて処理を変更
            switch (itemName) {
                case ItemDataSO.ItemName.Grenade: return ref grenadeBulletCount;
                case ItemDataSO.ItemName.TearGasGrenade: return ref tearGasGrenadeBulletCount;
                case ItemDataSO.ItemName.Assault: return ref assaultBulletCount;
                case ItemDataSO.ItemName.Shotgun: return ref shotgunBulletCount;
                case ItemDataSO.ItemName.Sniper: return ref sniperBulletCount;
                default: return ref x;
            }
        }

        /// <summary>
        /// 弾の最大数を取得する
        /// </summary>
        /// <param name="itemName">アイテム名</param>
        /// <returns>弾の最大数</returns>
        private int GetUseMaxBulletCount(ItemDataSO.ItemName itemName) 
        {
            //アイテム名を元に、弾の最大数を返す
            return ItemManager.instance.GetItemData(itemName).maxBulletCount;
        }

        /// <summary>
        /// 弾を発射する
        /// </summary>
        /// <param name="itemData">現在選択しているアイテムの情報</param>
        public void ShotBullet(ItemDataSO.ItemData itemData)
        {
            //経過時間が連射間隔より小さいか、残弾数が0なら
            if (timer < itemData.interval || GetBulletCount(itemData.itemName) == 0) 
            {
                //以降の処理を行わない
                return;
            }

            //弾を生成
            BulletDetailBase bullet = Instantiate(itemData.weaponPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0)).GetComponent<BulletDetailBase>();

            //弾を発射
            bullet.SetUpBulletDetail(itemData.attackPower, BulletOwnerType.Player, transform.forward * itemData.shotSpeed, itemData.seName, itemData.interval, itemData.effectPrefab);

            //Playerの死後も発射した弾は一定時間残る
            bullet.gameObject.transform.SetParent(temporaryObjectContainerTran);

            //残弾数を減らす
            UpdateBulletCount(itemData.itemName, -1);

            //経過時間を初期化
            timer = 0;

            // 手りゅう弾か、催涙弾で
            if (itemData.itemName == ItemDataSO.ItemName.Grenade || itemData.itemName == ItemDataSO.ItemName.TearGasGrenade) 
            {
                // 弾数が 0 になったら
                if (GetBulletCount(itemData.itemName) <= 0) 
                {
                    // アイテムを破棄
                    ItemManager.instance.DiscardItem(ItemManager.instance.SelectedItemNo);
                }
            }
        }

        /// <summary>
        /// 指定したアイテムの残弾数を取得する
        /// </summary>
        /// <param name="itemName">その弾を使用するアイテム</param>
        /// <returns>そのアイテムが使用する弾の残弾数</returns>
        public int GetBulletCount(ItemDataSO.ItemName itemName) 
        {
            //選択しているアイテムの名前に応じて処理を変更
            switch (itemName) {
                //手榴弾なら
                case ItemDataSO.ItemName.Grenade:
                    return grenadeBulletCount;

                //催涙弾なら
                case ItemDataSO.ItemName.TearGasGrenade:
                    return tearGasGrenadeBulletCount;

                //アサルトなら
                case ItemDataSO.ItemName.Assault:
                    return assaultBulletCount;

                //スナイパーなら
                case ItemDataSO.ItemName.Sniper:
                    return sniperBulletCount;

                //ショットガンなら
                case ItemDataSO.ItemName.Shotgun:
                    return shotgunBulletCount;

                //上記以外なら
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 外部から近接武器を使用する際に呼び出される処理
        /// </summary>
        /// <param name="itemData">アイテムのデータ</param>
        public void PrepareUseHandWeapon(ItemDataSO.ItemData itemData) 
        {
            //近接武器の使用を開始する
            StartCoroutine(UseHandWeapon(itemData));
        }

        /// <summary>
        /// 近接武器を使用する
        /// </summary>
        /// <param name="itemData">使用するアイテムのデータ</param>
        /// <returns>待ち時間</returns>
        public IEnumerator UseHandWeapon(ItemDataSO.ItemData itemData) 
        {
            //stopFlagがtrueなら
            if (stopFlag) 
            {
                //以降の処理を行わない
                yield break;
            }

            //重複処理を防ぐ
            stopFlag = true;

            //アイテムを生成
            HandWeaponDetailBase handWeapon = Instantiate(itemData.weaponPrefab, transform).GetComponent<HandWeaponDetailBase>();

            // アイテムの設定と攻撃
            handWeapon.SetUpWeaponDetail(itemData.attackPower, BulletOwnerType.Player, itemData.seName, itemData.interval);
            
            //アイテムの位置を設定(アイテム側のクラスで行ってもよい)
            handWeapon.gameObject.transform.localPosition = Vector3.zero;

            //アイテムのアニメーションが終わるまで待つ
            yield return new WaitForSeconds(itemData.interval);

            //使用許可を出す
            stopFlag = false;
        }
    }
}