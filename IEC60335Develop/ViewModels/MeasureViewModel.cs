using IEC60335Develop.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEC60335Develop.ViewModels {
    public class MeasureViewModel : BindableBase {
        private WTMeasureModel _wTMeasureModel;

        public WTMeasureModel WTMeasureModel {
            get { return _wTMeasureModel; }
            set { SetProperty(ref _wTMeasureModel, value); }
        }

        public List<string> Time { get; set; }

        public DelegateCommand StartCommand { get; set; }

        public DelegateCommand StopCommand { get; set; }

        public DelegateCommand ResetCommand { get; set; }

        public DelegateCommand OriginalDataOutputCommand { get; set; }


        //1.开始store；2.实时获取U、I、P、Pmax，存入List；3.实时绘制
        public void StartWT() {
            List<double> voltage = new List<double>();
            List<double> current = new List<double>();
            List<double> power = new List<double>();
            List<double> powerMax = new List<double>();

            ConnectionViewModel.WT1800.RemoteCTRL(":HSPEED:START");
            var value = ConnectionViewModel.WT1800.RemoteCTRL(":NUMeric:HSPeed:VALue?");
            List<string> UIPvalue = new List<string>();
            UIPvalue = value.Split(',');
            for (int i = 0; i < 300; i += 3) {
                voltage.Add(value.Split(","));
            }


        }

        //1.停止Store；2.清空List
        public void StopWT() {

        }

        public void ResetDisplay() {

        }

        public MeasureViewModel() {
            WTMeasureModel = new WTMeasureModel();
            WTMeasureModel.WTValueDict = new Dictionary<string, List<double>>();
        }
    }
}
