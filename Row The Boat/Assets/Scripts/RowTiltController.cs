using UnityEngine;
using System.Collections;

public class RowTiltController : MonoBehaviour {
    

    Vector3 Rotation;
    Vector3 Acceleration;

    enum RowSide
    {
        Left, Right, None
    }

    RowSide rowSide;

    float Efficiency;
    float AccumulatedStrength;

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Rotation = SensorHelper.rotation.eulerAngles;
        CheckRowSide();
        CheckEfficiency();
        CheckRowMotion();
	}

    public void CheckRowSide()
    {
        if (Rotation.z >= 15 && Rotation.z <= 60)
        {
            rowSide = RowSide.Left;
        }
        else if(Rotation.z <= 345 && Rotation.z >= 300)
        {
            rowSide = RowSide.Right;
        }
        else
        {
            rowSide = RowSide.None;
        }
    }

    public void CheckEfficiency()
    {
        if (rowSide != RowSide.None)
        {
            if (Rotation.x >= 270)
            {
                Efficiency = ((Rotation.x - 270) * (100f / 90f)) / 100f;
            }
            else if (Rotation.x <= 90)
            {
                Efficiency = Mathf.Abs(((Rotation.x - 90) * (100f / 90f)) / 100f);
            }
            else
            {
                Efficiency = 0;
            }
        }
        else
        {
            Efficiency = 0;
        }
    }

    public void CheckRowMotion()
    {
        Acceleration = Input.gyro.userAcceleration;
        if (Acceleration.z >= 0.05f)
        {
            AccumulatedStrength += Acceleration.z;
        }
        else if(AccumulatedStrength >= 0.3f)
        {
            AccumulatedStrength = 0;
            //Call Function with all information > AddForce(RowSide rowside, float efficiency, float accumulatedstrength)
        }
    }
}
