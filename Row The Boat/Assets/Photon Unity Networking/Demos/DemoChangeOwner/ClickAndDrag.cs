using UnityEngine;
using System.Collections;

public class ClickAndDrag : Photon.MonoBehaviour
{
    private Vector3 camOnPress;
    private bool following;
    private float factor = -0.1f;

	// Update is called once per frame
	void Update ()
	{
        if (!this.photonView.isMine)
        {
            return;
        }

	    InputToEvent input = Camera.main.GetComponent<InputToEvent>();
	    if (input == null) return;
        if (!this.following)
        {
            if (input.Dragging)
            {
                this.camOnPress = this.transform.position;
                this.following = true;
            }
            else
            {
                return;
            }
        }
        else
        {
            if (input.Dragging)
            {
                Vector3 target = this.camOnPress - (new Vector3(input.DragVector.x, 0, input.DragVector.y) * this.factor);
                this.transform.position = Vector3.Lerp(this.transform.position, target, Time.deltaTime*.5f);
            }
            else
            {
                this.camOnPress = Vector3.zero;
                this.following = false;
            }
        }
	}
}
