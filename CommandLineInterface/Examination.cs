using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineInterface
{
    internal class Examination
    {
        private int patientId;
        private Eye eye;
        private double sphereDiopter;
        private double cylinderDiopter;
        private int axis;

        public Examination(int patientId, Eye eye, double sphereDiopter, double cylinderDiopter, int axis)
        {
            this.PatientId = patientId;
            this.Eye = eye;
            this.SphereDiopter = sphereDiopter;
            this.CylinderDiopter = cylinderDiopter;
            this.Axis = axis;
        }

        public int PatientId { get => patientId; set => patientId = value; }
        public double SphereDiopter { get => sphereDiopter; set => sphereDiopter = value; }
        public double CylinderDiopter { get => cylinderDiopter; set => cylinderDiopter = value; }
        public int Axis { get => axis; set => axis = value; }
        internal Eye Eye { get => eye; set => eye = value; }

        public override string ToString()
        {
            return $"PatientID: {this.PatientId}\nEye: {this.Eye.ToString().ToLower()}\nSphereDiopter: {this.SphereDiopter}\nCylinderDiopter: {this.CylinderDiopter}\nAxis: {this.Axis}";
        }
    }
}
