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
        /// 効果音をAudioSourceを使って再生する
        /// </summary>
        /// <param name="soundData">効果音のデータ</param>
        /// <param name="loop">効果音を繰り返すかどうか</param>
        /// <param name="volume">効果音の大きさ</param>
        /// <returns>使用したAudioSource</returns>
        public void PlaySE(SeName soundEffectName, bool isLoop = false, float volume = 1f) {

            SeData seData = GetSoundEffectData(soundEffectName);

            foreach (var audioSource in seSources) {
                if (audioSource.isPlaying) {
                    continue;
                } else {
                    //効果音を繰り返すかどうかを設定
                    audioSource.loop = isLoop;

                    //効果音音の大きさを設定
                    audioSource.volume = volume;

                    //効果音を再生
                    audioSource.PlayOneShot(seData.audioClip);
                    break;
                }
            }
            Debug.Log(seData.seName);

            /// <summary>
            /// 指定した効果音のデータを取得する
            /// </summary>
            /// <param name="searchSeName">効果音の名前</param>
            /// <returns>指定した効果音のデータ</returns>
            SeData GetSoundEffectData(SeName searchSeName) {
                return soundDataSO.seList.Find(x => x.seName == searchSeName);
            }
        }

        /// <summary>
        /// AudioSourceを空の状態にする
        /// </summary>
        public void ClearAudioSource() {
            //clipにnullを入れる
            for (int i = 0; i < seSources.Length; i++) {
                if (seSources[i].isPlaying) {
                    seSources[i].clip = null;
                }
            }      
        }

        /// <summary>
        /// BGM 再生
        /// </summary>
        /// <param name="bgmName"></param>
        /// <param name="isLoop"></param>
        /// <param name="volume"></param>
        public void PlayBGM(BgmName bgmName, bool isLoop, float volume = 1f) {

            BgmData bgmData = GetBgmData(bgmName);


            /// <summary>
            /// BGM 取得(ローカル関数は、メソッド内であれば書く場所は任意)
            /// </summary>
            /// <param name="searchbgmName"></param>
            /// <returns></returns>
            BgmData GetBgmData(BgmName searchbgmName) {
                return soundDataSO.bgmList.Find(x => x.bgmName == searchbgmName);
            }


            //効果音を繰り返すかどうかを設定
            bgmSource.loop = isLoop;

            //効果音音の大きさを設定
            bgmSource.volume = volume;

            //オーディオクリップを設定
            bgmSource.clip = bgmData.audioClip;

            bgmSource.Play();            
        }
    }
}