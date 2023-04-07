using Imaginator.App.Data;
using Imaginator.App.Pages;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using OpenAI.GPT3.Extensions;
using System.Configuration;

namespace Imaginator.App {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            AppConstants.MainFrm = this;
            var apikey = ConfigurationManager.AppSettings["ApiKey"];
            var orgid = ConfigurationManager.AppSettings["OrgId"];
            var services = new ServiceCollection();
            services.AddOpenAIService(settings => { settings.ApiKey = apikey; settings.Organization = orgid; });

            services.AddWindowsFormsBlazorWebView();
            services.AddMudServices(config => {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 10000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });
            blazorWebView1.HostPage = "wwwroot\\index.html";
            blazorWebView1.Services = services.BuildServiceProvider();
            blazorWebView1.RootComponents.Add<App>("#app");
            blazorWebView1.RootComponents.Add<HeadOutlet>("head::after");
            this.FormClosed += (a, b) => { Environment.Exit(0); };
        }
    }
}