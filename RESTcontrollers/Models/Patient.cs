using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTcontrollers
{
    public class Patient
    {
        private int id;
        private string firstname;
        private string lastname;
        private Gender gender;
        private DateTime birth;

        public Patient(int id, string firstname, string lastname, Gender gender, DateTime birth)
        {
            this.Id = id;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Gender = gender;
            this.Birth = birth;
        }
        public Patient(string firstname, string lastname, Gender gender, DateTime birth)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Gender = gender;
            this.Birth = birth;
        }

        public int Id { get => id; set => id = value; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public DateTime Birth { get => birth; set => birth = value; }
        internal Gender Gender { get => gender; set => gender = value; }

        public Patient GetPatientFromString(string input)
        {
            string[] array = input.Split(',');
            return new Patient(int.Parse(array[0]), array[1], array[2], (Gender)Enum.Parse(typeof(Gender), array[3]), DateTime.Parse(array[4]));
        }
        public override string ToString()
        {
            return $"ID: {this.Id}\nFirstname: {this.Firstname}\nLastname: {this.Lastname}\nGender: {this.Gender.ToString().ToLower()}\nBirth: {this.Birth}";
        }
    }
}
