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
            Instantiate(ItemDataSO.itemDataList[Random.Range(0, 13)].prefab, CreateItemPosList()[i]);
        }
    }

    /// <summary>
    /// アイテムの生成位置のリストを作成する
    /// </summary>
    private List<Transform> CreateItemPosList()
    {
        //リストを作成
        List<Transform> generateItemPosList = new List<Transform>();

        //アイテムの位置情報をまとめたフォルダーの子オブジェクトの数だけ繰り返す
        for (int i = 0; i < itemTrans.childCount; i++)
        {
            //アイテムの位置情報をまとめたフォルダーの子オブジェクトの位置情報をリストに追加していく
            generateItemPosList.Add(itemTrans.GetChild(i).transform);
        }

        //生成したリストを返す
        return generateItemPosList;
    }
}
