using System.Collections.Generic;//���X�g���g�p
using UnityEngine;
using System;//Serializable�������g�p

namespace yamap 
{
    /// <summary>
    /// BGM
    /// </summary>
    public enum BgmName 
    {
        Main,
    }

    /// <summary>
    /// ���ʉ��̖��O
    /// </summary>
    public enum SeName 
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
        NextShotOkSE,//���ˊ������i�����[�h���j
        InflictDamageSE,//Player��Enemy�Ƀ_���[�W��^�����Ƃ��̉�
    }

    /// <summary>
    /// ���̃f�[�^���Ǘ�����
    /// </summary>
    [Serializable]
    public class SeData 
    {
        public SeName seName;//���ʉ��̖��O
        public AudioClip audioClip;//�I�[�f�B�I�N���b�v
    }

    /// <summary>
    /// BGM�̃f�[�^���Ǘ�����
    /// </summary>
    [Serializable]
    public class BgmData 
    {
        public BgmName bgmName;//BGM�̖��O
        public AudioClip audioClip;//�I�[�f�B�I�N���b�v
    }

    //�A�Z�b�g���j���[�ŁuCreate SoundDataSO_yamap�v��I������ƁA�uSoundDataSO�v���쐬�ł���
    [CreateAssetMenu(fileName = "SoundDataSO", menuName = "Create SoundDataSO_yamap")]
    public class SoundDataSO : ScriptableObject 
    {
        public List<SeData> seList = new ();//���ʉ��̃f�[�^�̃��X�g
        public List<BgmData> bgmList = new();//BGM�̃f�[�^�̃��X�g
    }
}