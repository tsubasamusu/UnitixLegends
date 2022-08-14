using System.Collections;
using System.Collections.Generic;//リストを使用
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//インスタンス

    [SerializeField]
    private GameObject itemTrans;//アイテムの位置情報をまとめたフォルダー

    public List<Transform> generateItemTranList = new List<Transform>();//アイテムの生成位置のリスト

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
    /// アイテムを生成する
    /// </summary>
    public void GenerateItem()
    {

    }
}
