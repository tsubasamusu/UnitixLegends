using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// 弾用の親クラス（バレットのプレファブにアタッチする）
    /// </summary>
    public class BulletDetailBase : WeaponBase 
    {
        protected float effectDuration;//エフェクトの方向

        /// <summary>
        /// 弾の設定
        /// </summary>
        /// <param name="attackPower">弾の威力</param>
        /// <param name="bulletOwnerType">弾の所有者</param>
        /// <param name="direction">弾の飛ぶ方向</param>
        /// <param name="seName">SE の種類</param>
        /// <param name="duration">弾の持続時間</param>
        /// <param name="effectPrefab">エフェクトのプレファブ</param>
        public virtual void SetUpBulletDetail(float attackPower, BulletOwnerType bulletOwnerType, Vector3 direction, SeName seName, float duration = 3.0f, GameObject effectPrefab = null) 
        {
            //既定値にリセットする
            Reset();

            //弾の威力を取得
            this.attackPower = attackPower;

            //弾の所有者を取得
            BulletOwnerType = bulletOwnerType;

            //弾を発射する
            TriggerBullet(direction, duration);

            //効果音が既にあるなら
            if (seName != SeName.None) 
            {
                //効果音を再生
                SoundManager.instance.PlaySE(seName);
            }

            //エフェクトが既にあるなら
            if (effectPrefab != null) 
            {
                //エフェクト生成
                GenerateEffect(effectPrefab);
            }
        }

        /// <summary>
        /// 弾を発射する
        /// </summary>
        /// <param name="direction">弾を消すまでの時間</param>
        /// <param name="soundType">音の種類</param>
        protected virtual void TriggerBullet(Vector3 direction, float duration) 
        {
            //力を加える
            rb.AddForce(direction);

            //弾を消す
            Destroy(gameObject, duration);
        }

        /// <summary>
        /// エフェクトを生成する
        /// </summary>
        /// <param name="effect">エフェクト</param>
        protected virtual void GenerateEffect(GameObject effectPrefab) 
        {
            //エフェクトを生成し、親をShoBulletに設定する
            GameObject effect = Instantiate(effectPrefab, transform);

            //生成したエフェクトを指定時間後に消す
            Destroy(effect, effectDuration);
        }

        /// <summary>
        /// 攻撃力を取得する
        /// </summary>
        /// <returns>攻撃力</returns>
        public float GetAttackPower() 
        {
            //攻撃力を返す
            return attackPower;
        }

        /// <summary>
        /// 追加効果
        /// </summary>
        public virtual void AddTriggerBullet(EnemyController enemyController) 
        {
            //追加効果を子クラスで記述する
        }
    }
}