using UnityEngine;
using System.Collections;

public class GunShoot : MonoBehaviour
{

  public Texture Crosshair;
  public GUIStyle ammoGUIStyle;
  public Texture AmmoIcon;
  public GameObject Nozzle;
  public GameObject Bullet;
  public Camera FPSCamera;
  public int TotalAmmo = 6;
  public int CurrentAmmo;
  public AudioClip GunShotSound;
  public bool IsPlayer;

  // Use this for initialization
  void Start()
  {
    CurrentAmmo = TotalAmmo;
  }

  // Update is called once per frame
  void Update()
  {
	if ( IsPlayer && ( Input.GetKeyDown( KeyCode.W ) || Input.GetKeyDown( KeyCode.UpArrow ) ) )
    {
		Shooting();
    }
  }

  public void Shooting()
  {
	if(CurrentAmmo <= 0 )
		return;

    float spawnDistance = 0.03f;
	GameObject bullet = Instantiate( Bullet, Nozzle.transform.position + spawnDistance * Nozzle.transform.forward, Nozzle.transform.rotation ) as GameObject;
    bullet.GetComponent<Bullet>().Shoot();
    audio.PlayOneShot( GunShotSound );
    CurrentAmmo -= 1;
  }
}
