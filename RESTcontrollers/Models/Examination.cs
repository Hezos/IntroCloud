using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;
using JsonConverterAttribute = System.Text.Json.Serialization.JsonConverterAttribute;

namespace RESTcontrollers
{
    public class Examination
    {

        public int patientId;

        public Eye eye;

        public double sphereDiopter;

        public double cylinderDiopter;

        public int axis;

        
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

        [JsonPropertyName("patientId")]
        public int PatientId { get => patientId; set => patientId = value; }
        [JsonPropertyName("sphereDiopter")]
        public double SphereDiopter { get => sphereDiopter; set => sphereDiopter = value; }
        [JsonPropertyName("cylinderDiopter")]
        public double CylinderDiopter { get => cylinderDiopter; set => cylinderDiopter = value; }
        [JsonPropertyName("axis")]
        public int Axis { get => axis; set => axis = value; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Eye Eye { get => eye; set => eye = value; }

        static public Examination GetExaminationFromString(string input)
        {
            string[] array = input.Split(';');
            return new Examination(int.Parse(array[0]), (Eye)Enum.Parse(typeof(Eye), array[1]), double.Parse(array[2]), double.Parse(array[3]), int.Parse(array[4]));
        }

        public override string ToString()
        {
            return $"PatientID: {this.PatientId}\nEye: {this.Eye.ToString().ToLower()}\nSphereDiopter: {this.SphereDiopter}\nCylinderDiopter: {this.CylinderDiopter}\nAxis: {this.Axis}";
        }
    }
}
