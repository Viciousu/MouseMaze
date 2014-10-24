using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

  private float lifetime = 1.0f;
  public float BulletSpeed = 1200f;

  void Awake()
  {
    Destroy( this.gameObject, lifetime );
  }

	public void FaceTarget(Vector3 target)
	{
		transform.LookAt(target);	
	}

	public void Shoot()
	{
    GetComponent<Rigidbody>().AddRelativeForce( new Vector3( 0, 0, BulletSpeed ) );
	}

  void OnTriggerEnter()
  {
    Destroy( this.gameObject );
  }
}
