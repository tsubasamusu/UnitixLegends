namespace yamap {

    /// <summary>
    /// �ߐڕ���̃v���t�@�u�ɃA�^�b�`����A�ߐڕ���̐e�N���X
    /// </summary>
    public class HandWeaponDetailBase : WeaponBase {
        
        public virtual void SetUpWeaponDetail(float attackPower, BulletOwnerType bulletOwnerType, SoundDataSO.SoundEffectName soundType, float duration) {
            this.attackPower = attackPower;
            BulletOwnerType = bulletOwnerType;

            // ����̍U��
            TriggerWeapon();

            // SE ������Ȃ�
            if (soundType != SoundDataSO.SoundEffectName.None) {
                // SE �Đ�
                PlaySE(soundType);
            }

            Destroy(gameObject, duration);
        }

        /// <summary>
        /// ����̌���
        /// </summary>
        protected virtual void TriggerWeapon() {
            // TODO �q�N���X�ɁA�e����̏������L�ڂ���

        }
    }
}