using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//UI���g�p
using DG.Tweening;//DOTween���g�p

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image eventHorizon;//���E

    [SerializeField]
    private Image logo;//���S

    [SerializeField]
    private Sprite gameStart;//�Q�[���X�^�[�g���S

    [SerializeField]
    private Sprite gameClear;//�Q�[���N���A���S

    [SerializeField]
    private Slider hpSlider;//�̗̓X���C�_�[

    /// <summary>
    /// �Q�[���X�^�[�g���o���s��
    /// </summary>
    public IEnumerator SetGameStart()
    {
        //���E�𔒐F�ɐݒ�
        eventHorizon.color = new Color(255.0f, 255.0f, 255.0f,0.0f);

        //���S���Q�[���X�^�[�g�ɐݒ�
        logo.sprite = gameStart;

        //2.0�b�����ă��S��\��
        logo.DOFade(1.0f,2.0f);

        //���S�̕\�����I���܂ő҂�
        yield return new WaitForSeconds(2.0f);

        //���E��1.0�b�|���ē����ɂ���
        eventHorizon.DOFade(0.0f, 1.0f);

        //���S��1.0�b�����ď���
        logo.DOFade(0.0f, 1.0f);

        //���E�ƃ��S�̉��o���I���܂ő҂�
        yield return new WaitForSeconds(1.0f);

        //GameManager����Q�[���J�n��Ԃɐ؂�ւ���
    }

    /// <summary>
    /// ���E���w�肳�ꂽ���Ԃ����Â�����
    /// </summary>
    public IEnumerator SetEventHorizonBlack(float time)
    {
        //���E�����F�ɐݒ�
        eventHorizon.color = new Color(0.0f, 0.0f, 0.0f,0.0f);

        //1.0�b�����Ď��E�����S�ɈÂ�����
        eventHorizon.DOFade(1.0f, 1.0f);

        //�����Ŏw�肳�ꂽ���Ԃ������E���Â��܂ܕۂ�
        yield return new WaitForSeconds(time);

        //0.5�b�����Ď��E�����ɖ߂�
        eventHorizon.DOFade(0.0f, 0.5f);
    }
}
