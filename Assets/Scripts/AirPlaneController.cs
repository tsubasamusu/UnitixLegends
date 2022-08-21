using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    private Transform propellerTran;//�v���y���̈ʒu���

    [SerializeField]
    private Transform playerTran;//Player�̈ʒu

    [SerializeField]
    private Transform aiplanePlayerTran;//��s�@�ł�Player�̈ʒu

    [SerializeField]
    private PlayerController playerController;//PlayerController

    [SerializeField]
    private CinemachineManager cinemachineManager;//CinemachineManager

    [SerializeField]
    private UIManager uiManager;//UIManager

    [SerializeField]
    private KeyCode fallKey;//��s�@�����э~���L�[

    private bool fellFromAirplane;//��s�@���痎���������ǂ���

    [SerializeField]
    private float rotSpeed;//�v���y���̉�]���x

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //PlayerController�𖳌���
        playerController.enabled = false;

        //��s�@�������ʒu�ɔz�u
        transform.position = new Vector3(120f, 100f, -120f);

        //�v���y���̉�]���J�n
        StartCoroutine(RotatePropeller());

        //��s�@�̑��c���J�n
        StartCoroutine(NavigateAirplane());

        //���b�Z�[�W��\��
        uiManager.SetMessageText("Tap\n'Space'\nTo Fall",Color.blue);
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //���ɔ�s�@�����э~�肽�̂Ȃ�
        if (fellFromAirplane)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //Player����ɔ�s�@�̐^���ɐݒu
        playerTran.position=aiplanePlayerTran.position;

        //��s�@�����э~���L�[�������ꂽ��
        if (Input.GetKeyDown(fallKey))
        {
            //���b�Z�[�W�̃e�L�X�g����ɂ���
            uiManager.SetMessageText("", Color.black);

            //PlayerController��L����
            playerController.enabled=true;

            //Player�J�����ɐ؂�ւ�
            cinemachineManager.SetAirplaneCameraPriority(9);

            //��s�@�����э~�肽��ԂɕύX
            fellFromAirplane = true;
        }
    }

    /// <summary>
    ///��s�@�̃v���y������]������
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator RotatePropeller()
    {
        //�������[�v
        while (true)
        {
            //�v���y������
            propellerTran.Rotate(0f, rotSpeed, 0f);

            //��莞�ԑ҂i�����AFixedUpdate���\�b�h�j
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// ��s�@�𑀏c����
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator NavigateAirplane()
    {
        //4��J��Ԃ�
        for (int i = 0; i < 4; i++)
        {
            //�ڕW�l
            Vector3 pos = Vector3.zero;

            //�J��Ԃ��񐔂ɉ����ĖړI�n��ύX
            switch (i)
            {
                case 0:
                    pos = new Vector3(120f, 100f, 120f);
                    break;
                case 1:
                    pos = new Vector3(-120f, 100f, 120f);
                    break;
                case 2:
                    pos = new Vector3(-120f, 100f, -120f);
                    break;
                case 3:
                    pos = new Vector3(120f, 100f, -120f);
                    break;
            }

            //10�b�����đO�i
            transform.DOMove(pos, 10f).SetEase(Ease.Linear);

            //10�b�҂�
            yield return new WaitForSeconds(10f);

            //4��ڂ̌J��Ԃ������Ȃ�
            if(i==3)
            {
                //�J��Ԃ��������I��
                break;
            }

            //1�b�����Đ���
            transform.DORotate(new Vector3(0f, (float)-90*(i+1), 0f), 1f).SetEase(Ease.Linear);

            //1�b�҂�
            yield return new WaitForSeconds(1f);
        }

        //�X�e�[�W�O�֔��ł���
        transform.DOMoveX(transform.position.x+100f, 10f);

        //10�b�҂�
        yield return new WaitForSeconds(10f);

        //��s�@������
        Destroy(gameObject);
    }
}
