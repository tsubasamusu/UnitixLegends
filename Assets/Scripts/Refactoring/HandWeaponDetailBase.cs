namespace yamap 
{
    /// <summary>
    /// �ߐڕ���̃v���t�@�u�ɃA�^�b�`����A�ߐڕ���̐e�N���X
    /// </summary>
    public class HandWeaponDetailBase : WeaponBase 
    {    
        /// <summary>
        /// WeaponDetail�̏����ݒ���s��
        /// </summary>
        /// <param name="attackPower">�U����</param>
        /// <param name="bulletOwnerType">�e�̏��L��</param>
        /// <param name="soundType">���̎��</param>
        /// <param name="duration">�Q�[���I�u�W�F�N�g�������܂ł̎���</param>
        public virtual void SetUpWeaponDetail(float attackPower, BulletOwnerType bulletOwnerType, SeName soundType, float duration) 
        {
            //�U���͂��擾
            this.attackPower = attackPower;
            
            //�e�̏��L�҂��擾
            BulletOwnerType = bulletOwnerType;

            //�U�����s��
            TriggerWeapon();

            //���ʉ�������Ȃ�inull�G���[����j
            if (soundType != SeName.None) 
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySE(soundType);
            }

            //��莞�Ԍo�ߌ�ɃQ�[���I�u�W�F�N�g������
            Destroy(gameObject, duration);
        }

        /// <summary>
        /// ����̌���
        /// </summary>
        protected virtual void TriggerWeapon() 
        {
            //�q�N���X�ɁA�e����̏������L�ڂ���
        }
    }
}