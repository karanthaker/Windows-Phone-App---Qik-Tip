//Karan Thaker
//karan.thaker@hotmail.com



using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace TipCalculator
{
    public partial class MainPage : PhoneApplicationPage
    {
        TipController _TipController = new TipController();
        string _strSeparator = ".";

        const decimal TIP_DEFAULT_VALUE = 0.15M;
        const decimal TIP_CHANGE_DEFAULT = 0.01M;
        const bool TIP_DEFAULT_ROUNDING_OPTION = true;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;

            _strSeparator = System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;

            this.OrientationChanged += new EventHandler<OrientationChangedEventArgs>(MainPage_OrientationChanged);

            ResetValues();

            // Set up all button handlers for input.
            InitButtonHandlers();
        }



        /// <summary>
        /// Invoked when the orientation of the display has been changed.  This will allow us
        /// to move our controls around to take advantage of landscape mode and back to portrait.
        /// </summary>
        void MainPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            // In landscape mode, the totals grid is moved to the right on the screen
            // which puts it in row 1, column 1.
            if ((e.Orientation & PageOrientation.Landscape) != 0)
            {
                LandscapeColumn.Width = GridLength.Auto;
                Grid.SetRow(TotalsGrid, 1);
                Grid.SetColumn(TotalsGrid, 1);

                LayoutRoot.ColumnDefinitions[1].Width = GridLength.Auto;
            }
            // In portrait mode, the totals grid goes below the number pad at the
            // bottom of the screen which is row 0, column 2.
            else
            {
                LandscapeColumn.Width = new GridLength(0);
                Grid.SetRow(TotalsGrid, 2);
                Grid.SetColumn(TotalsGrid, 0);

                LayoutRoot.ColumnDefinitions[1].Width = new GridLength(0);
            }
        }

        /// <summary>
        /// Set the same button handler for each of the numeric keys.  Each instance
        /// is mapped to is constant value so we can handle all number keys the same way.
        /// </summary>
        private void InitButtonHandlers()
        {
            for (int i = 0; i <= 9; i++)
            {
                string btnName = "button" + i.ToString();
                System.Windows.Controls.Button btn = 
                    (System.Windows.Controls.Button)this.LayoutRoot.FindName(btnName);

                int j = i; // avoid local variable capture
                btn.Click += new RoutedEventHandler(
                    (object sender, RoutedEventArgs e) => { TryUpdateBillAmount(textBlockBill.Text + j.ToString()); }
                );
            }
        }

        /// <summary>
        /// Add a separator character to the bill amount to track fractional values.
        /// </summary>
        private void buttonSep_Click(object sender, RoutedEventArgs e)
        {
            TryUpdateBillAmount(textBlockBill.Text + _strSeparator);
        }


        /// <summary>
        /// Take a new candidate value for the bill amount and validate the amount.
        /// If it is a valid value, then update the model and display values.
        /// </summary>
        /// <param name="strBillAmount">Bill amount to validate</param>
        private void TryUpdateBillAmount(string strBillAmount)
        {
            decimal NewBillAmount = 0M;
            if (String.IsNullOrEmpty(strBillAmount) ||
                System.Decimal.TryParse(strBillAmount, out NewBillAmount) == true)
            {
                _TipController.UpdateBillAmount(NewBillAmount);
                RefreshControlValues();
                textBlockBill.Text = strBillAmount;
            }
        }

        /// <summary>
        /// Delete the last character from the text buffer and attempt an update.  If
        /// successful, the field is permanently upated along with totals.
        /// </summary>
        private void buttonDel_Click(object sender, RoutedEventArgs e)
        {
            if (textBlockBill.Text.Length > 0)
            {
                string strBillAmount = textBlockBill.Text.Substring(0, textBlockBill.Text.Length - 1);
                TryUpdateBillAmount(strBillAmount);
            }
        }

        /// <summary>
        /// Reduce the tip amount by one whole increment (no partial values allowed).
        /// If successful, update the model and display values.
        /// </summary>
        private void buttonTipLower_Click(object sender, RoutedEventArgs e)
        {
            decimal TipPercent = _TipController.GetTipPercentage();
            if (TipPercent > System.Decimal.Zero)
            {
                TipPercent = TipPercent - TIP_CHANGE_DEFAULT;
                _TipController.UpdateTipPercentage(TipPercent);
                RefreshControlValues();
            }
        }

        /// <summary>
        /// Increment the tip amount by one whole increment (no partial values allowed).
        /// If successful, update the model and display values.
        /// </summary>
        private void buttonTipHigher_Click(object sender, RoutedEventArgs e)
        {
            decimal TipPercent = _TipController.GetTipPercentage();
            TipPercent = TipPercent + TIP_CHANGE_DEFAULT;
            _TipController.UpdateTipPercentage(TipPercent);
            RefreshControlValues();
        }

        /// <summary>
        /// Updates the value in all controls based on the model data.
        /// </summary>
        private void RefreshControlValues()
        {
            decimal tipPercent = _TipController.GetTipPercentage() * 100;
            textBlockTipPercent.Text = TipController.FormatNumberForDisplay(tipPercent, 0) + "%";
            textBlockTipAmount.Text = TipController.FormatNumberForDisplay(_TipController.GetTipAmount());
            textBlockTotal.Text = TipController.FormatNumberForDisplay(_TipController.GetTotal());
        }

        /// <summary>
        /// Changes status of rounding option.  Need to recalculate the totals
        /// when the option changes.
        /// </summary>
        private void toggleControlSwitchRoundTotal_Click(object sender, RoutedEventArgs e)
        {
            bool bRound = false;

            if (toggleControlSwitchRoundTotal.IsChecked.HasValue)
                bRound = toggleControlSwitchRoundTotal.IsChecked.Value;
            _TipController.UpdateRoundingOption(bRound);
            RefreshControlValues();
        }


        private void ResetValues()
        {
            // Set the default tip value and rounding value.
            _TipController.UpdateTipPercentage(TIP_DEFAULT_VALUE);
            _TipController.UpdateRoundingOption(TIP_DEFAULT_ROUNDING_OPTION);
            _TipController.UpdateBillAmount(0.0M);
            toggleControlSwitchRoundTotal.IsChecked = TIP_DEFAULT_ROUNDING_OPTION;
            RefreshControlValues();       
            textBlockBill.Text = "";        
        }

        private void appbar_buttonClear_Click(object sender, EventArgs e)
        {
            ResetValues();
        }

    }
}
