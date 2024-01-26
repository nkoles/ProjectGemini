using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraMovement : MonoBehaviour
{
    public Transform playerPositionRefPoint;
    public GameObject roundManager, gameLogic; 

    public Transform[] maskModels;
    public Transform[] maskTargetPosition;

    public void RAHHHH()
    {
        StartCoroutine(StartMovement());
    }

    public IEnumerator StartMovement(float speed = 1)
    {
        while(transform.position != playerPositionRefPoint.position || playerPositionRefPoint.rotation != transform.rotation)
        {
            transform.position = Vector3.Slerp(transform.position, playerPositionRefPoint.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation,playerPositionRefPoint.rotation, speed * Time.deltaTime);

            yield return null;
        }

        for(int i = 0; i < maskModels.Length; i++)
        {
            while (maskModels[i].position != playerPositionRefPoint.position)
            {
                maskModels[i].position = Vector3.Slerp(maskModels[i].position, maskTargetPosition[i].position, Time.deltaTime * speed * 10);

                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(0.2f, 1));
        }

        roundManager.SetActive(true);
        gameLogic.SetActive(true);
    }
}
