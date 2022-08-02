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
                $"https://api.openweathermap.org/data/2.5/forecast/daily?lat={lat}&lon={lon}&cnt=5&appid=c2cae76a00fc16fe82a39fcd23360dcf"; // TODO: Extract the API key
            client = new RestClient(requestUrl);
            request = new RestRequest();
            RestResponse responseForecast = client.Execute(request);
        }
    }
}