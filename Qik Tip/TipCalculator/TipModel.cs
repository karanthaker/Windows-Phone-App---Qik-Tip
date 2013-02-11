//Karan Thaker
//karan.thaker@hotmail.com


using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TipCalculator
{
    public class TipModel
    {
        public decimal  BillAmount;
        public decimal  TipPercentage;
        public decimal  TipAmount;
        public decimal  Total;
        public bool     RoundTotal;
    }
}
