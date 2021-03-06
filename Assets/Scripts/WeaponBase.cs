using UnityEngine;

public abstract class WeaponBase : ScriptableObject
{
	public Sprite icon;
	public Sprite equippedSprite;
	public new string name;
	public bool hasAmmo;
	public int ammoSize;
	public int maxAmmoSize;
	public float attackCoolDown;
	public GameObject projectile;
	public AudioClip attackSound;

	public abstract void Shoot();
	public abstract void Smash(BoxCollider2D collider2D);
}
