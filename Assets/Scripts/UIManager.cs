using System.Collections;
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

    /// <summary>
    /// �Q�[���X�^�[�g���o���s��
    /// </summary>
    public IEnumerator SetGameStart()
    {
        //���E�𔒐F�ɐݒ�
        eventHorizon.color = new Color(255.0f, 255.0f, 255.0f);

        //���S���Q�[���X�^�[�g�ɐݒ�
        logo.sprite = gameStart;

        //2.0�b�����ă��S��\��
        logo.DOFade(1.0f,12.0f);

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
}
