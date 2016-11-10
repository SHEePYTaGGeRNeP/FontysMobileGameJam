namespace Assets.Scripts.Helpers.Components
{
    using System;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    [System.Serializable]

    public class FPSDisplay : MonoBehaviour
    {
        private Text _fpsText;

        private int _frames;
        private float _time;

        [SerializeField]
        private int _targetFrameRate = 60;

        [SerializeField]
        private float _dipFrameRate = 25;
        public float DipFrameRate { get { return this._dipFrameRate; } }

        [SerializeField]
        private bool _autoChooseDipFrameRate = true;

        // Length in seconds of time "buffer" to average.
        [SerializeField]
        private float _updateTime = 0.1f;

        private float _currentFps;
        private readonly CappedQueue<float> _fpsQueue = new CappedQueue<float>(10);
        public CappedQueue<float> BigFpsQueue { get; private set; }
        public int DipsCount { get; private set; }



        void Awake()
        {
            Application.targetFrameRate = this._targetFrameRate;
            this.BigFpsQueue = new CappedQueue<float>(1000);
        }


        void Start()
        {
            this._fpsText = this.GetComponent<Text>();
            if (FindObjectsOfType<FPSDisplay>().Length > 1)
                Destroy(this.gameObject);
        }


        void Update()
        {
            this._time += Time.deltaTime;
            this._frames++;
            if (this._time < this._updateTime) return;

            this._currentFps = 1.0f / (this._time / this._frames);
            this._fpsQueue.Enqueue(this._currentFps, true);

            if (this._autoChooseDipFrameRate)
                this._dipFrameRate = this._fpsQueue.Average()*0.7f;
            if (this._currentFps <= this._dipFrameRate)
                this.DipsCount++;

            this.BigFpsQueue.Enqueue(this._currentFps);
            float highest = float.MinValue;
            float lowest = float.MaxValue;

            foreach (float f in this._fpsQueue)
            {
                if (f > highest)
                    highest = f;
                if (f < lowest)
                    lowest = f;
            }

            this._fpsText.text = String.Format("FPS:{0} MIN:{1} MAX:{2} DIPS:{3}<{4}FPS", this._currentFps, lowest, highest, this.DipsCount, this.DipFrameRate);

            this._time = 0.0f;
            this._frames = 0;
        }

        public void Reset()
        {
            this.DipsCount = 0;
            this.BigFpsQueue.Clear();
        }
    }
}