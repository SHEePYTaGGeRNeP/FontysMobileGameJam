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
        this.Rotation = SensorHelper.rotation.eulerAngles;
        this.Acceleration = Input.gyro.userAcceleration;
        if (this.Debugging) Debug.Log(string.Format("Rot: {0}, Accel: {1}", this.Rotation, this.Acceleration));

        this.CheckRowSide();
        this.CheckEfficiency();
        this.CheckRowMotion();
    }

    public void CheckRowSide()
    {
        if (this.Rotation.z >= 15 && this.Rotation.z <= 60)
        {
            this._rowSide = RowSide.Left;
        }
        else if (this.Rotation.z <= 345 && this.Rotation.z >= 300)
        {
            this._rowSide = RowSide.Right;
        }
        else
        {
            this._rowSide = RowSide.None;
        }
    }

    public void CheckEfficiency()
    {
        if (this._rowSide != RowSide.None)
        {
            if (this.Rotation.x >= 270)
            {
                this.Efficiency = ((this.Rotation.x - 270) * (100f / 90f)) / 100f;
            }
            else if (this.Rotation.x <= 90)
            {
                this.Efficiency = Mathf.Abs(((this.Rotation.x - 90) * (100f / 90f)) / 100f);
            }
            else
            {
                this.Efficiency = 0;
            }
        }
        else
        {
            this.Efficiency = 0;
        }
    }

    public void CheckRowMotion()
    {
        if (Mathf.Abs(this.Acceleration.z) >= 0.05f)
        {
            this.AccumulatedStrength += Mathf.Abs(this.Acceleration.z);
        }
        else if (Mathf.Abs(this.AccumulatedStrength) >= 0.3f)
        {
            this.OnRow(new RowEventArgs(this._rowSide, this.AccumulatedStrength, this.Efficiency));
            if (this.Debugging) Debug.Log("Rowed! " + this.AccumulatedStrength);
            this.AccumulatedStrength = 0;
        }
    }

    protected virtual void OnRow(RowEventArgs e)
    {
        var row = this.Row;
        if (row != null) row.Invoke(this, e);
    }

    public class RowEventArgs : EventArgs
    {
        public RowEventArgs(RowSide side, float strength, float efficiency)
        {
            this.Side = side;
            this.Strength = strength;
            this.Efficiency = efficiency;
        }

        public RowSide Side { get; protected set; }
        public float Strength { get; protected set; }
        public float Efficiency { get; protected set; }

        public override string ToString()
        {
            return string.Format("Side: {0}, Strength: {1}, Efficiency: {2}", this.Side, this.Strength, this.Efficiency);
        }
    }
}