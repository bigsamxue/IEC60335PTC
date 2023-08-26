using IEC60335Develop.Models;
using IEC60335Develop.ViewModels;
using ScottPlot.Plottable;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace IEC60335Develop.Views {
    /// <summary>
    /// MeasureWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MeasureView : UserControl {
        public MeasureView() {
            InitializeComponent();
            WTMeasureModel = new WTMeasureModel();
            //WTMeasureModel.VoltageValue = new double[100_000];
            //WTMeasureModel.CurrentValue = new double[100_000];
            //WTMeasureModel.PowerValue = new double[100_000];
            WTMeasureModel.VoltageValue = new List<double>();
            WTMeasureModel.CurrentValue = new List<double>();
            WTMeasureModel.PowerValue = new List<double>();

        }


        private WTMeasureModel _wTMeasureModel;
        public WTMeasureModel WTMeasureModel {
            get { return _wTMeasureModel; }
            set { _wTMeasureModel = value; }
        }


        int nextDataIndex = 1;
        SignalPlot signalPlot;
        Random rand = new Random(0);

        private DispatcherTimer _updateDataTimer;
        private DispatcherTimer _renderTimer;

        private void StartButton_Click(object sender, RoutedEventArgs e) {
            ConnectionViewModel.WT1800.RemoteCTRL(":HSPEED:START");

            Task task = new Task(new Action(GetValue));

            task.Start();








        }

        void GetValue() {
            for (int i = 0; i < 3; i++) {
                var voltageValue = ConnectionViewModel.WT1800.RemoteCTRL(":NUMeric:HSPeed:VALue? " + (double.Parse(SettingViewModel.ElementCopyToMesView.Substring(7)) + 2 * (double.Parse(SettingViewModel.ElementCopyToMesView.Substring(7)) - 1)).ToString());
                var currentValue = ConnectionViewModel.WT1800.RemoteCTRL(":NUMeric:HSPeed:VALue? " + (double.Parse(SettingViewModel.ElementCopyToMesView.Substring(7)) + 1 + 2 * (double.Parse(SettingViewModel.ElementCopyToMesView.Substring(7)) - 1)).ToString());
                var powerValue = ConnectionViewModel.WT1800.RemoteCTRL(":NUMeric:HSPeed:VALue? " + (double.Parse(SettingViewModel.ElementCopyToMesView.Substring(7)) + 2 + 2 * (double.Parse(SettingViewModel.ElementCopyToMesView.Substring(7)) - 1)).ToString());
                var powerMaxValue = ConnectionViewModel.WT1800.RemoteCTRL(":NUMERIC:HSPEED:MAXIMUM? " + (double.Parse(SettingViewModel.ElementCopyToMesView.Substring(7)) + 2 + 2 * (double.Parse(SettingViewModel.ElementCopyToMesView.Substring(7)) - 1)).ToString());
                //WTMeasureModel.VoltageValue = Array.ConvertAll<string, double>(voltageValue.Split(','), s => double.Parse(s));//
                //WTMeasureModel.CurrentValue = Array.ConvertAll<string, double>(currentValue.Split(','), s => double.Parse(s));
                //WTMeasureModel.PowerValue = Array.ConvertAll<string, double>(powerValue.Split(','), s => double.Parse(s));
                WTMeasureModel.VoltageValue.AddRange(Array.ConvertAll<string, double>(voltageValue.Split(','), s => double.Parse(s)));
                WTMeasureModel.CurrentValue.AddRange(Array.ConvertAll<string, double>(currentValue.Split(','), s => double.Parse(s)));
                WTMeasureModel.PowerValue.AddRange(Array.ConvertAll<string, double>(powerValue.Split(','), s => double.Parse(s)));
            }
            string path = @"C:\Users\30042383\Desktop\info.csv";
            if (!File.Exists(path))
                File.Create(path).Close();

            StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8);
            string dataHeader = "电压,电流,功率";
            sw.WriteLine(dataHeader);
            for (int j = 0; j < WTMeasureModel.VoltageValue.Count; j++) {
                sw.WriteLine($"{WTMeasureModel.VoltageValue[j]},{WTMeasureModel.CurrentValue[j]},{WTMeasureModel.PowerValue[j]}");
            }
            sw.Flush();
            sw.Close();
        }

        void UpdateData(object sender, EventArgs e) {
            if (nextDataIndex >= WTMeasureModel.CurrentValue.Count) {
                throw new OverflowException("data array isn't long enough to accomodate new data");
                // in this situation the solution would be:
                //   1. clear the plot
                //   2. create a new larger array
                //   3. copy the old data into the start of the larger array
                //   4. plot the new (larger) array
                //   5. continue to update the new array
            }

            double randomValue = Math.Round(rand.NextDouble() - .5, 3);
            double latestValue = WTMeasureModel.CurrentValue[nextDataIndex - 1] + randomValue;
            WTMeasureModel.CurrentValue[nextDataIndex] = latestValue;
            signalPlot.MaxRenderIndex = nextDataIndex;
            //ReadingsTextbox.Text = $"{nextDataIndex + 1}";
            //LatestValueTextbox.Text = $"{latestValue:0.000}";
            nextDataIndex += 1;
        }

        void Render(object sender, EventArgs e) {
            tILine.Plot.AxisAuto();
            tILine.Refresh();
        }

    }
}
