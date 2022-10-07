using UnityEngine;
using DG.Tweening;

namespace yamap 
{
    public class HandWeapon_Bat : HandWeaponDetailBase 
    {
        /// <summary>
        /// �o�b�g�̌��ʂ̏���
        /// </summary>
        protected override void TriggerWeapon() 
        {
            //�o�b�g�̃A�j���[�V�������J�n�i�O��ړ��j
            transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);

            //�o�b�g�̃A�j���[�V�������J�n�i��]�j
            transform.DOLocalRotate(new Vector3(60f, 0f, 0f), 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }
    }
}