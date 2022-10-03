using StudentDetails.Models;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudentDetails.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentsPage : ContentPage
    {
        public StudentsPage()
        {
            InitializeComponent();
            StudentDTO studentDTO = new StudentDTO();
            studentDTO.Genders = new List<GenderModel>();
            studentDTO.Student = new StudentModel();
            new StudentBusinessLogic().GetStudents(studentDTO);
            studentListView.ItemsSource = studentDTO.Students;
        }

        private void btnAdd_Clicked(object sender, EventArgs e)
        {
            StudentDTO studentDTO = new StudentDTO();
            studentDTO.Student = new StudentModel();
            Navigation.PushModalAsync(new StudentPage(studentDTO.Student.StudentID));
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            StudentDTO studentDTO = new StudentDTO();
            studentDTO.Student = new StudentModel();
            studentDTO.SearchStudents = txtSearch.Text;
            new StudentDataAccess().GetStudents(studentDTO);
            studentListView.ItemsSource = studentDTO.Students;
        }

        private void studentListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            StudentDTO studentDTO = new StudentDTO();
            studentDTO.Student = new StudentModel();
            var item1 = e.Item as StudentModel;
            Navigation.PushModalAsync(new StudentPage(item1.StudentID));
        }
    }
}