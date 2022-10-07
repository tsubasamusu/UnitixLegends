using UnityEngine;

namespace yamap 
{
    /// <summary>
    /// ����̏��L�҂̎��
    /// </summary>
    public enum BulletOwnerType 
    {
        Player,//�v���C���[
        Enemy//�G�l�~�[
    }

    /// <summary>
    /// �e�Ƌߐڕ���̗����ŋ��ʂ��鏈�����L�ڂ��Ă���A����S�̂̐e�N���X
    /// </summary>
    public class WeaponBase : MonoBehaviour 
    {
        protected float attackPower;//�U����

        protected Rigidbody rb;//Rigidbody

        protected BulletOwnerType bulletOwnerType;//�e�̏��L��

        /// <summary>
        /// �e�̏��L�҂̎擾�E�ݒ�p
        /// </summary>
        public BulletOwnerType BulletOwnerType { get => bulletOwnerType; set => bulletOwnerType = value; }

        /// <summary>
        /// ���Z�b�g����
        /// </summary>
        protected void Reset() 
        {
            //Rigidbody�̎擾�Ɏ��s������
            if (!TryGetComponent(out rb))
            {
                //�����
                Debug.Log("Rigidbody ���擾�o���܂���B");
            }    
        }
    }
}