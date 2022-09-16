using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundDataSO;

namespace yamap {

    public class SoundManager : MonoBehaviour {

        public static SoundManager instance;

        [SerializeField]
        private SoundDataSO soundDataSO;//SoundDataSO

        private AudioSource[] seSources;//AudioSource

        [SerializeField]
        private AudioSource bgmSource;
        private int seAudioCount = 16;


        void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }

            seSources = new AudioSource[seAudioCount];
            for (int i = 0; i < seAudioCount; i++) {
                seSources[i] = gameObject.AddComponent<AudioSource>();
            }
        }

        /// <summary>
        /// ���ʉ���AudioSource���g���čĐ�����
        /// </summary>
        /// <param name="soundData">���ʉ��̃f�[�^</param>
        /// <param name="loop">���ʉ����J��Ԃ����ǂ���</param>
        /// <param name="volume">���ʉ��̑傫��</param>
        /// <returns>�g�p����AudioSource</returns>
        public void PlaySE(SeName soundEffectName, bool isLoop = false, float volume = 1f) {

            SeData seData = GetSoundEffectData(soundEffectName);

            foreach (var audioSource in seSources) {
                if (audioSource.isPlaying) {
                    continue;
                } else {
                    //���ʉ����J��Ԃ����ǂ�����ݒ�
                    audioSource.loop = isLoop;

                    //���ʉ����̑傫����ݒ�
                    audioSource.volume = volume;

                    //���ʉ����Đ�
                    audioSource.PlayOneShot(seData.audioClip);
                    break;
                }
            }
            Debug.Log(seData.seName);

            /// <summary>
            /// �w�肵�����ʉ��̃f�[�^���擾����
            /// </summary>
            /// <param name="searchSeName">���ʉ��̖��O</param>
            /// <returns>�w�肵�����ʉ��̃f�[�^</returns>
            SeData GetSoundEffectData(SeName searchSeName) {
                return soundDataSO.seList.Find(x => x.seName == searchSeName);
            }
        }

        /// <summary>
        /// AudioSource����̏�Ԃɂ���
        /// </summary>
        public void ClearAudioSource() {
            //clip��null������
            for (int i = 0; i < seSources.Length; i++) {
                if (seSources[i].isPlaying) {
                    seSources[i].clip = null;
                }
            }      
        }

        /// <summary>
        /// BGM �Đ�
        /// </summary>
        /// <param name="bgmName"></param>
        /// <param name="isLoop"></param>
        /// <param name="volume"></param>
        public void PlayBGM(BgmName bgmName, bool isLoop, float volume = 1f) {

            BgmData bgmData = GetBgmData(bgmName);


            /// <summary>
            /// BGM �擾(���[�J���֐��́A���\�b�h���ł���Ώ����ꏊ�͔C��)
            /// </summary>
            /// <param name="searchbgmName"></param>
            /// <returns></returns>
            BgmData GetBgmData(BgmName searchbgmName) {
                return soundDataSO.bgmList.Find(x => x.bgmName == searchbgmName);
            }


            //���ʉ����J��Ԃ����ǂ�����ݒ�
            bgmSource.loop = isLoop;

            //���ʉ����̑傫����ݒ�
            bgmSource.volume = volume;

            //�I�[�f�B�I�N���b�v��ݒ�
            bgmSource.clip = bgmData.audioClip;

            bgmSource.Play();            
        }
    }
}