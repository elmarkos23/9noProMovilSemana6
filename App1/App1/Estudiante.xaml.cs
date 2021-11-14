using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class Estudiante : ContentPage
  {
    public HttpClient Client = new HttpClient();
    Modelo.Estudiante estudiante = new Modelo.Estudiante();
    public Estudiante(Modelo.Estudiante _estudiante)
    {
      InitializeComponent();
      estudiante = _estudiante;
      if (estudiante.codigo == 0)
      {
        this.Title = "Nuevo estudiante";
        txtCodigo.Text = string.Empty;
        txtCodigo.IsEnabled = true;
        txtNombre.Text = estudiante.nombre;
        txtApellido.Text = estudiante.apellido;
        txtEdad.Text = estudiante.edad.ToString();
        this.btnGrabar.Text = "Grabar";
      }
      else
      {
        this.Title = "Editar estudiante";
        txtCodigo.Text = estudiante.codigo.ToString();
        txtCodigo.IsEnabled = false;
        txtNombre.Text = estudiante.nombre;
        txtApellido.Text = estudiante.apellido;
        txtEdad.Text = estudiante.edad.ToString();
        this.btnGrabar.Text = "Actualizar";
      }
    }

    private async void Grabar_Clicked(object sender, EventArgs e)
    {
      if (estudiante.codigo.Equals(0))//nuevo
      {
        estudiante.codigo = Convert.ToInt32(txtCodigo.Text);
        estudiante.nombre = txtNombre.Text;
        estudiante.apellido = txtApellido.Text;
        estudiante.edad = Convert.ToInt32(txtEdad.Text);
        Insertar(estudiante);
        await Navigation.PopAsync();
      }
      else//actualizar
      {
        estudiante.nombre = txtNombre.Text;
        estudiante.apellido = txtApellido.Text;
        estudiante.edad = Convert.ToInt32(txtEdad.Text);
        ActualizarUsuario(estudiante);
        await Navigation.PopAsync();
      }
    }
    public int Insertar(Modelo.Estudiante estudiante)
    {
      int resultado = 0;
      try
      {
        var Client = new HttpClient();
        var parametros = new Dictionary<string, string>();
        parametros.Add("codigo", estudiante.codigo.ToString());
        parametros.Add("nombre", estudiante.nombre);
        parametros.Add("apellido", estudiante.apellido);
        parametros.Add("edad", estudiante.edad.ToString());

        Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        var response = Client.PostAsync("http://192.168.100.32:81/moviles/post.php", new FormUrlEncodedContent(parametros)).Result;
        if (response.IsSuccessStatusCode)
        {
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
    public  bool ActualizarUsuario(Modelo.Estudiante estudiante)
    {
      bool resultado = false;
      try
      {
        var Client = new HttpClient();
        var response = Client.PutAsync("http://192.168.100.32:81/moviles/post.php?codigo=" + estudiante.codigo + "&nombre=" + estudiante.nombre + "&apellido=" + estudiante.apellido + "&edad=" + estudiante.edad + "", null).Result;
        if (response.IsSuccessStatusCode)
        {
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
  }
}