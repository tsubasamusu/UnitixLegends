using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine;
using DG.Tweening;//DOTweenを使用

public class GameData : MonoBehaviour
{
    public static GameData instance;//インスタンス

    [SerializeField]
    private ItemDataSO ItemDataSO;//ItemDataSO

    [SerializeField]
    private UIManager uIManager;//UIManager

    [SerializeField]
    private Transform itemTrans;//アイテムの位置情報をまとめたフォルダー

    [SerializeField]
    private Transform playerTran;//Playerの位置情報

    [SerializeField]
    private float itemRotSpeed;//アイテムの回転速度

    [HideInInspector]
    public List<ItemDataSO.ItemData> generatedItemDataList = new List<ItemDataSO.ItemData>();//生成したアイテムのデータのリスト

    [HideInInspector]
    public List<Transform> generatedItemTranList = new List<Transform>();//アイテムの生成位置のリスト

    public List<ItemDataSO.ItemData> playerItemList=new List<ItemDataSO.ItemData>();//Playerが所持しているアイテムのリスト

    private int nearItemNo;//Playerの最も近くにあるアイテムの番号

    private bool isFull;//Playerの所有物が許容オーバーかどうか

    public bool IsFull//isFull変数用のプロパティ
    {
        get { return isFull; }//外部からは取得処理のみを可能に
    }

    public int NearItemNo//nearItemNo変数用のプロパティ
    {
        get { return nearItemNo; }//外部からは取得処理のみを可能に
    }

    private float lengthToNearItem;//「Playerの最も近くにあるアイテム」と「Player」との距離

    public float LengthToNearItem//lengthToNearItem変数用のプロパティ
    {
        get { return lengthToNearItem; }//外部からは取得処理のみを可能に
    }

    /// <summary>
    /// Startメソッドより前に呼び出される（以下、シングルトンに必須の記述）
    /// </summary>
    private void Awake()
    {
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
    /// ゲーム開始直後に呼び出される
    /// </summary>
    private void Start()
    {
        //アイテムを生成
        GenerateItem();
    }

    /// <summary>
    /// 毎フレーム呼び出される
    /// </summary>
    private void Update()
    {
        //Playerの最も近くにあるアイテムの情報を取得
        GetInformationOfNearItem(playerTran.position, true);
    }

    /// <summary>
    /// アイテムを生成する
    /// </summary>
    public void GenerateItem()
    {
        //アイテムの生成位置のリストを作成
        CreateGeneratedItemTranList();

        //アイテムの位置情報をまとめたフォルダーの子オブジェクトの数だけ繰り返す
        for (int i = 0; i < itemTrans.childCount; i++)
        {
            //ランダムな整数を取得
            int px = Random.Range(1, 14);

            //指定した位置にランダムなアイテムを生成し、アニメーションを開始
            StartCoroutine(PlayItemAnimation( Instantiate(ItemDataSO.itemDataList[px].prefab, generatedItemTranList[i])));

            //生成したアイテムのデータをリストに追加
            generatedItemDataList.Add(ItemDataSO.itemDataList[px]);
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
    /// 最も近くにあるアイテムの情報を得る
    /// </summary>
    public void GetInformationOfNearItem(Vector3 myPos, bool isPlayerPos)
    {
        //nullエラー回避
        if (generatedItemTranList.Count <= 0)
        {
            //以降の処理を行わない
            return;
        }

        //アイテムの番号
        int itemNo=0;

        //リストの0番の要素の座標をnearPosに仮に登録
        Vector3 nearPos = generatedItemTranList[0].position;

        //リストの要素数だけ繰り返す
        for (int i = 0; i < generatedItemTranList.Count; i++)
        {
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

        //myPosがPlayerの座標なら
        if (isPlayerPos)
        {
            //Playerの最も近くにあるアイテムの番号を登録
            nearItemNo = itemNo;

            //「Playerの最も近くにあるアイテム」と「Player」との距離を登録
            lengthToNearItem = Vector3.Scale((nearPos - myPos), new Vector3(1, 0, 1)).magnitude;
        }
    }

    /// <summary>
    /// アイテムのアニメーションを行う
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayItemAnimation(GameObject itemPrefab)
    {
        //nullエラー回避
        if (itemPrefab != null)
        {
            //アイテムを上下に無限に運動させる
            itemPrefab.transform.DOLocalMoveY(0.5f, 2.0f).SetEase(Ease.Linear). SetLoops(-1, LoopType.Yoyo);
        }

        //nullエラー回避
        while (itemPrefab != null)
        {
            //アイテムを回転させる
            itemPrefab.transform.Rotate(0, itemRotSpeed, 0);

            //次のフレームに飛ぶ（実質Updateメソッド）
            yield return null;
        }
    }

    /// <summary>
    /// アイテムを取得する処理
    /// </summary>
    /// <param name="nearItemNo">最も近くにあるアイテムの番号</param>
    /// <param name="isPlayer">アイテムの取得者がPlayerかどうか</param>
    public void GetItem(int nearItemNo, bool isPlayer)
    {
        //アイテムの取得者がPlayerではないなら
        if (!isPlayer)
        {
            //以降の処理を行わない
            return;
        }

        //仮に許容オーバーの状態として登録する
        isFull = true;

        //取得するアイテムが弾のアイテムではないなら
        if (generatedItemDataList[nearItemNo].isNotBullet)
        {
            //Playerが所持しているアイテムのリストの要素の数だけ繰り返す
            for (int i = 0; i < playerItemList.Count; i++)
            {
                //i番目の要素が空なら
                if (CheckTheElement(i))
                {
                    //アイテムスロットのSpriteを設定
                    uIManager.SetItemSprite(i+1, generatedItemDataList[nearItemNo].sprite);

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

        //許容オーバーではないなら
        if (!isFull)
        {
            //フロート表示を生成
            StartCoroutine(uIManager.GenerateFloatingMessage(generatedItemDataList[nearItemNo].itemName.ToString(), Color.blue));

            //近くのアイテムをリストから削除する
            RemoveItemList(nearItemNo);
        }
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
}
