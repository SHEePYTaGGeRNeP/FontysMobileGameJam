using UnityEngine;

public class FloatFilter : Filter<float>
{
    public override void UpdateFunc(float input)
    {
        this.Holder = Mathf.Lerp(this.Holder, input, Time.deltaTime * this.Hardness);
    }
    public FloatFilter(float hardness) : base(hardness)
    {
    }
}