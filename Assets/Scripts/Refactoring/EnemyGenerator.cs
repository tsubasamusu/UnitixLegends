using System.Collections;//IEnumerator���g�p
using System.Collections.Generic;//���X�g���g�p
using UnityEngine;

namespace yamap {

    public class EnemyGenerator : MonoBehaviour {

        [SerializeField]
        private PlayerController player;

        [SerializeField]
        private UIManager uIManager;//UIManager

        //[SerializeField]
        //private Transform enemyPrefab;//Enemy�̃v���t�@�u

        [SerializeField]
        private EnemyController enemyPrefab;//Enemy�̃v���t�@�u

        [SerializeField]
        private int maxGenerateCount;//Enemy�̍ő吶����

        [SerializeField]
        private float flightTime;//��s�@�̔�s����

        [SerializeField]
        private Transform enemiesTran;//Enemy�̐e�I�u�W�F�N�g

        [HideInInspector]
        public List<EnemyController> generatedEnemyList = new List<EnemyController>();//��������Enemy�̃��X�g

        private List<float> generateTimeList = new List<float>();//Enemy�𐶐����鎞�Ԃ̃��X�g

        private float timer;//�o�ߎ���

        private int generateCount;//Enemy�̐�����

        /// <summary>
        /// Enemy�𐶐�����
        /// </summary>
        /// <returns>�҂�����</returns>
        public IEnumerator GenerateEnemy() {
            //Enemy�̍ő吶���������J��Ԃ�
            for (int i = 0; i < maxGenerateCount; i++) {
                //Enemy�𐶐����鎞�Ԃ̃��X�g������������
                generateTimeList.Add(Random.Range(1f, flightTime));
            }

            //�����ɌJ��Ԃ�
            while (true) {
                //�o�ߎ��Ԃ��v��
                timer += Time.deltaTime;

                //Enemy�𐶐����鎞�Ԃ̃��X�g�̗v�f�������J��Ԃ�
                for (int i = 0; i < generateTimeList.Count; i++) {
                    //�o�ߎ��Ԃ��������Ԉȏ�ɂȂ�����
                    if (timer >= generateTimeList[i]) {

                        EnemyController enemy = Instantiate(enemyPrefab, transform.position, enemiesTran.rotation);

                        enemy.SetUpEnemy(uIManager, this, player, generateCount);

                        generatedEnemyList.Add(enemy);

                        //��������1��������
                        generateCount++;

                        //Enemy�𐶐����A�e��ݒ�
                        //Transform enemyTran = Instantiate(enemyPrefab, enemiesTran);

                        //��������Enemy�̏ꏊ�𒲐�
                        //enemyTran.position = transform.position;

                        //��������Enemy�����X�g�ɉ�����
                        //generatedEnemyList.Add(enemyTran.gameObject);

                        ////��������1��������
                        //generateCount++;

                        ////��������Enemy����EnemyController���擾
                        //if (enemyTran.gameObject.TryGetComponent(out EnemyController enemyController)) {
                        //    //��������Enemy�ɔԍ�����������
                        //    enemyController.MyNo = generateCount - 1;
                        //}
                        ////EnemyController�̎擾�Ɏ��s������
                        //else {
                        //    //�����
                        //    Debug.Log("EnemyController�̎擾�Ɏ��s");
                        //}

                        //Enemy�̐����X�V
                        uIManager.UpdateTxtOtherCount(enemiesTran.childCount);

                        //Enemy�𐶐����鎞�Ԃ̃��X�g���炻�̗v�f��r��
                        generateTimeList.RemoveAt(i);
                    }
                }

                //���̃t���[���֔�΂��i�����AUpdate���\�b�h�j
                yield return null;
            }
        }
    }
}