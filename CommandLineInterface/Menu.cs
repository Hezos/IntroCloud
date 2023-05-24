using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineInterface
{
    internal class Menu
    {
        private string[] elements;

        public Menu(string[] elements)
        {
            this.elements = elements;
        }

        public int ReadMenu()
        {
            Console.WriteLine("Menu:\n");
            for (int i = 0; i < this.elements.Length; i++)
            {
                Console.WriteLine($"\t{i + 1}. {this.elements[i]}");
            }
            Console.WriteLine("\nPlease select an action!");
            bool valid;
            int readNumber = 0;
            do
            {
                valid = true;
                try
                {
                    readNumber = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    valid = false;
                }

                if (!valid)
                {
                    valid = false;
                    Console.WriteLine("\tIt's not a number!");
                    Console.WriteLine("Enter it again!");
                }
                else
                {
                    if (readNumber > this.elements.Length || readNumber < 1)
                    {
                        valid = false;
                        Console.WriteLine("\tMenu item does not exist!");
                        Console.WriteLine("Enter it again!");
                    }
                }
            } while (!valid);
            Console.Write("\n");
            return readNumber;
        }

        public static Patient ReadNewPatient()
        {
            Console.WriteLine("\nPlease add a new patient!");
            Console.Write("\tFirstname: ");
            string firstname = Console.ReadLine();
            Console.Write("\tLastname: ");
            string lastname = Console.ReadLine();
            string genderString = "";
            bool genderIsValid = false;
            bool genderFirstTry = true;
            do
            {
                if (!genderFirstTry)
                {
                    Console.WriteLine("\tGender is not valid! Enter it again!");
                }
                Console.Write("\tGender(male/female/other): ");
                genderString = Console.ReadLine();
                if (genderString.ToLower() == "male" || genderString.ToLower() == "female" || genderString.ToLower() == "other")
                {
                    genderIsValid = true;
                }
                genderFirstTry = false;
            } while (!genderIsValid);
            Gender gender = Gender.Other;
            switch (genderString.ToLower())
            {
                case "male":
                    gender = Gender.Male;
                    break;
                case "female":
                    gender = Gender.Female;
                    break;
                default:
                    Gender other = Gender.Other;
                    break;
            }
            string birthString = "";
            bool birthIsValid = false;
            bool birthFirstTry = true;
            DateTime birth = DateTime.Now;
            do
            {
                if (!birthFirstTry)
                {
                    Console.WriteLine("\tBirth is not valid! Enter it again!");
                }
                Console.Write("\tBirth(YYYY-MM-DD): ");
                birthString = Console.ReadLine();
                if (DateTime.TryParse(birthString, out birth))
                {
                    birthIsValid = true;
                }
                birthFirstTry = false;
            } while (!birthIsValid);
            return new Patient(firstname, lastname, gender, birth);
        }

        public static Examination ReadNewExamination()
        {
            Console.WriteLine("\nPlease add a new examination!");
            int patientid;
            bool patientidIsValid = false;
            bool patientidFirstTry = true;
            do
            {
                if (!patientidFirstTry)
                {
                    Console.WriteLine("\tPatientID is not valid! Enter it again!");
                }
                Console.Write("\tPatientID: ");
                if (int.TryParse(Console.ReadLine(), out patientid))
                {
                    patientidIsValid = true;
                }
                patientidFirstTry = false;
            } while (!patientidIsValid);

            string eyeString = "";
            bool eyeIsValid = false;
            bool eyeFirstTry = true;
            do
            {
                if (!eyeFirstTry)
                {
                    Console.WriteLine("\tEye is not valid! Enter it again!");
                }
                Console.Write("\tEye(left/right): ");
                eyeString = Console.ReadLine();
                if (eyeString.ToLower() == "left" || eyeString.ToLower() == "right")
                {
                    eyeIsValid = true;
                }
                eyeFirstTry = false;
            } while (!eyeIsValid);
            Eye eye = Eye.Right;
            switch (eyeString.ToLower())
            {
                case "left":
                    eye = Eye.Left;
                    break;
                case "right":
                    eye = Eye.Right;
                    break;
            }

            double sphereDiopter;
            bool sphereDiopterIsValid = false;
            bool sphereDiopterFirstTry = true;
            do
            {
                if (!sphereDiopterFirstTry)
                {
                    Console.WriteLine("\tSphere Diopter is not valid! Enter it again!");
                }
                Console.Write("\tSphere Diopter: ");
                if (double.TryParse(Console.ReadLine(), out sphereDiopter))
                {
                    sphereDiopterIsValid = true;
                }
                sphereDiopterFirstTry = false;
            } while (!sphereDiopterIsValid);

            double cylinderDiopter;
            bool cylinderDiopterIsValid = false;
            bool cylinderDiopterFirstTry = true;
            do
            {
                if (!cylinderDiopterFirstTry)
                {
                    Console.WriteLine("\tCylinder Diopter is not valid! Enter it again!");
                }
                Console.Write("\tCylinder Diopter: ");
                if (double.TryParse(Console.ReadLine(), out cylinderDiopter))
                {
                    cylinderDiopterIsValid = true;
                }
                cylinderDiopterFirstTry = false;
            } while (!cylinderDiopterIsValid);

            int axis;
            bool axisIsValid = false;
            bool axisFirstTry = true;
            do
            {
                if (!axisFirstTry)
                {
                    Console.WriteLine("\tAxis is not valid! Enter it again!");
                }
                Console.Write("\tAxis: ");
                if (int.TryParse(Console.ReadLine(), out axis))
                {
                    axisIsValid = true;
                }
                axisFirstTry = false;
            } while (!axisIsValid);

            Console.WriteLine(new Examination(patientid, eye, sphereDiopter, cylinderDiopter, axis));
            return new Examination(patientid, eye, sphereDiopter, cylinderDiopter, axis);
        }
    }
}
