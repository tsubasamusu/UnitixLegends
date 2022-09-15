using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundDataSO;

namespace yamap {

    public class SoundManager : MonoBehaviour {

        public static SoundManager instance;

        [SerializeField]
        private SoundDataSO soundDataSO;//SoundDataSO

        [SerializeField]
        private AudioSource seSource;//AudioSource

        [SerializeField]
        private AudioSource bgmSource;


        void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// ���ʉ���AudioSource���g���čĐ�����
        /// </summary>
        /// <param name="soundData">���ʉ��̃f�[�^</param>
        /// <param name="loop">���ʉ����J��Ԃ����ǂ���</param>
        /// <param name="volume">���ʉ��̑傫��</param>
        /// <returns>�g�p����AudioSource</returns>
        public void PlaySE(SeName soundEffectName, float volume = 1f) {

            SeData seData = GetSoundEffectData(soundEffectName);

            //���ʉ����̑傫����ݒ�
            seSource.volume = volume;

            //���ʉ����Đ�
            seSource.PlayOneShot(seData.audioClip);


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
            seSource.clip = null;
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
            seSource.loop = isLoop;

            //���ʉ����̑傫����ݒ�
            seSource.volume = volume;
            
            //�I�[�f�B�I�N���b�v��ݒ�
            seSource.clip = bgmData.audioClip;

            bgmSource.Play();            
        }
    }
}