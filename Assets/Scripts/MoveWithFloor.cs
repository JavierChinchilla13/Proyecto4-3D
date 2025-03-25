using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithFloor : MonoBehaviour
{
    CharacterController player;

    Vector3 groundPosition;
    Vector3 lastGroundPosition;
    string groundName;
    string lastGroundName;

    Quaternion actualRot;
    Quaternion lastRot;

    // Start is called before the first frame update
    void Start()
    {
        player = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded) 
        { 
            RaycastHit hit;
            if(Physics.SphereCast(transform.position, player.height / 4.2f, -transform.up, out hit))
            {
                //TOMA LA INFORMACION DE LA COLISION
                GameObject groundedIn = hit.collider.gameObject;
                //TOMA LA INFORMACION DEL SUELO ACTUAL
                groundName = groundedIn.name;
                groundPosition = groundedIn.transform.position;
                actualRot = groundedIn.transform.rotation;
                //VALIDA SI SE ENCUENTRA SOBRE EL MISMO TERRENO
                //Y SE HA MOVIDO
                if(groundPosition != lastGroundPosition && groundName == lastGroundName)
                {
                    //MUEVE AL PERSONAJE
                    this.transform.position += groundPosition - lastGroundPosition;
                }
                //REALIZA LA ROTACION CON LA PLATAFORMA
                if (actualRot != lastRot && groundName == lastGroundName)
                { 
                    var newRot = this.transform.rotation * (actualRot.eulerAngles - lastRot.eulerAngles);
                    //GIRA SOBRE EL EJE DE LA PLATAFORMA
                    this.transform.RotateAround(groundedIn.transform.position, Vector3.up, newRot.y);
                }

                //ALMACENA LA INFORMACION DEL ULTIMO SUELO
                lastGroundName = groundName;
                lastGroundPosition = groundPosition;
                lastRot = actualRot;
            }
        }
        else
        {
            //LIMPIA LAS VARIABLES DEL SUELO
            lastGroundName = null;
            lastGroundPosition = Vector3.zero;
            lastRot = Quaternion.Euler(0,0, 0); 
        }

    }

    //private void OnDrawGizmos()
    //{
    //    player = this.GetComponent<CharacterController>();
    //    Gizmos.DrawSphere(transform.position, player.height / 4.2f);
    //}
}
