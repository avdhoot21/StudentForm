using StudentDetails.Views;
using Xamarin.Forms;

namespace StudentDetails
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new StudentsPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
