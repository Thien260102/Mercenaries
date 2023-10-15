﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : Pet
{
	#region Fields & Properties
	[SerializeField] private Bullet bulletPrefab;

	#endregion

	#region Methods
	public static Turret Create(Transform parent = null)
	{
		Turret turret = Instantiate(Resources.Load<Turret>("_Prefabs/Pet/Turret"), parent);

		return turret;
	}

	public override void Initialize(string tag)
	{
		base.Initialize(tag);

		SO_Turret stats = (SO_Turret)GameManager.Instance.GetStats(GameConfig.SO_TYPE.PET, (int)GameConfig.PET.TURRET);
		Stats.Add(GameConfig.STAT_TYPE.HP, stats.HP_DEFAULT);
		Stats.Add(GameConfig.STAT_TYPE.ATTACK_SPEED, stats.ATTACK_SPEED_DEFAULT);
		Stats.Add(GameConfig.STAT_TYPE.DAMAGE, stats.DAMAGE_DEFAULT);
		Stats.Add(GameConfig.STAT_TYPE.BULLET_SPEED, stats.BULLET_SPEED);
		Stats.Add(GameConfig.STAT_TYPE.ATTACK_RANGE, stats.ATTACK_RANGE_DEFAULT);
	}

	public override void UpdatePet(List<Enemy> enemies = null)
	{
		base.UpdatePet(enemies);
	}

	protected override void Attack()
	{
		base.Attack();

		if (!target)
		{
			return;
		}

		if (attackable)
		{
			Bullet bullet = Instantiate(bulletPrefab, transform.position, new Quaternion());
			bullet.Initialize(Stats[GameConfig.STAT_TYPE.DAMAGE],
							  Stats[GameConfig.STAT_TYPE.ATTACK_RANGE],
							  Stats[GameConfig.STAT_TYPE.BULLET_SPEED],
							 (target.position - transform.position).normalized);
			bullet.tag = this.tag;

			attackable = false;
			StartCoroutine(IE_Reload());
		}
	}

	public override void TakenDamage(float damage)
	{
		base.TakenDamage(damage);
		if(Stats[GameConfig.STAT_TYPE.HP] > 0)
		{
			Stats[GameConfig.STAT_TYPE.HP] -= damage;
			if(Stats[GameConfig.STAT_TYPE.HP] <= 0)
			{
				IsDeath = true;
			}
		}
	}

	#endregion
}