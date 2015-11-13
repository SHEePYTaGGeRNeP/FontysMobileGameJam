using System;
using UnityEngine;

public class RowTiltController : MonoBehaviour
{
    public enum RowSide
    {
        None,
        Left,
        Right
    }

    public bool Debugging;
    public Vector3 Acceleration;
    public Vector3 Rotation;
    public float AccumulatedStrength;
    public float Efficiency;
    public RowSide _rowSide;

    public event EventHandler<RowEventArgs> Row;

    // Use this for initialization
    private void Start()
    {
        SensorHelper.ActivateRotation();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Rotation = SensorHelper.rotation.eulerAngles;
        Acceleration = Input.gyro.userAcceleration;
        if (Debugging) Debug.Log(string.Format("Rot: {0}, Accel: {1}", Rotation, Acceleration));

        CheckRowSide();
        CheckEfficiency();
        CheckRowMotion();
    }

    public void CheckRowSide()
    {
        if (Rotation.z >= 15 && Rotation.z <= 60)
        {
            _rowSide = RowSide.Left;
        }
        else if (Rotation.z <= 345 && Rotation.z >= 300)
        {
            _rowSide = RowSide.Right;
        }
        else
        {
            _rowSide = RowSide.None;
        }
    }

    public void CheckEfficiency()
    {
        if (_rowSide != RowSide.None)
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
        if (Mathf.Abs(Acceleration.z) >= 0.05f)
        {
            AccumulatedStrength += Mathf.Abs(Acceleration.z);
        }
        else if (Mathf.Abs(AccumulatedStrength) >= 0.3f)
        {
            OnRow(new RowEventArgs(_rowSide, AccumulatedStrength, Efficiency));
            if (Debugging) Debug.Log("Rowed! " + AccumulatedStrength);
            AccumulatedStrength = 0;
        }
    }

    protected virtual void OnRow(RowEventArgs e)
    {
        var row = Row;
        if (row != null) row.Invoke(this, e);
    }

    public class RowEventArgs : EventArgs
    {
        public RowEventArgs(RowSide side, float strength, float efficiency)
        {
            Side = side;
            Strength = strength;
            Efficiency = efficiency;
        }

        public RowSide Side { get; protected set; }
        public float Strength { get; protected set; }
        public float Efficiency { get; protected set; }

        public override string ToString()
        {
            return string.Format("Side: {0}, Strength: {1}, Efficiency: {2}", Side, Strength, Efficiency);
        }
    }
}