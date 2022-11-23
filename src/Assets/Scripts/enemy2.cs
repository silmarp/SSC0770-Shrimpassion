using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class enemy2 : MonoBehaviour
{
	public float contParado = 0;
	public float contador = 0f;
	public float pi=3f;
	public double acceleration = 0;
	public bool idle;
	public float damage;
	public float speed;
	private Rigidbody2D rb2d;
	private float direction;
	private Animator anim;
	public float health;
	private bool walking;
	public Transform groundLocation;
	public LayerMask Ground;
	
	private bool isDamageable;
	public GameObject explosionEffect;
	
	public Rigidbody2D seahorse;
	public Collider2D colSeahorse;
    public Collider2D colCatWalk;
	public float walkForce;
	private bool trava;

	// Use this for initialization
	void Start()
	{
		trava=false;
		idle = true;
		damage = 0;
		speed = 0.2f;
		rb2d = GetComponent<Rigidbody2D>();
		//anim = GetComponent<Animator>();
		direction = 0;
		health = 2;
		isDamageable = false;
		walking = false;
		walkForce = 1f;

	}

	// Update is called once per frame
	void Update()
	{
		if(Input.anyKey) trava=true;
		contador=(contador+1)%100;
		//acceleration = contador%3 - 1;
		acceleration = Math.Sin(contador);
		//anim.SetBool("isIdle", idle);
		rb2d.velocity = new Vector2(direction * speed, rb2d.velocity.y);
		//if (!idle)
		//{
			isDamageable = true;
		//}
		
		contParado++;
		if(trava==true){
		if (Physics2D.OverlapCircle (groundLocation.position, 0.5f, Ground) && contador < 25)     
        {	
			seahorse.AddForce(Vector2.left * 25, (ForceMode2D)ForceMode.Impulse);
        }
		//else seahorse.AddForce(Vector2.right * 10, (ForceMode2D)ForceMode.Impulse);
			
		//else 	seahorse.AddForce(Vector2.left * 0.75f, (ForceMode2D)ForceMode.Acceleration);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			if (idle)
			{
				idle = false;
				transform.gameObject.tag = "Enemy";
				damage = 1;

				if (other.transform.position.x < transform.position.x)
				{
					direction = -1;
				}
				else
				{
					direction = 1;
				}
			}
			else
			{
				if (LifeController.isDamageable)
				{
					if (other.transform.position.x < transform.position.x)
					{
						PlayerController.knockRight = true;
					}
					else
					{
						PlayerController.knockRight = false;
					}
					other.SendMessage("ApplyDamage", damage);
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Stop")
		{
			direction = -1 * direction;
		}
	}

	void ApplyDamage(float damage)
	{
		if (isDamageable && damage > 0)
		{
			StartCoroutine(FlashingDamage());
			health -= damage;

			if (health <= 0)
			{
				GetComponent<Renderer>().enabled = false;
				//Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
				Destroy(gameObject);
			}

			isDamageable = false;
			Invoke("ResetIsDamageable", 1.2f);
		}
	}

	void ResetIsDamageable()
	{
		isDamageable = true;
	}

	IEnumerator FlashingDamage()
	{
		int i = 0;
		while (i < 8)
		{
			GetComponent<Renderer>().enabled = false;
			yield return new WaitForSeconds(0.05f);
			GetComponent<Renderer>().enabled = true;
			yield return new WaitForSeconds(0.05f);
			i++;
		}
		GetComponent<Renderer>().enabled = true;
	}
	
	public void walkCoolDown()
	{
		walking = false;
	}
}