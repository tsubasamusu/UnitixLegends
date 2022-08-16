using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField]
    private int shotCount;//�i���j

    [SerializeField]
    private Rigidbody bulletPrefab;//�i���j

    [SerializeField]
    private float shotSpeed;//�i���j

    [SerializeField]
    private Transform mainCamera;//���C���J����

    [SerializeField]
    private Transform temporaryObjectContainerTran;//�ꎞ�I�ɃQ�[���I�u�W�F�N�g�����e����Transform

    /// <summary>
    /// ���t���[���Ăяo�����
    /// </summary>
    void Update()
    {
        //Bullet�𔭎˂���������J�����̌����ɍ��킹��
        transform.eulerAngles = new Vector3(mainCamera.eulerAngles.x,mainCamera.eulerAngles.y,transform.eulerAngles.z);
    }

    /// <summary>
    /// ShotBullet�Q�[���I�u�W�F�N�g��L�����E���������s��
    /// </summary>
    public void SetShotBulletActiveOrPassive(bool set)
    {
        //���������ɁAShotBullet�Q�[���I�u�W�F�N�g�̗L�����E��������؂�ւ���
        transform.gameObject.SetActive(set);
    }

    /// <summary>
    /// �e�𔭎˂���
    /// </summary>
    public void ShotBullet()
    {
        //TODO:GameData����I�����Ă��镐��̒e�̏����擾���鏈�����Ăяo��

        //�e�𐶐�
        Rigidbody bulletRb=Instantiate(bulletPrefab,transform.position, Quaternion.Euler(transform.parent.eulerAngles.x, transform.parent.eulerAngles.y, 0));

        //�e�𔭎�
        bulletRb.AddForce(transform.forward* shotSpeed);

        //Player�̎�������˂����e�͈�莞�Ԏc��
        bulletRb.gameObject.transform.SetParent(temporaryObjectContainerTran);

        //�c�e�������炷
        shotCount--;

        //���˂����e��3.0�b��ɏ���
        Destroy(bulletRb.gameObject,3.0f);

        //TODO:SoundManager���畐��̔��ˉ����Đ����鏈�����Ăяo��
    }
}
