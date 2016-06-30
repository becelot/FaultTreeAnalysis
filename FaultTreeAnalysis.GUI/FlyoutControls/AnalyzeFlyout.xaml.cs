using System.Windows.Controls;

namespace FaultTreeAnalysis.GUI.FlyoutControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    using FaultTreeAnalysis.FaultTree;
    using FaultTreeAnalysis.GUI.Annotations;

    using LiveCharts;
    using LiveCharts.Configurations;

    public class MeasureModel
    {
        public double Time { get; set; }

        public double Value { get; set; }
    }

    /// <summary>
    /// Interaction logic for AnalyzeFlyout.xaml
    /// </summary>
    public partial class AnalyzeFlyout : UserControl, INotifyPropertyChanged
    {
        private double axisMin;

        private double axisMax;

        public AnalyzeFlyout()
        {
            this.InitializeComponent();
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
                return this.axisMin;
            }
            set
            {
                if (value.Equals(this.axisMin)) return;
                this.axisMin = value;
                this.OnPropertyChanged(nameof(this.AxisMin));
            }
        }

        public double AxisMax
        {
            get
            {
                return this.axisMax;
            }
            set
            {
                if (value.Equals(this.axisMax)) return;
                this.axisMax = value;
                this.OnPropertyChanged(nameof(this.AxisMax));
            }
        }

        public ChartValues<MeasureModel> Values { get; set; }

        public List<double> Input { get; set; }


        public DispatcherTimer Timer { get; set; }

        public async void Initialize(FaultTree faultTree)
        {
            if (this.Timer != null) this.Timer.IsEnabled = false;
            await Task.Delay(40);
            this.Values.Clear();
            this.Input = faultTree.Analyze(0.5, 40, 1e-16).ToList();
            
            this.Timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(35) };
            this.Timer.Tick += this.TimerOnTick;
            this.Timer.Start();
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            
            this.Values.Add(new MeasureModel { Time = Values.DefaultIfEmpty(new MeasureModel { Time = -0.5, Value = 0}).Last().Time + 0.5, Value = Input[0] });
            this.Input.RemoveAt(0);
            if (!this.Input.Any())
            {
                this.Timer.IsEnabled = false;
            }
        }

        public double AxisStep { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
