using Omni.Core;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joysticks : ServiceBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private float lerpTime, distRadius;
    private Vector2 posInitial;
    public float angleFixed;
    public float angle;
    public bool joyMove = false;

    public Vector2 direcao;
    protected override void OnStart()
    {
        posInitial = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 posMouse = Input.mousePosition;

        Vector3 posAtual = posMouse.RadiusPosition(posInitial, distRadius);
        direcao = (Vector2)posAtual - posInitial;

        angleFixed = posAtual.RadiusAngle(posInitial);

        angle += angleFixed; 

        transform.position = Vector2.Lerp(transform.position, (Vector2)posAtual, lerpTime * Time.deltaTime);

        joyMove = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = posInitial;
        joyMove = false;
    }
    
}

