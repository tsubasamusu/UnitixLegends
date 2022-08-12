using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//�C���X�^���X

    /// <summary>
    /// Start���\�b�h���O�ɌĂяo�����
    /// </summary>
    private void Awake()
    {
        //�ȉ��A�V���O���g���ɕK�{�̋L�q
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //TODO:�I�����Ă��镐��̏����擾���鏈��
}
