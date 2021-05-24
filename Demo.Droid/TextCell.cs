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
        
        Label titleLabel = new P42.Native.Controls.Label
        {
            FontStyle = FontStyle.Bold,
            VerticalAlignment = Alignment.Center,
            HorizontalAlignment = Alignment.Start
        };
        /*
        Label valueLabel = new Label
        {
            VerticalAlignment = Alignment.Center,
            HorizontalAlignment = Alignment.End
        };
        */

        //Label titleLabel;
        Label valueLabel;

        public TextCell(ListView listView) : base(listView)
        {


#if __ANDROID__

            //titleLabel = new Label(Context);
            valueLabel = new Label(Context);
            titleLabel.SetBackgroundColor(Color.Pink);
            titleLabel.SetTextColor(Color.Black);
            //valueLabel.Gravity = Android.Views.GravityFlags.Right;

            var grid = new Grid(Context)
            {
                RowCount = 2,
                ColumnCount = 1,
                LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent)
            };

            titleLabel.LayoutParameters = new Grid.LayoutParams(
                Grid.InvokeSpec(0),
                Grid.InvokeSpec(0));
            valueLabel.LayoutParameters = new Grid.LayoutParams(
                Grid.InvokeSpec(1, Grid.Center),
                Grid.InvokeSpec(0, Grid.RightAlighment));

            grid.AddView(titleLabel);
            grid.AddView(valueLabel);

            AddView(grid);
#endif

        }


        public override void OnDataContextChanged()
        {
            base.OnDataContextChanged();

            titleLabel.Text = DataContext.ToString();
            valueLabel.Text = DataContext.ToString();

            System.Diagnostics.Debug.WriteLine($"TextCell OnDataContextChanged : " + DataContext.ToString()) ;
        }
    }
}
