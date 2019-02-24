using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// Lanzamiento de objeto haciendo "drag&drop" con el ratón.
/// 
/// La dirección y la fuerza van en función del vector que va desde la posición del objeto en el último frame antes
/// de soltar y la posición en el frame en el que se suelta. Por lo tanto, es imprescindible tener el
/// ratón en movimiento al soltar el botón o el objeto no tendrá velocidad.
/// 
/// Cuando cae al inframundo vuelve a su posición inicial.
/// 
/// Author: Fernando Paniagua
/// 
/// </summary>
public class Pokeball : MonoBehaviour
{
    public float forceMultiplier;//Multiplicador de fuerza
    public float closeLimit;//Límite mínimo de proximidad a la cámara
    public float distanceToCamera = 1f;//Distancia entre la bola y la camara
    float distance;//Profundidad del objeto en función del eje Y del ra´ton
    bool selected = false;//Indica si el objeto está seleccionado
    Vector3 lastFramePos, endPos;//Posición el último frame y del frame en el que se suelta el objeto
    Vector3 resetPos;//Posición inicial del objeto para hacer reset
    float y;//Movimiento en y del raton
    private void Awake()
    {
        resetPos = transform.position;
        distance = distanceToCamera;
    }
    private void OnMouseDown()
    {
        CaughtBall();
    }
    private void OnMouseUp()
    {
        ReleaseBall();
    }
    private void Update()
    {
        if (selected)
        {
            lastFramePos = transform.position;
            Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
            distance = Mathf.Max(closeLimit, distance + Input.GetAxis("Mouse Y"));
            transform.position = newPos;
        }
    }
    private void CaughtBall()
    {
        selected = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void ReleaseBall() {
        selected = false;
        //Guardamos la posicion final
        endPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Screen.height * 0.5f, distance));
        //Lanzamos
        ThrowBall();
    }
    private void ThrowBall()
    {
        Vector3 dir = endPos - lastFramePos;
        float force = dir.magnitude;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(dir * force * forceMultiplier);
    }
    private void OnTriggerEnter(Collider other)
    {
        //Reset pokeball
        transform.position = resetPos;
        GetComponent<Rigidbody>().isKinematic = true;
        distance = distanceToCamera;
    }
}
