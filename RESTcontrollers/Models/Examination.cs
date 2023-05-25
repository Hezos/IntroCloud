using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RESTcontrollers
{
    public class Examination
    {
        //[JsonPropertyAttribute("patientId")]
        public int? patientId;
        public Eye? eye;
        public double? sphereDiopter;
        public double? cylinderDiopter;
        public int? axis;

        
        public Examination(int patientId, Eye eye, double sphereDiopter, double cylinderDiopter, int axis)
        {
            this.PatientId = patientId;
            this.Eye = eye;
            this.SphereDiopter = sphereDiopter;
            this.CylinderDiopter = cylinderDiopter;
            this.Axis = axis;
        }
        
        public Examination()
        {

        }

        public int? PatientId { get => patientId; set => patientId = value; }
        public double? SphereDiopter { get => sphereDiopter; set => sphereDiopter = value; }
        public double? CylinderDiopter { get => cylinderDiopter; set => cylinderDiopter = value; }
        public int? Axis { get => axis; set => axis = value; }
        public Eye? Eye { get => eye; set => eye = value; }

        public Examination GetExaminationFromString(string input)
        {
            string[] array = input.Split(',');
            return new Examination(int.Parse(array[0]), (Eye)Enum.Parse(typeof(Eye), array[1]), double.Parse(array[2]), double.Parse(array[3]), int.Parse(array[4]));
        }
        public override string ToString()
        {
            return $"PatientID: {this.PatientId}\nEye: {this.Eye.ToString().ToLower()}\nSphereDiopter: {this.SphereDiopter}\nCylinderDiopter: {this.CylinderDiopter}\nAxis: {this.Axis}";
        }
    }
}
