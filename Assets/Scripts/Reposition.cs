using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    //float dirX;

    private void LateUpdate()
    {
        transform.localScale = GameManager.Instance.player.scaleVec * 1.5f;
    }

    //void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (!collision.CompareTag("Area"))
    //        return;

    //    dirX = GameManager.Instance.player.isLeft ? -1f : 1f;

    //    switch (transform.tag)
    //    {
    //        case "Background":
    //            transform.Translate(Vector3.right * dirX * 40);
    //            break;
    //        case "Enemy":

    //            break;
    //    }
    //}
}
