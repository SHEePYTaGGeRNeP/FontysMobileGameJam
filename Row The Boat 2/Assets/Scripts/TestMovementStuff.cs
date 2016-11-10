using UnityEngine;
namespace Assets.Scripts
{
    class TestMovementStuff : MonoBehaviour
    {


        private void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            this.transform.Translate(new Vector3(x * Time.deltaTime * 10, 0, z * Time.deltaTime * 10));
            float rotY = 0;
            if (Input.GetKey(KeyCode.E))
                rotY = 51 * Time.deltaTime;
            else if (Input.GetKey(KeyCode.Q))
                rotY = -51 * Time.deltaTime;
            if (rotY != 0)
                this.transform.Rotate(0, rotY, 0, Space.Self);
        }
    }
}
