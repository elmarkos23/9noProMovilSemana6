using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
  public partial class MainPage : ContentPage
  {
    public HttpClient Client = new HttpClient();
    List<Modelo.Estudiante> resultado = new List<Modelo.Estudiante>();
    public MainPage()
    {
      InitializeComponent();
    }
    protected override async void OnAppearing()
    {
      base.OnAppearing();
      Select();
    }
    public async void Select()
    {
      try
      {
        var Uri = "http://192.168.100.32:81/moviles/post.php";
        HttpResponseMessage response = await Client.GetAsync(Uri);
        if (response.IsSuccessStatusCode)
        {
          string PlacesJson = response.Content.ReadAsStringAsync().Result;
          if (PlacesJson.Length > 0)
          {
            resultado = JsonConvert.DeserializeObject<List<Modelo.Estudiante>>(PlacesJson);
          }
        }
        else
        {
          //error en el servicio
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
      lvDatos.ItemsSource = null;
      lvDatos.ItemsSource = resultado;
    }

    private async void Insertar_Clicked(object sender, EventArgs e)
    {
      await Navigation.PushAsync(new Estudiante(new Modelo.Estudiante()));
    }


    private async void Eliminar_Clicked(object sender, EventArgs e)
    {
      try
      {
        var estudiante = lvDatos.SelectedItem as Modelo.Estudiante;
        bool answer = await DisplayAlert("Consulta?", "Desea eliminar el estudiante=" + estudiante.codigo.ToString(), "Yes", "No");
        if (answer)
        {
          //eliminar
          Eliminar(estudiante.codigo);
          //cargar de nuevo
          Select();
        }

      }
      catch
      {
        await DisplayAlert("Error", "Seleccione un estudiante", "Aceptar");
      }
    }
    public bool Eliminar(int codigo)
    {
      bool resultado = false;
      try
      {
        var Client = new HttpClient();
        var response = Client.DeleteAsync("http://192.168.100.32:81/moviles/post.php?codigo=" + codigo).Result;
        if (response.IsSuccessStatusCode)
        {
          string PlacesJson = response.Content.ReadAsStringAsync().Result;
          resultado = true;
        }
        else
        {
          // resultado diferente de 200 o succesfull
        }

      }
      catch (Exception ex)
      {
        //error
      }
      return resultado;
    }
    private async void Actualizar_Clicked(object sender, EventArgs e)
    {
      try
      {
        var estudiante = lvDatos.SelectedItem as Modelo.Estudiante;
        await Navigation.PushAsync(new Estudiante(estudiante));
      }
      catch
      {
        await DisplayAlert("Error", "Seleccione un estudiante", "Aceptar");
      }
    }
  }
}
