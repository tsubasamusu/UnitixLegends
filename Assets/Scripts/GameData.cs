using System.Collections;
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//�C���X�^���X

    [SerializeField]
    private GameObject itemTrans;//�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[

    public List<Transform> generateItemTranList = new List<Transform>();//�A�C�e���̐����ʒu�̃��X�g

    /// <summary>
    /// Start���\�b�h���O�ɌĂяo�����i�ȉ��A�V���O���g���ɕK�{�̋L�q�j
    /// </summary>
    private void Awake()
    {
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

    /// <summary>
    /// �A�C�e���𐶐�����
    /// </summary>
    public void GenerateItem()
    {

    }
}
