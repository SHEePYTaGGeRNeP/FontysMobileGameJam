using UnityEngine;

public class Vector3Filter : Filter<Vector3>
{
    public override void UpdateFunc(Vector3 input)
    {
        this.Holder = Vector3.Lerp(this.Holder, input, Time.deltaTime * this.Hardness);
    }
    public Vector3Filter(float hardness) : base(hardness)
    {
    }
}