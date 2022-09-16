using UnityEngine;

namespace yamap {

    /// <summary>
    /// 武器の所有者の種類
    /// </summary>
    public enum BulletOwnerType {
        Player,
        Enemy
    }

    /// <summary>
    /// 弾と近接武器の両方で共通する処理を記載している、武器全体の親クラス
    /// </summary>
    public class WeaponBase : MonoBehaviour {

        protected float attackPower;
        protected Rigidbody rb;

        // 弾の所有者
        protected BulletOwnerType bulletOwnerType;

        /// <summary>
        /// 弾の所有者のプロパティ
        /// </summary>
        public BulletOwnerType BulletOwnerType { get => bulletOwnerType; set => bulletOwnerType = value; }

        protected void Reset() {
            if (!TryGetComponent(out rb)) {
                Debug.Log("Rigidbody が取得出来ません。");
            }    
        }
    }
}