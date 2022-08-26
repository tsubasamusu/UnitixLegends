using System.Collections;
using System.Collections.Generic;
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

        [SerializeField]
        protected Rigidbody rb;

        // �e�̏��L��
        protected BulletOwnerType bulletOwnerType;

        /// <summary>
        /// �e�̏��L�҂̃v���p�e�B
        /// </summary>
        public BulletOwnerType BulletOwnerType { get => bulletOwnerType; set => bulletOwnerType = value; }


        /// <summary>
        /// SE �Đ�
        /// </summary>
        /// <param name="seName"></param>
        /// <param name=""></param>
        protected virtual void PlaySE(SoundDataSO.SoundEffectName seName) {
            AudioSource.PlayClipAtPoint(SoundManager.instance.GetSoundEffectData(seName).audioClip, transform.position);
        }
    }
}