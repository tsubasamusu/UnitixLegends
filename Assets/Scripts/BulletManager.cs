using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;//���C���J����

    public List<GameObject> bulletPrefabList = new List<GameObject>();//�e�̃v���t�@�u�̃��X�g

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    void Update()
    {
        //Bullet�𔭎˂���������J�����̌����ɍ��킹��
        transform.eulerAngles = new Vector3(mainCamera.transform.eulerAngles.x,mainCamera.transform.eulerAngles.y,transform.eulerAngles.z);
    }

    /// <summary>
    /// ShotBullet�Q�[���I�u�W�F�N�g��L�����E���������s��
    /// </summary>
    public void SetShotBulletActiveOrPassive(bool set)
    {
        //���������ɁAShotBullet�Q�[���I�u�W�F�N�g�̗L�����E��������؂�ւ���
        transform.gameObject.SetActive(set);
    }
}
