using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections;

public class CinematicController : MonoBehaviour
{
    [Header("Cinematic Cameras")]
    [SerializeField] private CinemachineCamera camA;

    [SerializeField] private CinemachineCamera camB;

    [SerializeField] private CinemachineCamera camC;

    [SerializeField] private CinemachineCamera camD;

    [Header("Gameplay Camera")]
    [SerializeField] private CinemachineCamera gameplayCam;

    [Header("Settings")]
    [SerializeField] private float timePerCamera = 5f;

    private int currentCam = 0;

    private void Start()
    {
        // Bloquear movimiento durante intro
       // Ship.canMove = false;

        // Empezar cinemática
        StartCoroutine(PlayCinematic());
    }

    [Button]
    public void SwitchCameras()
    {
        currentCam++;

        if (currentCam > 3)
            currentCam = 0;

        ActivateCamera(currentCam);
    }

    private void ActivateCamera(int index)
    {
        // Desactivar todas las cámaras cinemáticas
        camA.Priority = 0;
        camB.Priority = 0;
        camC.Priority = 0;
        camD.Priority = 0;

        switch (index) // Activar solo una
 
        {
            case 0:
                camA.Priority = 20;
                break;

            case 1:
                camB.Priority = 20;
                break;

            case 2:
                camC.Priority = 20;
                break;

            case 3:
                camD.Priority = 20;
                break;
        }
    }

    private IEnumerator PlayCinematic()
    {
       
        ActivateCamera(0);
        yield return new WaitForSeconds(timePerCamera);

        ActivateCamera(1);
        yield return new WaitForSeconds(timePerCamera);


        ActivateCamera(2);
        yield return new WaitForSeconds(timePerCamera);


        ActivateCamera(3);
        yield return new WaitForSeconds(timePerCamera);

 
        camA.Priority = 0;
        camB.Priority = 0;
        camC.Priority = 0;
        camD.Priority = 0;

     
        gameplayCam.Priority = 50;

        // Devolver control jugador
        //Ship.canMove = true;

        Debug.Log("Start");

      
        enabled = false;
    }
}