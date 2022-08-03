using System.Text.Json;
using RestSharp;

namespace WeatherApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnSearch;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchString = tbxInput.Text;
            string requestUrl =
                $"http://api.openweathermap.org/geo/1.0/direct?q={searchString}&limit=5&appid=c2cae76a00fc16fe82a39fcd23360dcf"; // TODO: Extract the API key
            RestClient client = new RestClient(requestUrl);
            RestRequest request = new RestRequest();
            RestResponse response = client.Execute(request);

            List<CoordinateInfo> coordinateInfos = JsonSerializer.Deserialize<List<CoordinateInfo>>(response.Content);
            dgwSearchResults.DataSource = coordinateInfos;

            dgwSearchResults.RowHeadersVisible = false;
            dgwSearchResults.Columns["Lat"].Visible = false;
            dgwSearchResults.Columns["Lon"].Visible = false;
            dgwSearchResults.ClearSelection();
        }

        private void dgwSearchResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgwSearchResults.Rows[e.RowIndex];
            double lat = Convert.ToDouble(row.Cells["Lat"].Value);
            double lon = Convert.ToDouble(row.Cells["Lon"].Value);

            // Current day
            string requestUrl =
                $"https://api.openweathermap.org/data/2.5/weather?units=metric&lat={lat}&lon={lon}&appid=c2cae76a00fc16fe82a39fcd23360dcf"; // TODO: Extract the API key
            RestClient client = new RestClient(requestUrl);
            RestRequest request = new RestRequest();
            RestResponse responseDay = client.Execute(request);

            // Forecast
            requestUrl =
                $"https://api.openweathermap.org/data/2.5/forecast?units=metric&lat={lat}&lon={lon}&appid=c2cae76a00fc16fe82a39fcd23360dcf"; // TODO: Extract the API key
            client = new RestClient(requestUrl);
            request = new RestRequest();
            RestResponse responseForecast = client.Execute(request);

            PopulateFiveDayForecast(responseForecast);
        }

        private void PopulateFiveDayForecast(RestResponse response)
        {
            if (response.Content != null)
            {
                FiveDaysForecast? forecast = JsonSerializer.Deserialize<FiveDaysForecast>(response.Content);
                TabPage[] tabs = { tabPage0, tabPage1, tabPage2, tabPage3, tabPage4 };
                DataGridView[] dgws = { dgwDay0, dgwDay1, dgwDay2, dgwDay3, dgwDay4 };

                for (int i = 0; i < 5; i++)
                {
                    DateTime tabDay = DateTime.Today.AddDays(i);
                    tabs[i].Text = tabDay.ToString("d MMMM dddd");
                }

                foreach (DataGridView dataGridView in dgws)
                {
                    dataGridView.Columns.Clear();
                    for (int i = 0; i < 24; i += 3)
                    {
                        dataGridView.Columns.Add(i.ToString(), i.ToString() + ":00");
                    }

                    dataGridView.Rows.Clear();
                    int rowIndex = dataGridView.Rows.Add();
                    dataGridView.Rows[rowIndex].HeaderCell.Value = "Temp (C°)";
                    rowIndex = dataGridView.Rows.Add();
                    dataGridView.Rows[rowIndex].HeaderCell.Value = "Feels Like";
                    rowIndex = dataGridView.Rows.Add();
                    dataGridView.Rows[rowIndex].HeaderCell.Value = "Humidity";
                    rowIndex = dataGridView.Rows.Add();
                    dataGridView.Rows[rowIndex].HeaderCell.Value = "Wind Speed (m/s)";
                }

                DateTime today = DateTime.Parse(forecast.List[0].Date);
                foreach (ThreeHourlyForecast threeHourlyForecast in forecast.List)
                {
                    DateTime currentDateTime = DateTime.Parse(threeHourlyForecast.Date);
                    int dayIndex = (currentDateTime.Day - today.Day);
                    if (dayIndex > 4)
                    {
                        break;
                    }

                    DataGridView dgw = dgws[dayIndex];
                    dgw.Rows[0].Cells[currentDateTime.Hour.ToString()].Value = threeHourlyForecast.Main.Temp;
                    dgw.Rows[1].Cells[currentDateTime.Hour.ToString()].Value = threeHourlyForecast.Main.FeelsLike;
                    dgw.Rows[2].Cells[currentDateTime.Hour.ToString()].Value = threeHourlyForecast.Main.Humidity;
                    dgw.Rows[3].Cells[currentDateTime.Hour.ToString()].Value = threeHourlyForecast.Wind.Speed;
                }
            }
        }
    }
}