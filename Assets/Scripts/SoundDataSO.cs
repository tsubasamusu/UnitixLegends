using System.Collections;
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p

//�A�Z�b�g���j���[�ŁuCreate SoundDataSO�v��I������ƁA�uSoundDataSO�v���쐬�ł���
[CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
    /// <summary>
    /// ���̎��
    /// </summary>
    public enum SoundType
    {
        SoundEffect,//���ʉ�
        BackgroundMusic//BGM
    }

    /// <summary>
    /// ���ʉ��̖��O
    /// </summary>
    public enum SoundEffectName
    {
        None,//�I�����Ȃ�
        KnifeSE,//�i�C�t�Ŏh����
        BatSE,//�o�b�g�ŉ��鉹
        ExplosionSE,//������
        GasSE,//�K�X�̉�
        AssaultSE,//�A�T���g���C�t���̉�
        ShotgunSE,//�V���b�g�K���̉�
        SniperSE,//�X�i�C�p�[���C�t���̉�
        RecoverySE,//�񕜉�
        BulletSE,//�e�̃A�C�e�����E�����Ƃ��̉�
        AirplaneSE,//��s�@�̉�
        FallSE,//��s�@�����э~��鉹
        LandingSE,//���n��
        RunningSE,//���鉹
        BePreparedSE,//�\���鉹
        GetItemSE,//�A�C�e���擾��
        DiscardItemSE,//�A�C�e���j����
        SelectItemSE,//�A�C�e���I����
        GameStartSE,//�Q�[���J�n��
        GameOverSE,//�Q�[���I�[�o�[��
        GameClearSE,//�Q�[���N���A��
        NoneItemSE,//�I�������A�C�e����None�̎��̉�
    }

    /// <summary>
    /// BGM�̖��O
    /// </summary>
    public enum BackgroundMusicName
    {
        None,//�I�����Ȃ�
        Main//���C��
    }

    /// <summary>
    /// ���̃f�[�^���Ǘ�����N���X
    /// </summary>
    [Serializable]
    public class SoundData
    { 
        public SoundType soundType;//���̎��
        public SoundEffectName soundEffectName;//���ʉ��̖��O
        public BackgroundMusicName backgroundMusicName;//BGM�̖��O
        public AudioClip audioClip;//�I�[�f�B�I�N���b�v
    }

    public List<SoundData> soundDataList=new List<SoundData>();//���̃f�[�^�̃��X�g
}
