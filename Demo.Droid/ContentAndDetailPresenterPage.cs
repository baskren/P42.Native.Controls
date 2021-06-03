using System;
using System.Collections.ObjectModel;
using P42.Native.Controls;

namespace Demo.Droid
{
    public class ContentAndDetailPresenterPage : Page
    {

        ObservableCollection<string> items = new ObservableCollection<string>
        {
                "1 one", "2 two", "3 three", "4 four", "5 five", "6 six", "7 seven", "8 eight", "9 nine", "10 ten", "11 eleven", "12 twelve", "13 thirteen", "14 fourteen", "15 fifteen", "16 sixteen", "17 seventeen", "18 eighteen", "19 nineteen", "20 twenty", "21 twenty one", "22 twenty two", "23 twenty three", "24 twenty four", "25 twenty five", "26 twenty six",
                "Item A", "Item B", "Item C", "Item D", "Item E", "Item F", "Item G", "Item H", "Item I", "Item J", "Item K", "Item L", "Item M", "Item N", "Item O", "Item P", "Item Q", "Item R", "Item S", "Item T", "Item U", "Item V", "Item W", "Item X", "Item Y", "Item Z",
                "Item A1", "Item B1", "Item C1", "Item D1", "Item E1", "Item F1", "Item G1", "Item H1", "Item I1", "Item J1", "Item K1", "Item L1", "Item M1", "Item N1", "Item O1", "Item P1", "Item Q1", "Item R1", "Item S1", "Item T1", "Item U1", "Item V1", "Item W1", "Item X1", "Item Y1", "Item Z1",
        };

        ListView _listView;

        public ContentAndDetailPresenterPage()
        {
            DipContent = new ListView()
                .Assign(out _listView)
                .ItemViewType(typeof(TextCell))
                .ItemsSource(items)
                .SelectionMode(SelectionMode.Single);

            _listView.DipSelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"ContentAndDetailPresenterPage.OnSelectionChanged: " + e.ToString());
        }
    }
}
