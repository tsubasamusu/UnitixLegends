using System.Collections;
using System.Collections.Generic;//リストを使用
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//インスタンス

    [SerializeField]
    private Transform itemTrans;//アイテムの位置情報をまとめたフォルダー

    [SerializeField]
    private ItemDataSO ItemDataSO;//ItemDataSO

    public List<ItemDataSO.ItemData> generatedItemDataList = new List<ItemDataSO.ItemData>();//生成したアイテムのデータのリスト

    public List<Transform> generatedItemTranlist=new List<Transform>();//アイテムの生成位置のリスト

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
        //アイテムの生成位置のリストを作成
        CreateGeneratedItemTranList();

        //アイテムを生成
        GenerateItem();
    }

    /// <summary>
    /// アイテムを生成する
    /// </summary>
    public void GenerateItem()
    {
        //アイテムの位置情報をまとめたフォルダーの子オブジェクトの数だけ繰り返す
        for (int i = 0; i < itemTrans.childCount; i++)
        {
            //ランダムな整数を取得
            int px = Random.Range(0, 13);

            //指定した位置にランダムなアイテムを生成
            Instantiate(ItemDataSO.itemDataList[px].prefab, generatedItemTranlist[i]);

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
            generatedItemTranlist.Add(itemTrans.GetChild(i).transform);
        }
    }
}
