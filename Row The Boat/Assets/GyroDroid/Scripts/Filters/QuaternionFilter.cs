using UnityEngine;

public class QuaternionFilter : Filter<Quaternion>
{
    public override void UpdateFunc(Quaternion input)
    {
        this.Holder = Quaternion.Lerp(this.Holder, input, Time.deltaTime * this.Hardness);
    }
    public QuaternionFilter(float hardness) : base(hardness)
    {
    }
}