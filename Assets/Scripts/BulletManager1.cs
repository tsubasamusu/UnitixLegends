using DG.Tweening;
using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;
using UnityEngine;

namespace yamap {

    public class BulletManager : MonoBehaviour {
        [SerializeField]
        private int maxBulletCount;//所持できるアイテム1つ当たりの弾の最大数

        [SerializeField]
        private Transform mainCamera;//メインカメラ

        [SerializeField]
        private Transform temporaryObjectContainerTran;//一時的にゲームオブジェクトを収容するTransform

        [SerializeField]
        private ItemDataSO itemDataSO;//ItemDataSO

        [SerializeField]
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
        /// ゲーム開始直後に呼び出される
        /// </summary>
        private void Start() {
            // 弾の最大数をセット


            //経過時間の計測を開始
            StartCoroutine(MeasureTime1());
        }

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        private void Update() {
            //Bulletを発射する向きをカメラの向きに合わせる
            transform.eulerAngles = new Vector3(mainCamera.eulerAngles.x, mainCamera.eulerAngles.y, transform.eulerAngles.z);
        }

        /// <summary>
        /// 経過時間1を計測する
        /// </summary>
        /// <returns>待ち時間</returns>
        private IEnumerator MeasureTime1() {
            //無限ループ
            while (true) {
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
        public void UpdateBulletCount(ItemDataSO.ItemName itemName, int updateValue) {
            //　選択している武器の情報が GameData にあるので、それを活用するようにすれば分岐なくせる

            //GameData.instance.playerItemList[playerController.SelectedItemNo].itemName

            // 弾の場合
            //if (itemData.itemType == ItemDataSO.ItemType.Bullet) {
            //    for (int i = 0; i < GameData.instance.playerItemList.Count; i++) {
            //        if (GameData.instance.playerItemList[playerController.SelectedItemNo].itemName == itemData.itemName) {
            //            プロパティに代入して、Mathf はプロパティ内で行う(最大値も利用できるため)
            //            GameData.instance.playerItemList[playerController.SelectedItemNo].BulletCount = itemData.bulletCount;
            //            return;
            //        }
            //    }
            //}

            //残弾数の更新数が0ではなかったら
            //if (updateValue != 0)
            //{
            //受け取ったアイテムの名前に応じて処理を変更
            switch (itemName) {
                //手榴弾なら
                case ItemDataSO.ItemName.Grenade:
                    grenadeBulletCount = Mathf.Clamp(grenadeBulletCount + updateValue, 0, maxBulletCount);  // アイテムごとの最大数を参照していない
                    break;

                //催涙弾なら
                case ItemDataSO.ItemName.TearGasGrenade:
                    tearGasGrenadeBulletCount = Mathf.Clamp(tearGasGrenadeBulletCount + updateValue, 0, maxBulletCount);
                    break;

                //アサルトなら
                case ItemDataSO.ItemName.Assault:
                case ItemDataSO.ItemName.AssaultBullet:
                    assaultBulletCount = Mathf.Clamp(assaultBulletCount + updateValue, 0, maxBulletCount);
                    break;

                //ショットガンなら
                case ItemDataSO.ItemName.Shotgun:
                case ItemDataSO.ItemName.ShotgunBullet:
                    shotgunBulletCount = Mathf.Clamp(shotgunBulletCount + updateValue, 0, maxBulletCount);
                    break;

                //スナイパーなら
                case ItemDataSO.ItemName.Sniper:
                case ItemDataSO.ItemName.SniperBullet:
                    sniperBulletCount = Mathf.Clamp(sniperBulletCount + updateValue, 0, maxBulletCount);
                    break;
            }

            //以降の処理を行わない
            //return;
            //}

            //受け取ったアイテムの名前に応じて処理を変更
            //switch (itemName)
            //{
            //    //手榴弾なら
            //    case ItemDataSO.ItemName.Grenade:
            //        grenadeBulletCount += itemDataSO.itemDataList[1].bulletCount;
            //        break;

            //    //催涙弾なら
            //    case ItemDataSO.ItemName.TearGasGrenade:
            //        tearGasGrenadeBulletCount += itemDataSO.itemDataList[2].bulletCount;
            //        break;

            //    //アサルトなら
            //    case ItemDataSO.ItemName.Assault:
            //        assaultBulletCount += itemDataSO.itemDataList[5].bulletCount;
            //        break;

            //    //ショットガンなら
            //    case ItemDataSO.ItemName.Shotgun:
            //        shotgunBulletCount += itemDataSO.itemDataList[6].bulletCount;
            //        break;

            //    //スナイパーなら
            //    case ItemDataSO.ItemName.Sniper:
            //        sniperBulletCount += itemDataSO.itemDataList[7].bulletCount;
            //        break;

            //    //アサルト用弾なら
            //    case ItemDataSO.ItemName.AssaultBullet:
            //        assaultBulletCount += itemDataSO.itemDataList[11].bulletCount;
            //        break;

            //    //ショットガン用弾なら
            //    case ItemDataSO.ItemName.ShotgunBullet:
            //        shotgunBulletCount += itemDataSO.itemDataList[12].bulletCount;
            //        break;

            //    //スナイパー用弾なら
            //    case ItemDataSO.ItemName.SniperBullet:
            //        sniperBulletCount += itemDataSO.itemDataList[13].bulletCount;
            //        break;
            //}
        }

        /// <summary>
        /// 弾を発射する
        /// </summary>
        /// <param name="itemData">使用するアイテムのデータ</param>
        /// <returns>待ち時間</returns>
        public void ShotBullet(ItemDataSO.ItemData itemData)   // 現在選択しているアイテムの情報
        {
            //経過時間が連射間隔より小さいか、残弾数が0なら
            if (timer < itemData.interval || GetBulletCount(itemData.itemName) == 0) {
                //以降の処理を行わない
                //yield break;
                return;
            }

            //弾を生成
            //Rigidbody bulletRb = Instantiate(itemData.bulletPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

            BulletDetailBase bullet = Instantiate(itemData.bulletDetailPrefab, transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

            bullet.SetUpBulletDetail(itemData.attackPower, BulletOwnerType.Player, transform.forward * itemData.shotSpeed);

            bullet.gameObject.transform.SetParent(temporaryObjectContainerTran);

            ////弾を発射
            //bulletRb.AddForce(transform.forward * itemData.shotSpeed);

            //Playerの死後も発射した弾は一定時間残る
            //bulletRb.gameObject.transform.SetParent(temporaryObjectContainerTran);

            //残弾数を減らす
            UpdateBulletCount(itemData.itemName, -1);

            //経過時間を初期化
            timer = 0;

            // 以下は Bullet 側でやる

            ////使用するアイテムが、手榴弾でも催涙弾でもないなら
            //if (itemData.itemName != ItemDataSO.ItemName.Grenade && itemData.itemName != ItemDataSO.ItemName.TearGasGrenade) {
            //    //発射した弾を3.0秒後に消す
            //    Destroy(bullet.gameObject, 3.0f);

            //    //以降の処理を行わない
            //    yield break;
            //}

            ////手榴弾か催涙弾の残りの数が0になったら
            //if (grenadeBulletCount == 0 || tearGasGrenadeBulletCount == 0) {
            //    //アイテムを破棄する
            //    GameData.instance.DiscardItem(playerController.SelectedItemNo - 1);
            //}

            ////爆破時間まで待つ
            //yield return new WaitForSeconds(itemData.timeToExplode);

            ////使用するアイテムが手榴弾なら
            //if (itemData.itemName == ItemDataSO.ItemName.Grenade) {
            //    //TODO:爆発する処理
            //}
            ////使用するアイテムが催涙弾なら
            //else if (itemData.itemName == ItemDataSO.ItemName.TearGasGrenade) {
            //    //TODO:ガスを放出する処理
            //}

            ////発射した弾を消す
            //Destroy(bullet.gameObject);
        }

        /// <summary>
        /// 指定したアイテムの残弾数を取得する
        /// </summary>
        /// <param name="itemName">その弾を使用するアイテム</param>
        /// <returns>そのアイテムが使用する弾の残弾数</returns>
        public int GetBulletCount(ItemDataSO.ItemName itemName) {
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


        public void PrepareUseHandWeapon(ItemDataSO.ItemData itemData) {
            StartCoroutine(UseHandWeapon(itemData));
        }


        /// <summary>
        /// 近接武器を使用する
        /// </summary>
        /// <param name="itemData">使用するアイテムのデータ</param>
        /// <returns>待ち時間</returns>
        public IEnumerator UseHandWeapon(ItemDataSO.ItemData itemData) {
            //stopFlagがtrueなら
            if (stopFlag) {
                //以降の処理を行わない
                yield break;
            }

            // 近接武器にもクラスをつけて、振る舞いを変える


            //アイテムを生成
            Rigidbody itemRb = Instantiate(itemData.bulletPrefab, transform);

            //アイテムの位置を設定
            itemRb.gameObject.transform.localPosition = Vector3.zero;

            //重複処理を防ぐ
            stopFlag = true;

            //生成したアイテムからBoxColliderの情報を取得できなかったら
            if (!itemRb.TryGetComponent(out BoxCollider boxCollider))//nullエラー回避
            {
                //問題を報告
                Debug.Log("BoxColliderの取得に失敗");

                //以降の処理を行わない
                yield break;
            }

            //使用するアイテムがナイフなら
            if (itemData.itemName == ItemDataSO.ItemName.Knife) {
                //ナイフのアニメーションを開始（前後移動）
                itemRb.gameObject.transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo);

                //ナイフのアニメーションが終わるまで待つ
                yield return new WaitForSeconds(1f);

                //生成したアイテムを消す
                Destroy(itemRb.gameObject);

                //重複処理を防ぐ
                stopFlag = false;
            }
            //使用するアイテムがバットなら
            else if (itemData.itemName == ItemDataSO.ItemName.Bat) {
                //バットのアニメーションを開始（前後移動）
                itemRb.gameObject.transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo);

                itemRb.gameObject.transform.DOLocalRotate(new Vector3(60f, 0f, 0f), 0.5f).SetLoops(2, LoopType.Yoyo);

                //バットのアニメーションが終わるまで待つ
                yield return new WaitForSeconds(1f);

                //生成したアイテムを消す
                Destroy(itemRb.gameObject);

                //重複処理を防ぐ
                stopFlag = false;
            }
        }
    }
}