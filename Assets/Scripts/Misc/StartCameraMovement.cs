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

    public IEnumerator StartMovement()
    {

        for (float j = 0; j <= 1; j += 0.01f)
        {
            transform.position = Vector3.Slerp(transform.position, playerPositionRefPoint.position, j);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerPositionRefPoint.rotation, j);

            yield return new WaitForSeconds(0.02f);
        }

        print("Camera Scroll Finished");

        StartCoroutine(StartMaskMovement());
    }

    public IEnumerator StartMaskMovement()
    {
        for (int i = 0; i < maskModels.Length; i++)
        {
            for (float j = 0; j <= 1; j += 0.01f)
            {
                print("why ???");
                maskModels[i].position = Vector3.Slerp(maskModels[i].position, maskTargetPosition[i].position, j);

                yield return new WaitForSeconds(0.02f);
            }

            yield return new WaitForSeconds(Random.Range(0.2f, 1));
        }

        roundManager.SetActive(true);
        gameLogic.SetActive(true);
    }
}
