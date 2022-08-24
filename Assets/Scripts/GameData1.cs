using System.Collections;//IEnumeratorを使用
using System.Collections.Generic;//リストを使用
using UnityEngine;
using DG.Tweening;//DOTweenを使用

namespace yamap {
    public class GameData : MonoBehaviour {

        public static GameData instance;//インスタンス

        [SerializeField]
        private float fallSpeed;//落下速度

        public float FallSpeed//fallSpeed変数用のプロパティ
        {
            get { return fallSpeed; }//外部からは取得処理のみを可能に
        }

        private int killCount;//Playerが倒した敵の数

        public int KillCount//killCount変数用のプロパティ
        {
            get { return killCount; }//取得処理
            set { killCount = value; }//設定処理
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
    }
}