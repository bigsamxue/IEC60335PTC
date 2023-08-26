using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEC60335Develop.Models {
    public class WTMeasureModel {
        //public double[] VoltageValue { get; set; }
        //public double[] CurrentValue { get; set; }
        //public double[] PowerValue { get; set; }
        public List<double> VoltageValue { get; set; }
        public List<double> CurrentValue { get; set; }
        public List<double> PowerValue { get; set; }
        public string PowerMaxValue { get; set; }

    }
}
