using DG.Tweening;

namespace yamap 
{
    public class HandWeapon_Knife : HandWeaponDetailBase
    {
        /// <summary>
        /// �i�C�t�̌��ʏ���
        /// </summary>
        protected override void TriggerWeapon() 
        {
            //�A�j���[�V�������J�n����i�O��ړ��j
            transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }
    }
}