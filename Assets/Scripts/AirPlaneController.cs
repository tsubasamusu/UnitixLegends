using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;//DOTween���g�p

public class AirPlaneController : MonoBehaviour
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
    private EnemyGenerator enemyGenerator;//EnemyGenerator

    [SerializeField]
    private SoundManager soundManager;//SoundManager

    [SerializeField]
    private KeyCode fallKey;//��s�@�����э~���L�[

    [SerializeField]
    private float rotSpeed;//�v���y���̉�]���x

    private bool fellFromAirplane;//��s�@���痎���������ǂ���

    private bool endFight;//��s�@�̔�s���I��������ǂ���

    private AudioSource audioSource;//AudioSource

    /// <summary>
    /// ��s�@�Ɋւ���ݒ���s��
    /// </summary>
    public void SetUpAirplane()
    {
        //��s�@�������ʒu�ɔz�u
        transform.position = new Vector3(120f, 100f, -120f);

        //�v���y���̉�]���J�n
        StartCoroutine(RotatePropeller());

        //��s�@�̑��c���J�n
        StartCoroutine(NavigateAirplane());

        //���b�Z�[�W��\��
        uiManager.SetMessageText("Tap\n'Space'\nTo Fall", Color.blue);

        //Player�̃L�����N�^�[�𖳌���
        cinemachineManager.SetPlayerCharacterActive(false);

        //��s�@�̉����Đ�
        audioSource=soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.AirplaneSE),true);
    }

    /// <summary>
    /// Player�̍s���𐧌䂷��
    /// </summary>
    /// <returns>�҂�����</returns>
    public IEnumerator ControlPlayerMovement()
    {
        //�܂���s�@�����э~��Ă��Ȃ��Ȃ�J��Ԃ�
        while(!fellFromAirplane)
        {
            //Player����ɔ�s�@�̐^���ɐݒu
            playerTran.position = aiplanePlayerTran.position;

            //��s���I������
            if (endFight)
            {
                //��s�@�����э~���
                StartCoroutine( FallFromAirplane());
            }

            //��s�@�����э~���L�[�������ꂽ��
            if (Input.GetKeyDown(fallKey))
            {
                //��s�@�����э~���
                StartCoroutine( FallFromAirplane());
            }

            //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
            yield return null;
        }
    }

    /// <summary>
    /// ��s�@�����э~���
    /// </summary>
    /// <returns>�҂�����</returns>
    private IEnumerator FallFromAirplane()
    {
        //��s�@�̉����t�F�[�h�A�E�g������
        audioSource.DOFade(0f, 5f);

        //��s�@�����э~��鉹���Đ�
        soundManager.PlaySoundEffectByAudioSource(soundManager.GetSoundEffectData(SoundDataSO.SoundEffectName.FallSE));

        //���b�Z�[�W�̃e�L�X�g����ɂ���
        uiManager.SetMessageText("", Color.black);

        //PlayerController��L����
        playerController.enabled = true;

        //Player�J�����ɐ؂�ւ�
        cinemachineManager.SetAirplaneCameraPriority(9);

        //CanvasGroup��\��
        uiManager.SetCanvasGroup(true);

        //�e�L�X�g�̕\���̍X�V���J�n
        StartCoroutine(uiManager.UpdateText());

        //��s�@�����э~�肽��ԂɕύX
        fellFromAirplane = true;

        //5�b�҂�
        yield return new WaitForSeconds(5f);

        //AudioSource����ɂ���
        soundManager.ClearAudioSource();
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
            //�ړI�n��ݒ�
            Vector3 pos = Vector3.Scale(transform.forward, new Vector3(240f, 0f, 240f))+transform.position;

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

        //��s���I�������Ԃɐ؂�ւ���
        endFight = true;

        //�X�e�[�W�O�֔��ł���
        transform.DOMoveX(transform.position.x+100f, 10f);

        //10�b�҂�
        yield return new WaitForSeconds(10f);

        //��s�@������
        Destroy(gameObject);
    }
}
