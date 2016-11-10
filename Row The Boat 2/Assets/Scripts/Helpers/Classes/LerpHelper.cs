using System;
using UnityEngine;

[Serializable]
public class LerpHelper
{
    /// <summary>
    /// http://wiki.unity3d.com/index.php?title=Mathfx
    /// </summary>
    public enum LerpType
    {
        Lerp,
        Hermite,
        Sinerp,
        Coserp,
        Berp,
        Smoothstep,
        Clerp
    }

    [SerializeField]
    private LerpType _currentLerpType;

    [SerializeField]
    private float _from;
    public float From { get { return this._from; } set { this._from = value; } }

    [SerializeField]
    private float _to = 1;
    public float To { get { return this._to; } set { this._to = value; } }

    public float CurrentValue { get; private set; }

    [SerializeField]
    [Range(float.Epsilon, float.MaxValue)]
    private float _speed = 1;
    public float Speed { get { return this._speed; } set { this._speed = value; } }

    [SerializeField]
    [Range(0, float.MaxValue)]
    private float _margin;
    public float Margin { get { return this._margin; } set { this._margin = value; } }

    public float LerpZeroToOne { get; private set; }

    public delegate void LerpReachedHandler();
    public event LerpReachedHandler OnLerpReached;

    public bool IsLerpReached
    {
        get
        {
            if (this.From < this.To)
                return this.To - this.CurrentValue < this.Margin;
            return this.To + this.CurrentValue < this.Margin;
        }
    }

    public LerpHelper(float from, float to, float margin, float speed, LerpType lerpType)
    {
        this.From = from;
        this.CurrentValue = from;
        this.To = to;
        this.Margin = margin;
        this.Speed = speed;
        this._currentLerpType = lerpType;
    }

    /// <summary>
    /// Increase the lerp value. Usually called in Update
    /// </summary>
    /// <param name="value">Time.deltaTime</param>
    public float LerpStep(float value)
    {
        this.LerpZeroToOne += Mathf.Min(1, value * this.Speed);
        this.CurrentValue = this.DoLerp();
        if (!this.IsLerpReached) return this.CurrentValue;
        if (this.OnLerpReached != null)
            this.OnLerpReached.Invoke();
        return this.CurrentValue;
    }

    public void Reset()
    {
        this.LerpZeroToOne = 0;
        this.CurrentValue = this.From;
    }

    private float DoLerp()
    {
        switch (this._currentLerpType)
        {
            case LerpType.Lerp: return Mathfx.Lerp(this.From, this.To, this.LerpZeroToOne);
            case LerpType.Hermite: return Mathfx.Lerp(this.From, this.To, this.LerpZeroToOne);
            case LerpType.Sinerp: return Mathfx.Sinerp(this.From, this.To, this.LerpZeroToOne);
            case LerpType.Coserp: return Mathfx.Coserp(this.From, this.To, this.LerpZeroToOne);
            case LerpType.Berp: return Mathfx.Berp(this.From, this.To, this.LerpZeroToOne);
            case LerpType.Smoothstep: return Mathfx.SmoothStep(this.From, this.To, this.LerpZeroToOne);
            case LerpType.Clerp: return Mathfx.Clerp(this.From, this.To, this.LerpZeroToOne);
            default: throw new ArgumentOutOfRangeException();
        }
    }
}
