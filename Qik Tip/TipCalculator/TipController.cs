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
    public class TipController
    {
        private TipModel _Model = new TipModel();

        public decimal GetBillAmount() { return _Model.BillAmount; }
        public decimal GetTipPercentage() { return _Model.TipPercentage; }
        public decimal GetTipAmount() { return _Model.TipAmount; }
        public decimal GetTotal() { return _Model.Total; }
        public bool GetRoundTotal() { return _Model.RoundTotal; }

        /// <summary>
        /// Update the bill amount we are calcuating tax on.
        /// </summary>
        /// <param name="NewAmount">New bill amount</param>
        public void UpdateBillAmount(decimal NewAmount)
        {
            _Model.BillAmount = NewAmount;
            RefreshTipAndTotal();
        }

        /// <summary>
        /// Update the tip percentage to allocate for the bill.
        /// </summary>
        /// <param name="NewAmount">New tip percentage amount</param>
        public void UpdateTipPercentage(decimal NewAmount)
        {
            _Model.TipPercentage = NewAmount;
            RefreshTipAndTotal();
        }

        /// <summary>
        /// Update the rounding option. If true, the total bill will
        /// be rounded to an even value.  For false, no rounding is done.
        /// </summary>
        /// <param name="RoundTotal"></param>
        public void UpdateRoundingOption(bool RoundTotal)
        {
            _Model.RoundTotal = RoundTotal;
            RefreshTipAndTotal();
        }

        private void RefreshTipAndTotal()
        {
            decimal NewTipAmount = System.Decimal.Round((_Model.BillAmount * _Model.TipPercentage), 2);
            decimal NewTotal = System.Decimal.Round((_Model.BillAmount + NewTipAmount), 2);

            if (_Model.RoundTotal)
            {
                decimal whole = System.Decimal.Truncate(NewTotal);
                decimal fraction = NewTotal - whole;

                if (fraction < 0.50M)
                {
                    NewTipAmount = NewTipAmount - fraction;
                    if (NewTipAmount < System.Decimal.Zero)
                        NewTipAmount = 0M;
                }
                else
                {
                    decimal RoundAmount = (whole + 1) - NewTotal;
                    NewTipAmount += RoundAmount;
                }
            }

            _Model.TipAmount = NewTipAmount;
            _Model.Total = System.Decimal.Round((_Model.BillAmount + _Model.TipAmount), 2);
        }



        public static string FormatNumberForDisplay(decimal Amount, int decimals = 2)
        {
            string str = String.Format("{0}", System.Decimal.Round(Amount, decimals));
            return (str);
        }

    }
}
