using System;
using System.Collections.Generic;
using System.Diagnostics;
/// <summary>
/// Helpful tool for recording how long something takes.
/// </summary>
public class StopwatchHelper
{
    private Stopwatch _stopWatch;

    private readonly List<string> _result = new List<string>();

    private string _current;

    public void Start(string name)
    {
        if (this._result.Count > 0)
            this._result.Clear();
        this._stopWatch = new Stopwatch();
        this._current = name;
        this._stopWatch.Start();
    }

    public string[] Stop()
    {
        this.AddAndClear();
        this._stopWatch.Stop();

        List<string> returnResult = new List<string>(this._result);
        //lvResult.Insert(0, "");
        //lvResult.Insert(0, "------------------------------------------------------------------------");
        //lvResult.Insert(0, "\t\t\t" + DateTime.Now + ": StopwatchHelper log");
        //lvResult.Insert(0, "------------------------------------------------------------------------");
        //lvResult.Insert(0, "");
        this._result.Clear();
        return returnResult.ToArray();
    }

    /// <summary>
    /// First call Start
    /// </summary>
    /// <param name="name"></param>
    public void Add(string name)
    {
        this.AddAndClear();
        this._current = name;
    }


    /// <summary>
    /// First call Start
    /// </summary>
    private void AddAndClear()
    {
        this._result.Add(String.Format("{0} \t\t\t\t\t\t\tTime elapsed: {1}", this._current, this._stopWatch.Elapsed));
        this._stopWatch.Reset();
        this._stopWatch.Start();
        this._current = String.Empty;
    }

}
