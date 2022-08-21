using System.Collections;
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform enemyPrefab;//Enemy�̃v���t�@�u

    [SerializeField]
    private int maxGenerateCount;//Enemy�̍ő吶����

    [SerializeField]
    private float flightTime;//��s�@�̔�s����

    [SerializeField]
    private Transform enemiesTran;//Enemy�̐e�I�u�W�F�N�g

    [HideInInspector]
    public List<Transform> generatedEnemyTranList=new List<Transform>();//��������Enemy�̈ʒu���̃��X�g

    private List<float> generateTimeList=new List<float>();//Enemy�𐶐����鎞�Ԃ̃��X�g

    private float timer;//�o�ߎ���

    private int generateCount;//Enemy�̐�����

    /// <summary>
    /// �Q�[���J�n����ɌĂяo�����
    /// </summary>
    private void Start()
    {
        //Enemy�̍ő吶���������J��Ԃ�
        for(int i = 0; i < maxGenerateCount; i++)
        {
            //Enemy�𐶐����鎞�Ԃ̃��X�g������������
            generateTimeList.Add(Random.Range(1f,flightTime));
        }
    }

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    private void Update()
    {
        //TODO:��s�@�������n�߂���ȍ~�̏������s��

        //�S�Ă�Enemy�𐶐����I�������
        if(generateCount==maxGenerateCount)
        {
            //�ȍ~�̏������s��Ȃ�
            return;
        }

        //�o�ߎ��Ԃ��v��
        timer+=Time.deltaTime;

        //Enemy�𐶐����鎞�Ԃ̃��X�g�̗v�f�������J��Ԃ�
        for (int i = 0; i < generateTimeList.Count; i++)
        {
            //�o�ߎ��Ԃ��������Ԉȏ�ɂȂ�����
            if (timer >= generateTimeList[i])
            {
                //Enemy�𐶐����A�e��ݒ�
                Transform enemyTran = Instantiate(enemyPrefab, enemiesTran);

                //��������Enemy�̏ꏊ�𒲐�
                enemyTran.position = transform.position;

                //��������Enemy�̑傫���𒲐�
                enemyTran.localScale = new Vector3(2f, 2f, 2f);

                //��������Enemy�̈ʒu�������X�g�ɉ�����
                generatedEnemyTranList.Add(enemyTran);

                //��������1��������
                generateCount++;

                //Enemy�𐶐����鎞�Ԃ̃��X�g���炻�̗v�f��r��
                generateTimeList.RemoveAt(i);
            }
        }
    }
}
