using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// �e�p�̐e�N���X�i�o���b�g�̃v���t�@�u�ɃA�^�b�`����j
    /// </summary>
    public class BulletDetailBase : WeaponBase 
    {
        protected float effectDuration;//�G�t�F�N�g�̕���

        /// <summary>
        /// �e�̐ݒ�
        /// </summary>
        /// <param name="attackPower">�e�̈З�</param>
        /// <param name="bulletOwnerType">�e�̏��L��</param>
        /// <param name="direction">�e�̔�ԕ���</param>
        /// <param name="seName">SE �̎��</param>
        /// <param name="duration">�e�̎�������</param>
        /// <param name="effectPrefab">�G�t�F�N�g�̃v���t�@�u</param>
        public virtual void SetUpBulletDetail(float attackPower, BulletOwnerType bulletOwnerType, Vector3 direction, SeName seName, float duration = 3.0f, GameObject effectPrefab = null) 
        {
            //����l�Ƀ��Z�b�g����
            Reset();

            //�e�̈З͂��擾
            this.attackPower = attackPower;

            //�e�̏��L�҂��擾
            BulletOwnerType = bulletOwnerType;

            //�e�𔭎˂���
            TriggerBullet(direction, duration);

            //���ʉ������ɂ���Ȃ�
            if (seName != SeName.None) 
            {
                //���ʉ����Đ�
                SoundManager.instance.PlaySE(seName);
            }

            //�G�t�F�N�g�����ɂ���Ȃ�
            if (effectPrefab != null) 
            {
                //�G�t�F�N�g����
                GenerateEffect(effectPrefab);
            }
        }

        /// <summary>
        /// �e�𔭎˂���
        /// </summary>
        /// <param name="direction">�e�������܂ł̎���</param>
        /// <param name="soundType">���̎��</param>
        protected virtual void TriggerBullet(Vector3 direction, float duration) 
        {
            //�͂�������
            rb.AddForce(direction);

            //�e������
            Destroy(gameObject, duration);
        }

        /// <summary>
        /// �G�t�F�N�g�𐶐�����
        /// </summary>
        /// <param name="effect">�G�t�F�N�g</param>
        protected virtual void GenerateEffect(GameObject effectPrefab) 
        {
            //�G�t�F�N�g�𐶐����A�e��ShoBullet�ɐݒ肷��
            GameObject effect = Instantiate(effectPrefab, transform);

            //���������G�t�F�N�g���w�莞�Ԍ�ɏ���
            Destroy(effect, effectDuration);
        }

        /// <summary>
        /// �U���͂��擾����
        /// </summary>
        /// <returns>�U����</returns>
        public float GetAttackPower() 
        {
            //�U���͂�Ԃ�
            return attackPower;
        }

        /// <summary>
        /// �ǉ�����
        /// </summary>
        public virtual void AddTriggerBullet(EnemyController enemyController) 
        {
            //�ǉ����ʂ��q�N���X�ŋL�q����
        }
    }
}