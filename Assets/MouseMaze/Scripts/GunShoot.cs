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
	if ( IsPlayer && ( Input.GetMouseButtonDown( 0 ) || Input.GetKeyDown( KeyCode.W ) || Input.GetKeyDown( KeyCode.UpArrow ) ) && CurrentAmmo > 0 )
    {
      Shooting();
      CurrentAmmo -= 1;
    }
  }

  public void Shooting()
  {
    float spawnDistance = 0.01f;
    GameObject bullet = Instantiate( Bullet, Nozzle.transform.position + spawnDistance * Nozzle.transform.forward, transform.rotation /*Quaternion.identity*/ ) as GameObject;
    bullet.GetComponent<Bullet>().Shoot();
    audio.PlayOneShot( GunShotSound );
  }
}
