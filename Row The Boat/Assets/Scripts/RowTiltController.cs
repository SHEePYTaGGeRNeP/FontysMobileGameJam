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

    private Vector3 _acceleration;
    private float _accumulatedStrength;
    private float _efficiency;
    private Vector3 _rotation;
    private RowSide _rowSide;

    public event EventHandler<RowEventArgs> Row;

    // Use this for initialization
    private void Start()
    {
        Sensor.Activate(Sensor.Type.GameRotationVector);
    }

    // Update is called once per frame
    private void Update()
    {
        _rotation = Sensor.gameRotationVector;
        _acceleration = Input.gyro.userAcceleration;
        Debug.Log(string.Format("Rot: {0}, Accel: {1}", _rotation, _acceleration));

        CheckRowSide();
        CheckEfficiency();
        CheckRowMotion();
    }

    public void CheckRowSide()
    {
        if (_rotation.z >= 15 && _rotation.z <= 60)
        {
            _rowSide = RowSide.Left;
        }
        else if (_rotation.z <= 345 && _rotation.z >= 300)
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
            if (_rotation.x >= 270)
            {
                _efficiency = ((_rotation.x - 270) * (100f / 90f)) / 100f;
            }
            else if (_rotation.x <= 90)
            {
                _efficiency = Mathf.Abs(((_rotation.x - 90) * (100f / 90f)) / 100f);
            }
            else
            {
                _efficiency = 0;
            }
        }
        else
        {
            _efficiency = 0;
        }
    }

    public void CheckRowMotion()
    {
        if (_acceleration.z >= 0.05f)
        {
            _accumulatedStrength += _acceleration.z;
        }
        else if (_accumulatedStrength >= 0.3f)
        {
            OnRow(new RowEventArgs(_rowSide, _accumulatedStrength, _efficiency));
            Debug.Log(string.Format("Side: {0}, Strength: {1}, Efficiency: {2}", _rowSide, _accumulatedStrength,
                _efficiency));
            _accumulatedStrength = 0;
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