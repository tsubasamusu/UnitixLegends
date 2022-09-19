using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class StormController : MonoBehaviour
{
    [SerializeField]
    private float timeLimit;//��������

    private Vector3 firstStormScale;//�X�g�[���̑傫���̏����l

    private float currentScaleRate=100f;//���݂̃X�g�[���̑傫���̊���(%)

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //�X�g�[���̑傫���̏����l��ݒ�
        firstStormScale = transform.localScale;

        //�X�g�[���̏k�����J�n����
        MakeStormSmaller();
    }

    /// <summary>
    /// �X�g�[���̏k�����J�n����
    /// </summary>
    private void MakeStormSmaller()
    {
        //�������ԓ��ɓ����Łu�X�g�[���̑傫���̊����v��100%����0%�ɂ���
        DOTween.To(() => currentScaleRate,(x) => currentScaleRate = x,0f,timeLimit).SetEase(Ease.Linear);
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //�����ɉ����ăX�g�[�����k��������
        transform.localScale = new Vector3((firstStormScale.x*(currentScaleRate/100f)),firstStormScale.y,(firstStormScale.z*(currentScaleRate/100f)));
    }

    /// <summary>
    /// ���g�����u���ɋ��邩�ǂ������ׂ�
    /// </summary>
    /// <param name="myPos">���g�̍��W</param>
    /// <returns>���g�����u���ɂ�����true</returns>
    public bool CheckEnshrine(Vector3 myPos)
    {
        //���g�̍��W��x-z���ʏ�ŕ\��
        Vector3 pos = Vector3.Scale(myPos, new Vector3(1f, 0f, 1f));

        //�X�g�[���̒����̍��W��(0,0,0)�ɐݒ�
        Vector3 centerPos = Vector3.zero;

        //�X�g�[���̒����܂ł̋����ix-z���ʏ�j���擾
        float length =(pos - centerPos).magnitude;

        //���g�����u���ɂ�����true�A���u�O�ɂ���Ȃ�false��Ԃ�
        return length <=transform.localScale.x/2f?true:false;
    }
}
