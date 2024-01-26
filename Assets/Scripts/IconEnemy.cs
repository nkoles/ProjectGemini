using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class IconEnemy : MonoBehaviour
{
    private Vector3 _defaultPosition;

    private void Start()
    {
        _defaultPosition = transform.position;
    }

    private IEnumerator SlideToPosition(Vector3 refPoint, float speed = 10)
    {
        transform.position = _defaultPosition;

        while(transform.position != refPoint) {
            transform.position = Vector3.Slerp(transform.position, refPoint, speed*Time.deltaTime);

            yield return null;
        }
    }
}
