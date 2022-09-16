using UnityEngine;

namespace yamap {

    /// <summary>
    /// ����̏��L�҂̎��
    /// </summary>
    public enum BulletOwnerType {
        Player,
        Enemy
    }

    /// <summary>
    /// �e�Ƌߐڕ���̗����ŋ��ʂ��鏈�����L�ڂ��Ă���A����S�̂̐e�N���X
    /// </summary>
    public class WeaponBase : MonoBehaviour {

        protected float attackPower;
        protected Rigidbody rb;

        // �e�̏��L��
        protected BulletOwnerType bulletOwnerType;

        /// <summary>
        /// �e�̏��L�҂̃v���p�e�B
        /// </summary>
        public BulletOwnerType BulletOwnerType { get => bulletOwnerType; set => bulletOwnerType = value; }

        protected void Reset() {
            if (!TryGetComponent(out rb)) {
                Debug.Log("Rigidbody ���擾�o���܂���B");
            }    
        }
    }
}