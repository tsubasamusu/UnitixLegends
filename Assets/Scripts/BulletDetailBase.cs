using UnityEngine;

namespace yamap {

    /// <summary>
    /// �o���b�g�̃v���t�@�u�ɃA�^�b�`����A�e�p�̐e�N���X
    /// </summary>
    public class BulletDetailBase : WeaponBase {

        protected float effectDuration;

        /// <summary>
        /// �e�̐ݒ�
        /// </summary>
        /// <param name="attackPower">�e�̈З�</param>
        /// <param name="bulletOwnerType">�e�̏��L��</param>
        /// <param name="direction">�e�̔�ԕ���</param>
        /// <param name="seName">SE �̎��</param>
        /// <param name="duration">�e�̎�������</param>
        /// <param name="effectPrefab">�G�t�F�N�g�̃v���t�@�u</param>
        public virtual void SetUpBulletDetail(float attackPower, BulletOwnerType bulletOwnerType, Vector3 direction, SoundDataSO.SoundEffectName seName, float duration = 3.0f, GameObject effectPrefab = null) {
            this.attackPower = attackPower;
            BulletOwnerType = bulletOwnerType;

            // �e�̔���
            TriggerBullet(direction, duration);

            // SE ������Ȃ�
            if (seName != SoundDataSO.SoundEffectName.None) {
                // SE �Đ�
                PlaySE(seName);
            }

            // �G�t�F�N�g������Ȃ�
            if (effectPrefab != null) {
                // �G�t�F�N�g����
                GenerateEffect(effectPrefab);
            }
        }

        /// <summary>
        /// �e�̔���
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="soundType"></param>
        /// <param name="duration"></param>
        protected virtual void TriggerBullet(Vector3 direction, float duration) {
            rb.AddForce(direction);

            Destroy(gameObject, duration);
        }

        /// <summary>
        /// �G�t�F�N�g����
        /// </summary>
        /// <param name="effect"></param>
        protected virtual void GenerateEffect(GameObject effectPrefab) {
            //�G�t�F�N�g�𐶐����A�e��ShoBullet�ɐݒ�
            GameObject effect = Instantiate(effectPrefab, transform);

            //���������G�t�F�N�g���w�莞�Ԍ�ɏ���
            Destroy(effect, effectDuration);
        }

        public float GetAttackPower() {
            return attackPower;
        }
    }
}