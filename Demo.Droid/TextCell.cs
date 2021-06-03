using System;
using P42.Native.Controls;

#if __ANDROID__
using Grid = Android.Widget.GridLayout;
using Label = Android.Widget.TextView;
using Color = Android.Graphics.Color;
#endif

namespace Demo.Droid
{
    public class TextCell : P42.Native.Controls.Cell
    {
        
        Label _titleLabel = new P42.Native.Controls.Label
        {
            DipFontStyle = FontStyle.Bold,
            DipVerticalAlignment = Alignment.Center,
            DipHorizontalAlignment = Alignment.Start
        };
        public Label _valueLabel;
        Grid _grid;

        public TextCell(ListView listView) : base(listView)
        {


#if __ANDROID__

            //titleLabel = new Label(Context);
            _valueLabel = new Label(Context);
            _titleLabel.SetBackgroundColor(Color.Pink);
            _titleLabel.SetTextColor(Color.Black);
            //valueLabel.Gravity = Android.Views.GravityFlags.Right;

            _grid = new Grid(Context)
            {
                RowCount = 2,
                ColumnCount = 1,
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
            };

            _titleLabel.LayoutParameters = new Grid.LayoutParams(
                Grid.InvokeSpec(0),
                Grid.InvokeSpec(0));
            _valueLabel.LayoutParameters = new Grid.LayoutParams(
                Grid.InvokeSpec(1, Grid.Center),
                Grid.InvokeSpec(0, Grid.RightAlighment));

            _grid.AddView(_titleLabel);
            _grid.AddView(_valueLabel);

            AddView(_grid);
#endif

        }


        bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _grid.RemoveAllViews();
                _grid.Dispose();
                _valueLabel.Dispose();
                _titleLabel.Dispose();
            }
            base.Dispose(disposing);
        }

        public override void DipOnDataContextChanged()
        {
            base.DipOnDataContextChanged();

            _titleLabel.Text = DipDataContext.ToString();
            _valueLabel.Text = DipDataContext.ToString();

            //System.Diagnostics.Debug.WriteLine($"TextCell OnDataContextChanged : " + DipDataContext.ToString()) ;
        }
    }
}
