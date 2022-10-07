using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yamap 
{
    public class Weapon_TearGasGrenade : BulletDetailBase 
    {
        /// <summary>
        /// ’e‚Ìˆ—‚ğs‚¤
        /// </summary>
        /// <param name="enemyController">EnemyController</param>
        public override void AddTriggerBullet(EnemyController enemyController) 
        {
            //Ã—Ü’eŒø‰Ê‚Ì€”õ‚ğs‚¤
            enemyController.PrepareTearGasGrenade();
        }
    }
}