using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;

namespace DatabaseTableExplorer;

public partial class MainPage : ContentPage
{
    private string connectionString = "server=localhost;user=mysql;password=root;";
    public ObservableCollection<string> Databases { get; } = new();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        try
        {
            Databases.Clear();
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand("SHOW DATABASES;", connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Databases.Add(reader.GetString(0));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}