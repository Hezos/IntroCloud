using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using RESTcontrollers;
using System;
using System.Text;
using System.Text.Json;

namespace CommandLineInterface
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Program started");

            Menu mainmenu = new Menu(new string[]{
                "Add a new patient",
                "Add a new examination",
                "Get information about an examination",
                "Trigger EventHub event",
                "Quit"
            });
            bool ShouldExit = false;
            while (!ShouldExit)
            {
                switch (mainmenu.ReadMenu())
                {
                    case 1:
                        //NEW PATIENT
                        Menu.ReadNewPatient();
                        break;
                    case 2:
                        //NEW EXAMINATION
                        Menu.ReadNewExamination();
                        break;
                    case 3:
                        //GET INFORMATION ABPUT AN EXAMINATION
                        ReadData();
                        break;
                    case 4:
                        //TRIGGER EVENTHUB EVENT
                        AddExamination();
                        break;
                    case 5:
                        ShouldExit = true;
                        //QUIT
                        break;
                }
            }
        }

        /*
        public static void WriteMenu()
        {
            Console.WriteLine("Please select an action!");
            Console.WriteLine("1-Add a new examination.");
            Console.WriteLine("2-Get information about an examination");
            Console.WriteLine("3-Trigger EventHub event.");
            Console.WriteLine("4-Quit");
        }
        */
        public static void AddExamination()
        {
            HttpServiceClass serviceClass = new HttpServiceClass(new HttpClient());
            serviceClass.SendRequest(HttpMethod.Post, "https://localhost:7252/Add", new Examination());
        }

        public static void ReadData()
        {
            Examination examination = new Examination();
            HttpServiceClass httpService = new HttpServiceClass(new HttpClient());
            //;
            dynamic result = httpService.GetRequest(HttpMethod.Get, "https://localhost:7252/GetExaminations", "");
            
            Console.WriteLine(result);
        }
        


        //Itt vannak a kommentek:
        //Patient REST rész híányzik
        //Controller, Service, Repository, nagyrészt másolás az Examination-ból
        //ExaminationController kommentek
    }
}