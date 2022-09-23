using DG.Tweening;//DOTweenを使用
using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine;

namespace yamap 
{
    public class ItemManager : MonoBehaviour 
    {
        public static ItemManager instance;//インスタンス

        [SerializeField]
        private ItemDataSO itemDataSO;//ItemDataSO

        /// <summary>
        /// ItemDataSO取得用
        /// </summary>
        public ItemDataSO ItemDataSO { get => itemDataSO; }

        private UIManager uIManager;//UIManager

        private BulletManager bulletManager;//BulletManager

        [SerializeField]
        private Transform itemTrans;//アイテムの位置情報をまとめたフォルダー

        [SerializeField]
        private Transform generateItemPosPrefab;//アイテムの生成位置のプレファブ

        private Transform playerTran;//Playerの位置情報

        [SerializeField]
        private float itemRotSpeed;//アイテムの回転速度

        [SerializeField]
        private int maxItemTranCount;//生成するアイテムの生成位置の最大数

        public List<ItemDataSO.ItemData> generatedItemDataList = new List<ItemDataSO.ItemData>();//生成したアイテムのデータのリスト

        public List<Transform> generatedItemTranList = new List<Transform>();//アイテムの生成位置のリスト

        public List<ItemDataSO.ItemData> playerItemList = new List<ItemDataSO.ItemData>(5);//Playerが所持しているアイテムのリスト

        private bool isFull;//Playerの所有物が許容オーバーかどうか

        /// <summary>
        /// 許容オーバー判定取得用
        /// </summary>
        public bool IsFull{ get=>isFull;}

        private int nearItemNo;//Playerの最も近くにあるアイテムの番号

        /// <summary>
        /// Playerの最も近くにあるアイテムの番号の取得用
        /// </summary>
        public int NearItemNo { get => nearItemNo;}

        private float lengthToNearItem;//「Playerの最も近くにあるアイテム」と「Player」との距離

        /// <summary>
        /// 「Playerの最も近くにあるアイテム」と「Player」との距離の取得用
        /// </summary>
        public float LengthToNearItem { get => lengthToNearItem;}

        private int selectedItemNo = 0;//使用しているアイテムの番号

        /// <summary>
        /// 使用しているアイテムの番号の取得・設定用
        /// </summary>
        public int SelectedItemNo
        {
            get { return selectedItemNo; }
            set { selectedItemNo = value; }
        }

        /// <summary>
        /// Startメソッドより前に呼び出される
        /// </summary>
        void Awake() 
        {
            //以下、シングルトンに必須の記述
            if (instance == null) 
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } 
            else 
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// ItemManagerの初期設定を行う
        /// </summary>
        /// <param name="uiManager">UIManager</param>
        /// <param name="playerController">PlayerController</param>
        /// <param name="bulletManager">BulletManager</param>
        public void SetUpItemManager(UIManager uiManager, PlayerController playerController, BulletManager bulletManager)
        { 
            //UIManagerを取得
            this.uIManager = uiManager;

            //Playerの位置情報を取得
            playerTran = playerController.transform;

            //BulletManagerを取得
            this.bulletManager = bulletManager;

            //プレイヤーとエネミーのアイテムの種類を仕分けする
            (List<int> playerItemNums, List<int> enemyItemNums) sortingItemNums = GetSotringItemNums();

            /// <summary>
            /// プレイヤーとエネミーのアイテムとで仕分けしたリストを作成する
            /// </summary>
            /// <returns>プレイヤーとエネミーのアイテムとで仕分けしたリスト</returns>
            (List<int> playerNums, List<int> enemyNums) GetSotringItemNums() 
            {

                List<int> playerList = new();
                List<int> enemyList = new();

                //アイテムの数だけ繰り返す
                for (int i = 0; i < itemDataSO.itemDataList.Count; i++) 
                {
                    //最初のアイテムはNoneなので飛ばす
                    if (i == 0) 
                    {
                        //次の繰り返し処理へ飛ぶ
                        continue;
                    }

                    //エネミーの使用できるアイテムなら
                    if (itemDataSO.itemDataList[i].enemyCanUse) 
                    {
                        //エネミー用のリストに追加
                        enemyList.Add(itemDataSO.itemDataList[i].itemNo);
                    } 
                    //エネミーが使用できないアイテムなら
                    else
                    {
                        //プレイヤー用のリストに追加
                        playerList.Add(itemDataSO.itemDataList[i].itemNo);
                    }
                }

                //作成した2つのリストを返す
                return (playerList, enemyList);
            }

            //アイテムを生成する
            GenerateItem(sortingItemNums.playerItemNums, sortingItemNums.enemyItemNums);
        }

        /// <summary>
        /// 毎フレーム呼び出される
        /// </summary>
        private void Update() 
        {
            //Playerが存在しないなら（nullエラー回避）
            if (!playerTran) 
            {
                //以降の処理を行わない
                return;
            }

            //Playerの最も近くにあるアイテムの情報を取得
            GetInformationOfNearItem(playerTran.position);
        }

        /// <summary>
        /// 指定したアイテムのデータを取得する
        /// </summary>
        /// <param name="itemName">アイテムの名前</param>
        /// <returns>アイテムのデータ</returns>
        public ItemDataSO.ItemData GetItemData(ItemDataSO.ItemName itemName) 
        {
            //指定したアイテムのデータを返す
            return itemDataSO.itemDataList.Find(x => x.itemName == itemName);
        }

        /// <summary>
        /// アイテムを生成する
        /// </summary>
        /// <param name="playerItemNums">Player用のアイテムのリスト</param>
        /// <param name="enemyItemNums">Enemy用のアイテムのリスト</param>
        public void GenerateItem(List<int> playerItemNums, List<int> enemyItemNums) 
        {
            //アイテムの生成位置を生成する
            GenerateItemTran();

            //アイテムの生成位置のリストを作成
            CreateGeneratedItemTranList();

            //アイテムの位置情報をまとめたフォルダーの子オブジェクトだけ繰り返す
            for (int i = 0; i < itemTrans.childCount; i++) 
            {
                //0から2までのランダムな整数を取得
                int px = Random.Range(0, 3);

                //取得した整数が0ではないなら（2/3の確率でEnemyが使える武器を出現させる）
                if (px != 0) 
                {
                    //ランダムな整数を取得
                    int py = enemyItemNums[Random.Range(0, enemyItemNums.Count)];
                    
                    //指定した位置にランダムなアイテムを生成し、アニメーションを開始
                    StartCoroutine(PlayItemAnimation(Instantiate(itemDataSO.itemDataList[py].itemPrefab, generatedItemTranList[i])));

                    //生成したアイテムのデータをリストに追加
                    generatedItemDataList.Add(itemDataSO.itemDataList[py]);

                    //次の繰り返し処理へ移る
                    continue;
                }

                //ランダムな整数を取得
                int pz = playerItemNums[Random.Range(0, playerItemNums.Count)];
                
                //指定した位置にランダムなアイテムを生成し、アニメーションを開始
                StartCoroutine(PlayItemAnimation(Instantiate(itemDataSO.itemDataList[pz].itemPrefab, generatedItemTranList[i])));

                //生成したアイテムのデータをリストに追加
                generatedItemDataList.Add(itemDataSO.itemDataList[pz]);
            }
        }

        /// <summary>
        /// アイテムの生成位置を生成する
        /// </summary>
        private void GenerateItemTran() 
        {
            //生成するアイテムの生成位置の最大数だけ繰り返す
            for (int i = 0; i < maxItemTranCount; i++) 
            {
                //アイテムの生成位置を設定
                Transform generateItemPosTran = Instantiate(generateItemPosPrefab);

                //生成したアイテムの生成位置の親を設定
                generateItemPosTran.SetParent(itemTrans);

                //0から3までのランダムな整数を取得
                int px = Random.Range(0, 4);

                //-120から120までのランダムな小数を取得
                float py = Random.Range(-120f, 120f);

                //pxの値に応じて処理を変更
                generateItemPosTran.localPosition = px switch 
                {
                    0 => new (py, 0f, -120f),
                    1 => new (120f, 0f, py),
                    2 => new (py, 0f, 120f),
                    3 => new (-120f, 0f, py),
                    _ => Vector3.zero,
                };
            }
        }

        /// <summary>
        /// アイテムの生成位置のリストを作成する
        /// </summary>
        private void CreateGeneratedItemTranList() 
        {
            //アイテムの位置情報をまとめたフォルダーの子オブジェクトの数だけ繰り返す
            for (int i = 0; i < itemTrans.childCount; i++) 
            {
                //アイテムの位置情報をまとめたフォルダーの子オブジェクトの位置情報をリストに追加していく
                generatedItemTranList.Add(itemTrans.GetChild(i).transform);
            }
        }

        /// <summary>
        /// 最も近くにあるアイテムの番号と位置情報を得る
        /// </summary>
        /// <param name="myPos">自分自身の座標</param>
        /// <param name="isPlayerPos">第一引数はPlayerの座標かどうか</param>
        public void GetInformationOfNearItem(Vector3 myPos) 
        {
            //nullエラー回避
            if (generatedItemTranList.Count <= 0) 
            {
                //以降の処理を行わない
                return;
            }

            //アイテムの番号
            int itemNo = 0;

            //リストの0番の要素の座標をnearPosに仮に登録
            Vector3 nearPos = generatedItemTranList[0].position;

            //リストの要素数だけ繰り返す
            for (int i = 0; i < generatedItemTranList.Count; i++) 
            {
                //繰り返し処理で得た要素がnullなら
                if (generatedItemTranList[i] == null) 
                {
                    //次の繰り返し処理に移る
                    continue;
                }

                //リストのi番の要素の座標をposに登録
                Vector3 pos = generatedItemTranList[i].position;

                //仮登録した要素と、for文で得た要素の、myPosとの距離を比較
                if (Vector3.Scale((pos - myPos), new Vector3(1, 0, 1)).magnitude < Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude) 
                {
                    //Playerの最も近くにあるアイテムの番号をiで登録
                    itemNo = i;

                    //nearPosを再登録
                    nearPos = pos;
                }
            }

            //Playerの最も近くにあるアイテムの番号を登録
            nearItemNo = itemNo;

            //「Playerの最も近くにあるアイテム」と「Player」との距離を登録
            lengthToNearItem = Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude;
        }
        
        /// <summary>
        /// アイテムのアニメーションを行う
        /// </summary>
        /// <param name="item">アイテム</param>
        /// <returns>待ち時間</returns>
        private IEnumerator PlayItemAnimation(ItemDetail item) 
        {
            //nullエラー回避
            if (item != null) 
            {
                //アイテムを上下に無限に運動させる
                item.transform.DOLocalMoveY(0.5f, 2.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(item.gameObject);
            }

            //nullエラー回避
            while (item != null) 
            {
                //アイテムを回転させる
                item.transform.Rotate(0, itemRotSpeed, 0);

                //次のフレームに飛ぶ（実質Updateメソッド）
                yield return null;
            }
        }

        /// <summary>
        /// アイテムを取得する
        /// </summary>
        /// <param name="nearItemNo">最も近くにあるアイテムの番号</param>
        /// <param name="isPlayer">アイテム取得者がPlayerかどうか</param>
        /// <param name="playerHealth">PlayerHealth</param>
        public void GetItem(int nearItemNo, bool isPlayer, PlayerHealth playerHealth = null) 
        {
            //アイテムの取得者がPlayerではないなら
            if (!isPlayer) 
            {
                //近くのアイテムをリストから削除する
                RemoveItemList(nearItemNo);

                //以降の処理を行わない
                return;
            }

            //取得するアイテムが弾のアイテムではないなら
            if (generatedItemDataList[nearItemNo].itemType != ItemDataSO.ItemType.Bullet)
            {
                //Playerが所持しているアイテムのリストの要素の数だけ繰り返す
                for (int i = 0; i < playerItemList.Count; i++) 
                {
                    //i番目の要素が空なら
                    if (CheckTheElement(i))
                    {
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
            else 
            {
                //許容オーバーかどうか調べる
                CheckIsFull();
            }

            //許容オーバーかつ、取得するアイテムが弾ではないなら
            if (isFull && generatedItemDataList[nearItemNo].itemType != ItemDataSO.ItemType.Bullet)
            {
                //以降の処理を行わない
                return;
            }

            //取得するアイテムが飛び道具なら
            if (generatedItemDataList[nearItemNo].itemType == ItemDataSO.ItemType.Missile || generatedItemDataList[nearItemNo].itemType == ItemDataSO.ItemType.Bullet) 
            {
                //残弾数を更新
                bulletManager.UpdateBulletCount(generatedItemDataList[nearItemNo].itemName, generatedItemDataList[nearItemNo].bulletCount);
            }
            //取得するアイテムに回復効果があるなら
            else if (generatedItemDataList[nearItemNo].restorativeValue > 0) 
            {
                //回復アイテムの所持数を更新
                playerHealth.UpdateRecoveryItemCount(generatedItemDataList[nearItemNo].itemName, generatedItemDataList[nearItemNo].bulletCount);
            }

            //全てのアイテムスロットのSpriteを再設定する
            SetIAlltemSlotSprite();

            //フロート表示を生成
            StartCoroutine(uIManager.GenerateFloatingMessage(generatedItemDataList[nearItemNo].itemName.ToString(), Color.blue));

            //近くのアイテムをリストから削除する
            RemoveItemList(nearItemNo);
        }

        /// <summary>
        /// PlayerItemListの指定した番号の要素が空いているかどうかを調べる
        /// </summary>
        /// <param name="elementNo">要素の番号</param>
        /// <returns> PlayerItemListの指定した番号の要素が空いていたらtrueを返す</returns>
        public bool CheckTheElement(int elementNo)
        {
            //PlayerItemListの指定した番号の要素が空いていたらtrueを返す
            return playerItemList[elementNo].itemName == ItemDataSO.ItemName.None ? true : false;
        }

        /// <summary>
        /// 指定した番号のアイテムをリストから削除する
        /// </summary>
        /// <param name="itemNo">アイテムの番号</param>
        private void RemoveItemList(int itemNo) 
        {
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
        public ItemDataSO.ItemData GetSelectedItemData() 
        {
            //選択されているアイテムのデータをリストから取得して返す
            return playerItemList[SelectedItemNo];
        }

        /// <summary>
        /// アイテムを破棄する
        /// </summary>
        /// <param name="itemNo">破棄するアイテムの番号</param>
        public void DiscardItem(int itemNo) 
        {
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
        public void SetIAlltemSlotSprite() 
        {
            //Playerが所持しているアイテムのリストの要素の数だけ繰り返す
            for (int i = 0; i < playerItemList.Count; i++) 
            {
                //Playerの所持しているアイテムのi番目が空なら
                if (playerItemList[i].itemName == ItemDataSO.ItemName.None) 
                {
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
        public void CheckIsFull() 
        {
            //仮に許容オーバーの状態として登録する
            isFull = true;

            //Playerが所持しているアイテムのリストの要素の数だけ繰り返す
            for (int i = 0; i < playerItemList.Count; i++) 
            {
                //i番目の要素が空なら
                if (CheckTheElement(i)) 
                {
                    //まだ許容オーバーではない
                    isFull = false;
                }
            }
        }

       /// <summary>
       /// アイテムを使用する
       /// </summary>
       /// <param name="itemData">アイテムのデータ</param>
       /// <param name="playerHealth">PlayerHealth</param>
        public void UseItem(ItemDataSO.ItemData itemData, PlayerHealth playerHealth) 
        {
            //使用するアイテムが飛び道具なら
            if (itemData.itemType == ItemDataSO.ItemType.Missile) 
            {
                //弾を発射
                bulletManager.ShotBullet(itemData);
            }
            //使用するアイテムに回復効果があり、左クリックされたら
            else if (itemData.restorativeValue > 0 && Input.GetKeyDown(KeyCode.Mouse0)) 
            {
                //効果音を再生
                SoundManager.instance.PlaySE(SeName.RecoverySE);

                //PlayerのHpを更新
                playerHealth.UpdatePlayerHp(itemData.restorativeValue);

                //その回復アイテムの所持数を1減らす
                playerHealth.UpdateRecoveryItemCount(itemData.itemName, -1);

                //選択している回復アイテムの所持数が0になったら
                if (playerHealth.GetRecoveryItemCount(GetSelectedItemData().itemName) == 0) 
                {
                    //選択しているアイテムの要素を消す
                    DiscardItem(SelectedItemNo);
                }
            }
            //使用するアイテムが近接武器かつ、左クリックされたら
            else if (itemData.itemType == ItemDataSO.ItemType.HandWeapon && Input.GetKeyDown(KeyCode.Mouse0)) 
            {
                //近接武器を使用する
                bulletManager.PrepareUseHandWeapon(itemData);
            }
        }
    }
}
