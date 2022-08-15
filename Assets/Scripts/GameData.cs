using System.Collections;
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;//�C���X�^���X

    [SerializeField]
    private Transform itemTrans;//�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[

    [SerializeField]
    private ItemDataSO ItemDataSO;//ItemDataSO

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
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        GenerateItem();
    }

    /// <summary>
    /// �A�C�e���𐶐�����
    /// </summary>
    public void GenerateItem()
    {
        //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̐������J��Ԃ�
        for (int i = 0; i < itemTrans.childCount; i++)
        {
            Instantiate(ItemDataSO.itemDataList[Random.Range(0, 13)].prefab, CreateItemPosList()[i]);
        }
    }

    /// <summary>
    /// �A�C�e���̐����ʒu�̃��X�g���쐬����
    /// </summary>
    private List<Transform> CreateItemPosList()
    {
        //���X�g���쐬
        List<Transform> generateItemPosList = new List<Transform>();

        //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̐������J��Ԃ�
        for (int i = 0; i < itemTrans.childCount; i++)
        {
            //�A�C�e���̈ʒu�����܂Ƃ߂��t�H���_�[�̎q�I�u�W�F�N�g�̈ʒu�������X�g�ɒǉ����Ă���
            generateItemPosList.Add(itemTrans.GetChild(i).transform);
        }

        //�����������X�g��Ԃ�
        return generateItemPosList;
    }
}
