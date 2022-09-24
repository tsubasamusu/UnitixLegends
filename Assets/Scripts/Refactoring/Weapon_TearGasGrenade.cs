using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap 
{
    public class Weapon_TearGasGrenade : BulletDetailBase 
    {
        /// <summary>
        /// �e�̏������s��
        /// </summary>
        /// <param name="enemyController">EnemyController</param>
        public override void AddTriggerBullet(EnemyController enemyController) 
        {
            //�×ܒe���ʂ̏������s��
            enemyController.PrepareTearGasGrenade();
        }
    }
}