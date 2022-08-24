using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private SoundDataSO soundDataSO;//SoundDataSO

    [SerializeField]
    private AudioSource audioSource;//AudioSource

    /// <summary>
    /// �w�肵�����ʉ��̃f�[�^���擾����
    /// </summary>
    /// <param name="soundEffectName">���ʉ��̖��O</param>
    /// <returns>�w�肵�����ʉ��̃f�[�^</returns>
    public SoundDataSO.SoundData GetSoundEffectData(SoundDataSO.SoundEffectName soundEffectName)
    {
        //���̃f�[�^�̃��X�g����v�f��1�����o��
        foreach (SoundDataSO.SoundData soundData in soundDataSO.soundDataList)
        {
            //���o�����v�f�̖��O�������Ɠ�����������
            if(soundData.soundEffectName == soundEffectName)
            {
                //���o�����v�f��Ԃ�
                return soundData;
            }
        }

        //null��Ԃ�
        return null;
    }

    /// <summary>
    /// ���ʉ���AudioSource���g���čĐ�����
    /// </summary>
    /// <param name="soundData">���ʉ��̃f�[�^</param>
    /// <param name="loop">���ʉ����J��Ԃ����ǂ���</param>
    /// <param name="volume">���ʉ��̑傫��</param>
    /// <returns>�g�p����AudioSorce</returns>
    public AudioSource PlaySoundEffectByAudioSource(SoundDataSO.SoundData soundData, bool loop = false, float volume = 1f)
    {
        //�I�[�f�B�I�N���b�v��ݒ�
        audioSource.clip = soundData.audioClip;

        //���ʉ����J��Ԃ����ǂ�����ݒ�
        audioSource.loop = loop;

        //���ʉ����̑傫����ݒ�
        audioSource.volume = volume;

        //���ʉ����Đ�
        audioSource.Play();

        //AudioSorce��Ԃ�
        return audioSource;
    }
}
