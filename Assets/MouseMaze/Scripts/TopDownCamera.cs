using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour
{
  private GameObject player;
  Vector3 offset;

  private Vector3 originPosition;
  private Quaternion originRotation;
  public float shake_decay;
  public float shake_intensity;

  void Start()
  {
  }

  public void SetTarget( GameObject target )
  {
    player = target;
    offset = transform.position - player.transform.position;
  }

  public void Shake()
  {
    originPosition = transform.position;
    originRotation = transform.rotation;
    shake_intensity = .03f;
    shake_decay = 0.01f;
  }

  void LateUpdate()
  {
    if ( player == null )
      return;

    if ( shake_intensity > 0 )
    {
      transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
      transform.rotation = new Quaternion(
      originRotation.x + Random.Range( -shake_intensity, shake_intensity ) * .2f,
      originRotation.y + Random.Range( -shake_intensity, shake_intensity ) * .2f,
      originRotation.z + Random.Range( -shake_intensity, shake_intensity ) * .2f,
      originRotation.w + Random.Range( -shake_intensity, shake_intensity ) * .2f );
      shake_intensity -= shake_decay;
      return;
    }

    Vector3 desiredPosition = player.transform.position + offset;
    transform.position = desiredPosition;
    transform.LookAt( player.transform.position );
    transform.eulerAngles = new Vector3( 90, 0, 0 );
  }

}
