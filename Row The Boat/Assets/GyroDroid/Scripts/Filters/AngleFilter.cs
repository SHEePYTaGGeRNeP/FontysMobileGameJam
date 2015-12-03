using UnityEngine;

public class AngleFilter : FloatFilter
{
    public override void UpdateFunc(float input)
    {
        this.Holder = Mathf.LerpAngle(this.Holder, input, Time.deltaTime * this.Hardness);
    }
    public AngleFilter(float hardness) : base(hardness)
    {
    }
}