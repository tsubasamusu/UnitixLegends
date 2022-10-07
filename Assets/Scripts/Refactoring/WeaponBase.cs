using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// 武器の所有者の種類
    /// </summary>
    public enum BulletOwnerType 
    {
        Player,//プレイヤー
        Enemy//エネミー
    }

    /// <summary>
    /// 弾と近接武器の両方で共通する処理を記載している、武器全体の親クラス
    /// </summary>
    public class WeaponBase : MonoBehaviour 
    {
        protected float attackPower;//攻撃力

        protected Rigidbody rb;//Rigidbody

        protected BulletOwnerType bulletOwnerType;//弾の所有者

        /// <summary>
        /// 弾の所有者の取得・設定用
        /// </summary>
        public BulletOwnerType BulletOwnerType { get => bulletOwnerType; set => bulletOwnerType = value; }

        /// <summary>
        /// リセットする
        /// </summary>
        protected void Reset() 
        {
            //Rigidbodyの取得に失敗したら
            if (!TryGetComponent(out rb))
            {
                //問題を報告
                Debug.Log("Rigidbody が取得出来ません。");
            }    
        }
    }
}