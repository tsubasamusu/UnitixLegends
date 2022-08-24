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
    /// 指定した効果音のデータを取得する
    /// </summary>
    /// <param name="soundEffectName">効果音の名前</param>
    /// <returns>指定した効果音のデータ</returns>
    public SoundDataSO.SoundData GetSoundEffectData(SoundDataSO.SoundEffectName soundEffectName)
    {
        //音のデータのリストから要素を1つずつ取り出す
        foreach (SoundDataSO.SoundData soundData in soundDataSO.soundDataList)
        {
            //取り出した要素の名前が引数と同じだったら
            if(soundData.soundEffectName == soundEffectName)
            {
                //取り出した要素を返す
                return soundData;
            }
        }

        //nullを返す
        return null;
    }

    /// <summary>
    /// 効果音をAudioSourceを使って再生する
    /// </summary>
    /// <param name="soundData">効果音のデータ</param>
    /// <param name="loop">効果音を繰り返すかどうか</param>
    /// <param name="volume">効果音の大きさ</param>
    /// <returns>使用したAudioSorce</returns>
    public AudioSource PlaySoundEffectByAudioSource(SoundDataSO.SoundData soundData, bool loop = false, float volume = 1f)
    {
        //オーディオクリップを設定
        audioSource.clip = soundData.audioClip;

        //効果音を繰り返すかどうかを設定
        audioSource.loop = loop;

        //効果音音の大きさを設定
        audioSource.volume = volume;

        //効果音を再生
        audioSource.Play();

        //AudioSorceを返す
        return audioSource;
    }
}
