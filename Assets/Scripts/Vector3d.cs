using System;
using UnityEngine;
using UnityEngine.Internal;

namespace DoubleComputation
{
    public struct Vector3d
    {
        public double x;
        public double y;
        public double z;

        //constructor
        //in case that height is not needed
        public Vector3d(double input_x, double input_y)
        {
            x = input_x;
            y = input_y;
            z = 0;
        }

        public Vector3d(double input_x, double input_y, double input_z)
        {
            x = input_x;
            y = input_y;
            z = input_z;
        }

        public override string ToString(){
            return String.Format("({0},{1},{2})", x, y, z);
        }
        public string ToString(string format){
            return String.Format("({0},{1},{2})", x.ToString(format), y.ToString(format), z.ToString(format));
        }

    }

    public struct Mathd
    {
        public static double PI =  3.14159265358979;
        public static double Deg2Rad = PI/180.0;
        public static double Rad2Deg = 180/PI;

        //below not needed, use lib Math for computation
        /*
        public static double Sin(double radius){
        }

        public static double Cos(double radius){
        }

        public static double Atan2(double x, double y){
        }

        public static double Sqrt(double x){
        }

        public static double Asin(double x){
        }
        */
    }
}
