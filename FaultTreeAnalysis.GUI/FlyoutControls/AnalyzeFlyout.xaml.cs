using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;
using FaultTreeAnalysis.GUI.Annotations;
using LiveCharts;
using LiveCharts.Configurations;

namespace FaultTreeAnalysis.GUI.FlyoutControls
{
	public class MeasureModel
    {
        public double Time { get; set; }

        public double Value { get; set; }
    }

    /// <summary>
    /// Interaction logic for AnalyzeFlyout.xaml
    /// </summary>
    public partial class AnalyzeFlyout : INotifyPropertyChanged
    {
        private double axisMin;

        private double axisMax;
	    private double axisStep;

	    public AnalyzeFlyout()
        {
            InitializeComponent();
            var mapper = Mappers.Xy<MeasureModel>().X(model => model.Time).Y(model => model.Value);

            Charting.For<MeasureModel>(mapper);
            this.Values = new ChartValues<MeasureModel>();

            this.AxisStep = 8;
            this.AxisMin = 0;
            this.AxisMax = 40;

            this.DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double AxisMin
        {
            get
            {
                return axisMin;
            }
            set
            {
                if (value.Equals(axisMin)) return;
                axisMin = value;
                OnPropertyChanged(nameof(AxisMin));
            }
        }

        public double AxisMax
        {
            get
            {
                return axisMax;
            }
            set
            {
                if (value.Equals(axisMax)) return;
                axisMax = value;
                OnPropertyChanged(nameof(AxisMax));
            }
        }

        public ChartValues<MeasureModel> Values { get; set; }

        public List<double> Input { get; set; }


        public DispatcherTimer Timer { get; set; }

        public async void Initialize(FaultTree.FaultTree faultTree)
        {
            if (this.Timer != null) this.Timer.IsEnabled = false;
            await Task.Delay(40);
			this.Input = faultTree.Analyze(Config.Instance.SamplingRate, Config.Instance.TimeSpan, Config.Instance.ErrorTolerance).ToList();
	        this.AxisMax = Config.Instance.TimeSpan;
	        this.AxisStep = Config.Instance.TimeSpan * 0.2d;

			this.Values.Clear();
           
            this.Timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(2800 / this.Input.Count) };
            this.Timer.Tick += TimerOnTick;
            this.Timer.Start();
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            
            this.Values.Add(new MeasureModel { Time = this.Values.DefaultIfEmpty(new MeasureModel { Time = -Config.Instance.SamplingRate, Value = 0}).Last().Time + Config.Instance.SamplingRate, Value = Input[0] });
            this.Input.RemoveAt(0);
            if (!this.Input.Any())
            {
                this.Timer.IsEnabled = false;
            }
        }

	    public double AxisStep
	    {
		    get { return axisStep; }
		    set
		    {
			    if (value.Equals(axisStep)) return;
			    axisStep = value;
			    OnPropertyChanged();
		    }
	    }

	    [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
