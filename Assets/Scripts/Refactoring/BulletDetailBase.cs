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
        public virtual void SetUpBulletDetail(float attackPower, BulletOwnerType bulletOwnerType, Vector3 direction, SeName seName, float duration = 3.0f, GameObject effectPrefab = null) {
            Reset();

            this.attackPower = attackPower;
            BulletOwnerType = bulletOwnerType;

            // �e�̔���
            TriggerBullet(direction, duration);


            // SE ������Ȃ�
            if (seName != SeName.None) {
                // SE �Đ�
                SoundManager.instance.PlaySE(seName);
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

        /// <summary>
        /// �ǉ�����
        /// </summary>
        public virtual void AddTriggerBullet(EnemyController enemyController) {
            Debug.Log("�ǉ����ʂ��q�N���X�ŋL�q����");
        }
    }
}