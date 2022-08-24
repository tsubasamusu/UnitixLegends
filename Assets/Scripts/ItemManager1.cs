using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap {

    public class ItemManager : MonoBehaviour {

        public static ItemManager instance;

        [SerializeField]
        private ItemDataSO itemDataSO;//ItemDataSO

        [SerializeField]
        private UIManager uIManager;//UIManager

        [SerializeField]
        private BulletManager bulletManager;//BulletManager

        [SerializeField]
        private PlayerController playerController;//PlayerController

        [SerializeField]
        private PlayerHealth playerHealth;//PlayerHealth

        [SerializeField]
        private Transform itemTrans;//アイテムの位置情報をまとめたフォルダー

        [SerializeField]
        private Transform generateItemPosPrefab;//アイテムの生成位置のプレファブ

        [SerializeField]
        private Transform playerTran;//Playerの位置情報

        [SerializeField]
        private float itemRotSpeed;//アイテムの回転速度

        [SerializeField]
        private int maxItemTranCount;//生成するアイテムの生成位置の最大数

        [HideInInspector]
        public List<ItemDataSO.ItemData> generatedItemDataList = new List<ItemDataSO.ItemData>();//生成したアイテムのデータのリスト

        [HideInInspector]
        public List<Transform> generatedItemTranList = new List<Transform>();//アイテムの生成位置のリスト

        [HideInInspector]
        public List<ItemDataSO.ItemData> playerItemList = new List<ItemDataSO.ItemData>();//Playerが所持しているアイテムのリスト

        private bool isFull;//Playerの所有物が許容オーバーかどうか

        public bool IsFull//isFull変数用のプロパティ
        {
            get { return isFull; }//外部からは取得処理のみを可能に
        }

        private int nearItemNo;//Playerの最も近くにあるアイテムの番号

        public int NearItemNo//nearItemNo変数用のプロパティ
        {
            get { return nearItemNo; }//外部からは取得処理のみを可能に
        }

        private float lengthToNearItem;//「Playerの最も近くにあるアイテム」と「Player」との距離

        public float LengthToNearItem//lengthToNearItem変数用のプロパティ
        {
            get { return lengthToNearItem; }//外部からは取得処理のみを可能に
        }

        private int selectedItemNo = 1;//使用しているアイテムの番号

        public int SelectedItemNo//useItemNo変数用のプロパティ
        {
            get { return selectedItemNo; }//外部からは取得処理のみ可能に
        }


        void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }


        /// <summary>
        /// ゲーム開始直後に呼び出される
        /// </summary>
        private void Start() {
            //アイテムを生成
            GenerateItem();
        }

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        private void Update() {
            //Playerの最も近くにあるアイテムの情報を取得
            GetInformationOfNearItem(playerTran.position, true);
        }

        /// <summary>
        /// 指定したアイテムのデータを取得する
        /// </summary>
        /// <param name="itemName">アイテムの名前</param>
        /// <returns>アイテムのデータ</returns>
        public ItemDataSO.ItemData GetItemData(ItemDataSO.ItemName itemName) {
            //アイテムのデータのリストから要素を1つずつ取り出す
            foreach (ItemDataSO.ItemData itemData in itemDataSO.itemDataList) {
                //取り出した要素の名前が引数と同じだったら
                if (itemData.itemName == itemName) {
                    //取り出した要素を返す
                    return itemData;
                }
            }

            //nullを返す
            return null;
        }

        /// <summary>
        /// アイテムを生成する
        /// </summary>
        public void GenerateItem() {
            //アイテムの生成位置を生成する
            GenerateItemTran();

            //アイテムの生成位置のリストを作成
            CreateGeneratedItemTranList();

            //アイテムの位置情報をまとめたフォルダーの子オブジェクトだけ繰り返す
            for (int i = 0; i < itemTrans.childCount; i++) {
                //0から2までのランダムな整数を取得
                int px = Random.Range(0, 3);

                //取得した整数が0ではないなら（2/3の確率でEnemyが使える武器を出現させる）
                if (px != 0) {
                    //5から7までのランダムな整数を取得
                    int py = Random.Range(5, 8);

                    //指定した位置にランダムなアイテムを生成し、アニメーションを開始
                    StartCoroutine(PlayItemAnimation(Instantiate(itemDataSO.itemDataList[py].prefab, generatedItemTranList[i])));

                    //生成したアイテムのデータをリストに追加
                    generatedItemDataList.Add(itemDataSO.itemDataList[py]);

                    //次の繰り返し処理へ移る
                    continue;
                }

                //Enemyが使用できないアイテムの番号
                int[] randomNumbers = { 1, 2, 3, 4, 8, 9, 10, 11, 12, 13 };

                //上記の配列からランダムな要素を取得
                int pz = randomNumbers[Random.Range(0, 10)];

                //指定した位置にランダムなアイテムを生成し、アニメーションを開始
                StartCoroutine(PlayItemAnimation(Instantiate(itemDataSO.itemDataList[pz].prefab, generatedItemTranList[i])));

                //生成したアイテムのデータをリストに追加
                generatedItemDataList.Add(itemDataSO.itemDataList[pz]);
            }
        }

        /// <summary>
        /// アイテムの生成位置を生成する
        /// </summary>
        private void GenerateItemTran() {
            //生成するアイテムの生成位置の最大数だけ繰り返す
            for (int i = 0; i < maxItemTranCount; i++) {
                //アイテムの生成位置を設定
                Transform generateItemPosTran = Instantiate(generateItemPosPrefab);

                //生成したアイテムの生成位置の親を設定
                generateItemPosTran.SetParent(itemTrans);

                //0から3までのランダムな整数を取得
                int px = Random.Range(0, 4);

                //-120から120までのランダムな小数を取得
                float py = Random.Range(-120f, 120f);

                //pxの値に応じて処理を変更
                switch (px) {
                    case 0:
                        generateItemPosTran.localPosition = new Vector3(py, 0f, -120f);
                        break;

                    case 1:
                        generateItemPosTran.localPosition = new Vector3(120f, 0f, py);
                        break;

                    case 2:
                        generateItemPosTran.localPosition = new Vector3(py, 0f, 120f);
                        break;

                    case 3:
                        generateItemPosTran.localPosition = new Vector3(-120f, 0f, py);
                        break;
                }
            }
        }

        /// <summary>
        /// アイテムの生成位置のリストを作成する
        /// </summary>
        private void CreateGeneratedItemTranList() {
            //アイテムの位置情報をまとめたフォルダーの子オブジェクトの数だけ繰り返す
            for (int i = 0; i < itemTrans.childCount; i++) {
                //アイテムの位置情報をまとめたフォルダーの子オブジェクトの位置情報をリストに追加していく
                generatedItemTranList.Add(itemTrans.GetChild(i).transform);
            }
        }

        /// <summary>
        /// 最も近くにあるアイテムの番号と位置情報を得る
        /// </summary>
        /// <param name="myPos">自分自身の座標</param>
        /// <param name="isPlayerPos">第一引数はPlayerの座標かどうか</param>
        public void GetInformationOfNearItem(Vector3 myPos, bool isPlayerPos) {
            //nullエラー回避
            if (generatedItemTranList.Count <= 0) {
                //以降の処理を行わない
                return;
            }

            //アイテムの番号
            int itemNo = 0;

            //リストの0番の要素の座標をnearPosに仮に登録
            Vector3 nearPos = generatedItemTranList[0].position;

            //リストの要素数だけ繰り返す
            for (int i = 0; i < generatedItemTranList.Count; i++) {
                //繰り返し処理で得た要素がnullなら
                if (generatedItemTranList[i] == null) {
                    //次の繰り返し処理に移る
                    continue;
                }

                //リストのi番の要素の座標をposに登録
                Vector3 pos = generatedItemTranList[i].position;

                //仮登録した要素と、for文で得た要素の、myPosとの距離を比較
                if (Vector3.Scale((pos - myPos), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude) {
                    //Playerの最も近くにあるアイテムの番号をiで登録
                    itemNo = i;

                    //nearPosを再登録
                    nearPos = pos;
                }
            }

            //myPosがPlayerの座標なら
            if (isPlayerPos) {
                //Playerの最も近くにあるアイテムの番号を登録
                nearItemNo = itemNo;

                //「Playerの最も近くにあるアイテム」と「Player」との距離を登録
                lengthToNearItem = Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude;
            }
        }

        /// <summary>
        /// アイテムのアニメーションを行う
        /// </summary>
        /// <param name="itemPrefab">アイテムのプレファブ</param>
        /// <returns>待ち時間</returns>
        private IEnumerator PlayItemAnimation(GameObject itemPrefab) {
            //nullエラー回避
            if (itemPrefab != null) {
                //アイテムを上下に無限に運動させる
                itemPrefab.transform.DOLocalMoveY(0.5f, 2.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
            }

            //nullエラー回避
            while (itemPrefab != null) {
                //アイテムを回転させる
                itemPrefab.transform.Rotate(0, itemRotSpeed, 0);

                //次のフレームに飛ぶ（実質Updateメソッド）
                yield return null;
            }
        }

        /// <summary>
        /// アイテムを取得する
        /// </summary>
        /// <param name="nearItemNo">最も近くにあるアイテムの番号</param>
        /// <param name="isPlayer">アイテムの取得者がPlayerかどうか</param>
        public void GetItem(int nearItemNo, bool isPlayer) {
            //アイテムの取得者がPlayerではないなら
            if (!isPlayer) {
                //近くのアイテムをリストから削除する
                RemoveItemList(nearItemNo);

                //以降の処理を行わない
                return;
            }


/********************/


            //取得するアイテムが弾のアイテムではないなら
            if (generatedItemDataList[nearItemNo].itemType != ItemDataSO.ItemType.Bullet)  // .isNotBullet
            {
                //Playerが所持しているアイテムのリストの要素の数だけ繰り返す
                for (int i = 0; i < playerItemList.Count; i++) {
                    //i番目の要素が空なら
                    if (CheckTheElement(i)) {
                        //Playerが所持しているアイテムのリストの空いている要素に、アイテムの情報を代入
                        playerItemList[i] = generatedItemDataList[nearItemNo];

                        //まだ許容オーバーではない
                        isFull = false;

                        //繰り返し処理を終了する
                        break;
                    }
                }
            }
            //取得するアイテムが弾のアイテムなら
            else {
                //許容オーバーかどうか調べる
                CheckIsFull();
            }

            //許容オーバーではないか、取得するアイテムが弾なら
            if (!IsFull || generatedItemDataList[nearItemNo].itemType == ItemDataSO.ItemType.Bullet)  // .isNotBullet
            {
                //残弾数を更新
                //bulletManager.UpdateBulletCount(generatedItemDataList[nearItemNo].itemName);

                bulletManager.UpdateBulletCount(generatedItemDataList[nearItemNo].itemName, generatedItemDataList[nearItemNo].bulletCount);

                //全てのアイテムスロットのSpriteを再設定する
                SetIAlltemSlotSprite();

                //フロート表示を生成
                StartCoroutine(uIManager.GenerateFloatingMessage(generatedItemDataList[nearItemNo].itemName.ToString(), Color.blue));

                //近くのアイテムをリストから削除する
                RemoveItemList(nearItemNo);
            }

            //取得するアイテムが投擲武器ではないなら
            if (!generatedItemDataList[nearItemNo].isThrowingWeapon) {
                //以降の処理を行わない
                return;
            }

            //取得するアイテムが手榴弾かつ、Playerが所持しているアイテムのリストに手榴弾が既にあるなら
            if (generatedItemDataList[nearItemNo].itemName == ItemDataSO.ItemName.Grenade && playerItemList.Contains(itemDataSO.itemDataList[1])) {
                //TODO:手榴弾の残弾数を増やす処理
            }
            //取得するアイテムが催涙弾かつ、Playerが所持しているアイテムのリストに催涙弾が既にあるなら
            else if (generatedItemDataList[nearItemNo].itemName == ItemDataSO.ItemName.TearGasGrenade && playerItemList.Contains(itemDataSO.itemDataList[2])) {
                //TODO:催涙弾の残弾数を増やす処理
            }



/********************/


        }

        /// <summary>
        /// PlayerItemListの指定した番号の要素が空いているかどうかを調べる
        /// </summary>
        /// <param name="elementNo">要素の番号</param>
        /// <returns> PlayerItemListの指定した番号の要素が空いていたらtrueを返す</returns>
        public bool CheckTheElement(int elementNo) {
            //PlayerItemListの指定した番号の要素が空いていたらtrueを返す
            return playerItemList[elementNo].itemName == ItemDataSO.ItemName.None ? true : false;
        }

        /// <summary>
        /// 指定した番号のアイテムをリストから削除する
        /// </summary>
        /// <param name="itemNo">アイテムの番号</param>
        private void RemoveItemList(int itemNo) {
            //アイテムのデータのリストから要素を削除
            generatedItemDataList.RemoveAt(itemNo);

            //アイテムの位置情報のリストから要素を削除
            generatedItemTranList.RemoveAt(itemNo);

            //アイテムのゲームオブジェクトを消す
            Destroy(itemTrans.GetChild(itemNo).gameObject);
        }

        /// <summary>
        /// 選択されているアイテムのデータを取得する
        /// </summary>
        /// <returns>選択されているアイテムのデータ</returns>
        //public ItemDataSO.ItemData GetSelectedItemData() {
        //    //選択されているアイテムのデータをリストから取得して返す
        //    return playerItemList[playerController.SelectedItemNo - 1];
        //}

        /// <summary>
        /// アイテムを破棄する
        /// </summary>
        /// <param name="itemNo">破棄するアイテムの番号</param>
        public void DiscardItem(int itemNo) {
            //許容オーバーかどうか調べる
            CheckIsFull();

            //指定されたアイテムをリストから削除する
            playerItemList.RemoveAt(itemNo);

            //Playerが所持しているアイテムのリストの要素の数を一定に保つ
            playerItemList.Add(itemDataSO.itemDataList[0]);

            //全てのアイテムスロットのSpriteを再設定する
            SetIAlltemSlotSprite();
        }

        /// <summary>
        /// Playerが所持しているアイテムのリストを元に、全てのアイテムスロットのSpriteを設定する
        /// </summary>
        public void SetIAlltemSlotSprite() {
            //Playerが所持しているアイテムのリストの要素の数だけ繰り返す
            for (int i = 0; i < playerItemList.Count; i++) {
                //Playerの所持しているアイテムのi番目が空なら
                if (playerItemList[i].itemName == ItemDataSO.ItemName.None) {
                    //アイテムスロットのイメージを透明にする
                    uIManager.imgItemSlotList[i].DOFade(0f, 0f);

                    //以降の処理を行わない
                    return;
                }

                //全てのアイテムスロットのSpriteを設定する
                uIManager.SetItemSprite(i + 1, playerItemList[i].sprite);
            }
        }

        /// <summary>
        /// 許容オーバーかどうか調べる
        /// </summary>
        public void CheckIsFull() {
            //仮に許容オーバーの状態として登録する
            isFull = true;

            //Playerが所持しているアイテムのリストの要素の数だけ繰り返す
            for (int i = 0; i < playerItemList.Count; i++) {
                //i番目の要素が空なら
                if (CheckTheElement(i)) {
                    //まだ許容オーバーではない
                    isFull = false;
                }
            }
        }

        /// <summary>
        /// アイテムを使用する
        /// </summary>
        /// <param name="itemData">使用するアイテムのデータ</param>
        public void UseItem(ItemDataSO.ItemData itemData) {
            //使用するアイテムが銃火器なら
            if (itemData.isFirearms) {
                //弾を発射
                //StartCoroutine(bulletManager.ShotBullet(itemData));
                bulletManager.ShotBullet(itemData);
            }
            //使用するアイテムに回復効果があったら
            else if (itemData.restorativeValue > 0) {
                //PlayerのHpを更新
                playerHealth.UpdatePlayerHp(itemData.restorativeValue);
            }
        }

        ///// <summary>
        ///// アイテムを使用する
        ///// </summary>
        ///// <param name="itemData">使用するアイテムのデータ</param>
        //public void UseItem(ItemDataSO.ItemData itemData) {
        //    //使用するアイテムが飛び道具なら
        //    if (itemData.isMissile) {
        //        //弾を発射
        //        StartCoroutine(bulletManager.ShotBullet(itemData));
        //    }
        //    //使用するアイテムに回復効果があり、左クリックされたら
        //    else if (itemData.restorativeValue > 0 && Input.GetKeyDown(KeyCode.Mouse0)) {
        //        //効果音を再生
        //        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.RecoverySE));

        //        //PlayerのHpを更新
        //        playerHealth.UpdatePlayerHp(itemData.restorativeValue);

        //        //その回復アイテムの所持数を1減らす
        //        playerHealth.UpdateRecoveryItemCount(itemData.itemName, -1);

        //        //選択している回復アイテムの所持数が0になったら
        //        if (playerHealth.GetRecoveryItemCount(GetSelectedItemData().itemName) == 0) {
        //            //選択しているアイテムの要素を消す
        //            DiscardItem(playerController.SelectedItemNo - 1);
        //        }
        //    }
        //    //使用するアイテムが近接武器かつ、左クリックされたら
        //    else if (itemData.isHandWeapon && Input.GetKeyDown(KeyCode.Mouse0)) {
        //        //近接武器を使用する
        //        StartCoroutine(bulletManager.UseHandWeapon(itemData));
        //    }
        //}
    }
}
