using StudentDetails.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StudentDetails.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentPage : ContentPage
    {

        private readonly int _studentID;
        public StudentPage(int studentId)
        {
            _studentID = studentId;
            InitializeComponent();
            StudentDTO studentDTO = new StudentDTO();
            studentDTO.Genders = new List<GenderModel>();
            studentDTO.Student = new StudentModel();
            new StudentBusinessLogic().GetStudent(studentDTO);
            cmbGender.ItemsSource = studentDTO.Genders;
            if (_studentID > 0)
            {
                lblHeader.Text = "Edit Student";
                btnDelete.IsVisible = true;
                studentDTO.Student = new StudentModel();
                studentDTO.Student.StudentID = _studentID;
                new StudentBusinessLogic().GetStudent(studentDTO);
                txtFirstName.Text = studentDTO.Student.FirstName;
                txtLastName.Text = studentDTO.Student.LastName;
                cmbGender.SelectedIndex = studentDTO.Student.GenderID;
                txtAge.Text = Convert.ToString(studentDTO.Student.Age);
                dtpDOB.Date = Convert.ToDateTime(studentDTO.Student.DateOfBirth.Date);
                txtClass.Text = studentDTO.Student.Class;
                txtAddress.Text = studentDTO.Student.Address;
            }
            else
            {
                lblHeader.Text = "Add Student";
                btnDelete.IsVisible = false;
                //cmbGender.SelectedItem = null;
            }
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new StudentsPage());
        }

        private void btnSave_Clicked(object sender, EventArgs e)
        {
            if (IsValidation())
            {
                StudentDTO studentDTO = new StudentDTO();
                studentDTO.Student = new StudentModel();
                studentDTO.Student.StudentID = _studentID;
                studentDTO.Student.FirstName = txtFirstName.Text.Trim();
                studentDTO.Student.LastName = txtLastName.Text.Trim();
                studentDTO.Student.GenderID = Convert.ToInt32(cmbGender.SelectedIndex);
                studentDTO.Student.DateOfBirth = dtpDOB.Date;
                studentDTO.Student.Age = Convert.ToInt32(txtAge.Text.Trim());
                studentDTO.Student.Class = txtClass.Text;
                studentDTO.Student.Address = txtAddress.Text;
                StudentBusinessLogic studentBusinessLogic = new StudentBusinessLogic();
                studentBusinessLogic.SaveStudent(studentDTO);
                Navigation.PushModalAsync(new StudentsPage());
            }
        }

        private bool IsValidation()
        {
            bool isValid = true;
            if (txtFirstName.Text == null)
            {
                lblFirstNameWarn.IsVisible = true;
                lblFirstNameWarn.Text = "This Field is Required";
                isValid = false;
            }
            else if (txtFirstName.Text.Trim().Length < 3 || txtFirstName.Text.Trim().Length > 15)
            {
                lblFirstNameWarn.IsVisible = true;
                lblFirstNameWarn.Text = "minimum 3 and maximum 15 characters allowed";
                isValid = false;
            }
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                lblLastNameWarn.IsVisible = true;
                lblLastNameWarn.Text = "This Field is Required";
                isValid = false;
            }
            else if (txtLastName.Text.Trim().Length < 2 || txtLastName.Text.Trim().Length > 18)
            {
                lblLastNameWarn.IsVisible = true;
                lblLastNameWarn.Text = "minimum 2 and maximum 18 characters allowed";
                isValid = false;
            }
            if (cmbGender.SelectedIndex < 0)
            {
                lblGenderWarn.IsVisible = true;
                lblGenderWarn.Text = "This Field is required";
                isValid = false;
            }
            if (string.IsNullOrEmpty(txtAge.Text))
            {
                lblAgeWarn.IsVisible = true;
                lblAgeWarn.Text = "This Field is Required";
                isValid = false;
            }
            else if (Convert.ToInt32(txtAge.Text) < 5 || Convert.ToInt32(txtAge.Text) > 99)
            {
                lblAgeWarn.IsVisible = true;
                lblAgeWarn.Text = "Age should be between 5-99";
                isValid = false;
            }
            return isValid;
        }

        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            String val = entry.Text; //Get Current Text
            var regex = new Regex("^[a-zA-Z ]*$");

            foreach (var str in val)
            {
                if (!regex.IsMatch(val))
                {
                    val = val.Remove(val.Length - 1);
                    txtFirstName.Text = val;
                    lblFirstNameWarn.IsVisible = true;
                    lblFirstNameWarn.Text = "Only characters required";
                    break;
                }

                if (!string.IsNullOrEmpty(val))
                {
                    lblFirstNameWarn.IsVisible = false;
                }
            }
        }

            private void txtLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry entry = sender as Entry;
            String val = entry.Text; //Get Current Text
            var regex = new Regex("^[a-zA-Z ]*$");
            foreach (var str in val)
            {
                if (!regex.IsMatch(val))
                {
                    val = val.Remove(val.Length - 1);
                    txtLastName.Text = val;
                    lblLastNameWarn.IsVisible = true;
                    lblLastNameWarn.Text = "Only characters required";
                    break;
                }

                if (!string.IsNullOrEmpty(val))
                {
                    lblLastNameWarn.IsVisible = false;
                }
            }
        }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGender.SelectedIndex > -1)
            {
                lblGenderWarn.IsVisible = false;
            }
        }

        private void dtpDOB_DateSelected(object sender, DateChangedEventArgs e)
        {
            int currentAge = DateTime.Today.Year - dtpDOB.Date.Year;
            txtAge.Text = currentAge.ToString();
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Question Alert Title", "Are You Sure you want to Delete this student Record ", "Yes", "No");
            if (result)
            {
                StudentDTO studentDTO = new StudentDTO();
                studentDTO.Student = new StudentModel();
                studentDTO.Student.StudentID = _studentID;
                StudentBusinessLogic MyLogic = new StudentBusinessLogic();
                MyLogic.DeleteStudent(studentDTO);
            }
            await Navigation.PushModalAsync(new StudentsPage());
        }

        private void txtAge_TextChanged(object sender, TextChangedEventArgs e)
        {
            dtpDOB.Date = DateTime.Now.AddYears(-Convert.ToInt32(txtAge.Text));
            if (!string.IsNullOrEmpty(txtAge.Text.Trim()))
            {
                lblAgeWarn.IsVisible = false;
            }
        }
    }

}