using UnityEngine;

namespace yamap {

    /// <summary>
    /// バレットのプレファブにアタッチする、弾用の親クラス
    /// </summary>
    public class BulletDetailBase : WeaponBase {

        protected float effectDuration;

        /// <summary>
        /// 弾の設定
        /// </summary>
        /// <param name="attackPower">弾の威力</param>
        /// <param name="bulletOwnerType">弾の所有者</param>
        /// <param name="direction">弾の飛ぶ方向</param>
        /// <param name="seName">SE の種類</param>
        /// <param name="duration">弾の持続時間</param>
        /// <param name="effectPrefab">エフェクトのプレファブ</param>
        public virtual void SetUpBulletDetail(float attackPower, BulletOwnerType bulletOwnerType, Vector3 direction, SeName seName, float duration = 3.0f, GameObject effectPrefab = null) {
            Reset();

            this.attackPower = attackPower;
            BulletOwnerType = bulletOwnerType;

            // 弾の発射
            TriggerBullet(direction, duration);


            // SE があるなら
            if (seName != SeName.None) {
                // SE 再生
                SoundManager.instance.PlaySE(seName);
            }

            // エフェクトがあるなら
            if (effectPrefab != null) {
                // エフェクト生成
                GenerateEffect(effectPrefab);
            }
        }

        /// <summary>
        /// 弾の発射
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="soundType"></param>
        /// <param name="duration"></param>
        protected virtual void TriggerBullet(Vector3 direction, float duration) {
            rb.AddForce(direction);

            Destroy(gameObject, duration);
        }

        /// <summary>
        /// エフェクト生成
        /// </summary>
        /// <param name="effect"></param>
        protected virtual void GenerateEffect(GameObject effectPrefab) {
            //エフェクトを生成し、親をShoBulletに設定
            GameObject effect = Instantiate(effectPrefab, transform);

            //生成したエフェクトを指定時間後に消す
            Destroy(effect, effectDuration);
        }

        public float GetAttackPower() {
            return attackPower;
        }

        /// <summary>
        /// 追加効果
        /// </summary>
        public virtual void AddTriggerBullet(EnemyController enemyController) {
            Debug.Log("追加効果を子クラスで記述する");
        }
    }
}