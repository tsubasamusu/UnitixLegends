using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine;
using DG.Tweening;//DOTweenを使用

namespace yamap {
    public class GameData : MonoBehaviour {

        public static GameData instance;//インスタンス

        public List<ItemDataSO.ItemData> playerItemList = new List<ItemDataSO.ItemData>();//Playerが所持しているアイテムのリスト

        private bool isFull;//Playerの所有物が許容オーバーかどうか

        public bool IsFull//isFull変数用のプロパティ
        {
            get => isFull;
            set => isFull = value;//外部からは取得処理のみを可能に
        }

        private int killCount;//Playerが倒した敵の数

        public int KillCount//killCount変数用のプロパティ
        {
            get { return killCount; }//取得処理
            set { killCount = value; }//設定処理
        }

        private int selectedItemNo = 1;//使用しているアイテムの番号

        public int SelectedItemNo//useItemNo変数用のプロパティ
        {
            get { return selectedItemNo; }//外部からは取得処理のみ可能に
            set { selectedItemNo = value; }
        }

        /// <summary>
        /// Startメソッドより前に呼び出される（以下、シングルトンに必須の記述）
        /// </summary>
        private void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
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
        /// 許容オーバーかどうか調べる
        /// </summary>
        public bool CheckIsFull() {
            //仮に許容オーバーの状態として登録する
            //isFull = true;

            //Playerが所持しているアイテムのリストの要素の数だけ繰り返す
            for (int i = 0; i < playerItemList.Count; i++) {
                //i番目の要素が空ではないなら
                if (!CheckTheElement(i)) {
                    //許容オーバー
                    //isFull = false;
                    return true;
                }
            }
            // チェックが通ったら、空がある
            return false;
        }
    }
}