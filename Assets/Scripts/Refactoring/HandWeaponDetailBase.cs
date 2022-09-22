namespace yamap 
{
    /// <summary>
    /// 近接武器のプレファブにアタッチする、近接武器の親クラス
    /// </summary>
    public class HandWeaponDetailBase : WeaponBase 
    {    
        /// <summary>
        /// WeaponDetailの初期設定を行う
        /// </summary>
        /// <param name="attackPower">攻撃力</param>
        /// <param name="bulletOwnerType">弾の所有者</param>
        /// <param name="soundType">音の種類</param>
        /// <param name="duration">ゲームオブジェクトを消すまでの時間</param>
        public virtual void SetUpWeaponDetail(float attackPower, BulletOwnerType bulletOwnerType, SeName soundType, float duration) 
        {
            //攻撃力を取得
            this.attackPower = attackPower;
            
            //弾の所有者を取得
            BulletOwnerType = bulletOwnerType;

            //攻撃を行う
            TriggerWeapon();

            //効果音があるなら（nullエラー回避）
            if (soundType != SeName.None) 
            {
                //効果音を再生
                SoundManager.instance.PlaySE(soundType);
            }

            //一定時間経過後にゲームオブジェクトを消す
            Destroy(gameObject, duration);
        }

        /// <summary>
        /// 武器の効果
        /// </summary>
        protected virtual void TriggerWeapon() 
        {
            //子クラスに、各武器の処理を記載する
        }
    }
}