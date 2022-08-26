namespace yamap {

    /// <summary>
    /// 近接武器のプレファブにアタッチする、近接武器の親クラス
    /// </summary>
    public class HandWeaponDetailBase : WeaponBase {
        
        public virtual void SetUpWeaponDetail(float attackPower, BulletOwnerType bulletOwnerType, SoundDataSO.SoundEffectName soundType, float duration) {
            this.attackPower = attackPower;
            BulletOwnerType = bulletOwnerType;

            // 武器の攻撃
            TriggerWeapon();

            // SE があるなら
            if (soundType != SoundDataSO.SoundEffectName.None) {
                // SE 再生
                PlaySE(soundType);
            }

            Destroy(gameObject, duration);
        }

        /// <summary>
        /// 武器の効果
        /// </summary>
        protected virtual void TriggerWeapon() {
            // TODO 子クラスに、各武器の処理を記載する

        }
    }
}