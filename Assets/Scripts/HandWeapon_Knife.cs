using DG.Tweening;

namespace yamap {

    public class HandWeapon_Knife : HandWeaponDetailBase
    {
        /// <summary>
        /// �i�C�t�̌��ʏ���
        /// </summary>
        protected override void TriggerWeapon() {
            // ���[�v�����ɂ́A�J��Ԃ��񐔂����܂��Ă��Ă��A��{�I�ɂ�SetLink �����āA�������P�A����
            // ���̏����̍Đ����Ƀv���C���[�̃Q�[���I�u�W�F�N�g���j�����ꂽ��A�V�[���J�ڂ���ƃG���[(�x��)�ɂȂ邽��
            transform.DOLocalMoveZ(2f, 0.5f).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }
    }
}