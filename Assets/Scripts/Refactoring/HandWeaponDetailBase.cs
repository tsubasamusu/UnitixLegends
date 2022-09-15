namespace yamap {

    /// <summary>
    /// �ߐڕ���̃v���t�@�u�ɃA�^�b�`����A�ߐڕ���̐e�N���X
    /// </summary>
    public class HandWeaponDetailBase : WeaponBase {
        
        public virtual void SetUpWeaponDetail(float attackPower, BulletOwnerType bulletOwnerType, SeName soundType, float duration) {
            this.attackPower = attackPower;
            BulletOwnerType = bulletOwnerType;

            // ����̍U��
            TriggerWeapon();

            // SE ������Ȃ�
            if (soundType != SeName.None) {
                // SE �Đ�
                SoundManager.instance.PlaySE(soundType);
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