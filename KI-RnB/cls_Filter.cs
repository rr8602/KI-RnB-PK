using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Emgu.CV;
//using Emgu.CV.Structure;
//using Emgu.CV.UI;

namespace KI_RnB
{
    class cls_Filter
    {
        //--------------------------------------------------------------------------
        // This function returns the data filtered. Converted to C# 2 July 2014.
        // Original source written in VBA for Microsoft Excel, 2000 by Sam Van
        // Wassenbergh (University of Antwerp), 6 june 2007.
        //--------------------------------------------------------------------------
        public static double[] Butterworth(double[] indata, double deltaTimeinsec, double CutOff)
        {
            if (indata == null) return null;
            if (CutOff == 0) return indata;

            double Samplingrate = 1 / deltaTimeinsec;
            long dF2 = indata.Length - 1;        // The data range is set with dF2
            double[] Dat2 = new double[dF2 + 4]; // Array with 4 extra points front and back
            double[] data = indata; // Ptr., changes passed data

            // Copy indata to Dat2
            for (long r = 0; r < dF2; r++)
            {
                Dat2[2 + r] = indata[r];
            }
            Dat2[1] = Dat2[0] = indata[0];
            Dat2[dF2 + 3] = Dat2[dF2 + 2] = indata[dF2];

            const double pi = 3.14159265358979;
            double wc = Math.Tan(CutOff * pi / Samplingrate);
            double k1 = 1.414213562 * wc; // Sqrt(2) * wc
            double k2 = wc * wc;
            double a = k2 / (1 + k1 + k2);
            double b = 2 * a;
            double c = a;
            double k3 = b / k2;
            double d = -2 * a + k3;
            double e = 1 - (2 * a) - k3;

            // RECURSIVE TRIGGERS - ENABLE filter is performed (first, last points constant)
            double[] DatYt = new double[dF2 + 4];
            DatYt[1] = DatYt[0] = indata[0];
            for (long s = 2; s < dF2 + 2; s++)
            {
                DatYt[s] = a * Dat2[s] + b * Dat2[s - 1] + c * Dat2[s - 2]
                           + d * DatYt[s - 1] + e * DatYt[s - 2];
            }
            DatYt[dF2 + 3] = DatYt[dF2 + 2] = DatYt[dF2 + 1];

            // FORWARD filter
            double[] DatZt = new double[dF2 + 2];
            DatZt[dF2] = DatYt[dF2 + 2];
            DatZt[dF2 + 1] = DatYt[dF2 + 3];
            for (long t = -dF2 + 1; t <= 0; t++)
            {
                DatZt[-t] = a * DatYt[-t + 2] + b * DatYt[-t + 3] + c * DatYt[-t + 4]
                            + d * DatZt[-t + 1] + e * DatZt[-t + 2];
            }

            // Calculated points copied for return
            for (long p = 0; p < dF2; p++)
            {
                data[p] = DatZt[p];
            }

            return data;
        }

    }

    public class FilterButterworth
    {
        /// <summary>
        /// rez amount, from sqrt(2) to ~ 0.1
        /// </summary>
        private readonly float resonance;

        private readonly float frequency;
        private readonly int sampleRate;
        private readonly PassType passType;

        private readonly float c, a1, a2, a3, b1, b2;

        /// <summary>
        /// Array of input values, latest are in front
        /// </summary>
        private float[] inputHistory = new float[2];

        /// <summary>
        /// Array of output values, latest are in front
        /// </summary>
        private float[] outputHistory = new float[3];

        public FilterButterworth(float frequency, int sampleRate, PassType passType, float resonance)
        {
            this.resonance = resonance;
            this.frequency = frequency;
            this.sampleRate = sampleRate;
            this.passType = passType;

            switch (passType)
            {
                case PassType.Lowpass:
                    c = 1.0f / (float)Math.Tan(Math.PI * frequency / sampleRate);
                    a1 = 1.0f / (1.0f + resonance * c + c * c);
                    a2 = 2f * a1;
                    a3 = a1;
                    b1 = 2.0f * (1.0f - c * c) * a1;
                    b2 = (1.0f - resonance * c + c * c) * a1;
                    break;

                case PassType.Highpass:
                    c = (float)Math.Tan(Math.PI * frequency / sampleRate);
                    a1 = 1.0f / (1.0f + resonance * c + c * c);
                    a2 = -2f * a1;
                    a3 = a1;
                    b1 = 2.0f * (c * c - 1.0f) * a1;
                    b2 = (1.0f - resonance * c + c * c) * a1;
                    break;
            }
        }

        public enum PassType
        {
            Highpass,
            Lowpass,
        }

        public void Update(float newInput)
        {
            float newOutput = a1 * newInput + a2 * this.inputHistory[0] + a3 * this.inputHistory[1] - b1 * this.outputHistory[0] - b2 * this.outputHistory[1];

            this.inputHistory[1] = this.inputHistory[0];
            this.inputHistory[0] = newInput;

            this.outputHistory[2] = this.outputHistory[1];
            this.outputHistory[1] = this.outputHistory[0];
            this.outputHistory[0] = newOutput;
        }

        public float Value
        {
            get { return this.outputHistory[0]; }
        }
    }

    #region Kalman Filter
    //public class SyntheticData
    //{
    //    public Matrix<float> state;
    //    public Matrix<float> transitionMatrix;
    //    public Matrix<float> measurementMatrix;
    //    public Matrix<float> processNoise;
    //    public Matrix<float> measurementNoise;
    //    public Matrix<float> errorCovariancePost;

    //    public SyntheticData()
    //    {
    //        state = new Matrix<float>(4, 1);
    //        state[0, 0] = 0f; // x-pos
    //        state[1, 0] = 0f; // y-pos
    //        state[2, 0] = 0f; // x-velocity
    //        state[3, 0] = 0f; // y-velocity
    //        transitionMatrix = new Matrix<float>(new float[,]
    //                {
    //                    {1, 0, 1, 0},  // x-pos, y-pos, x-velocity, y-velocity
    //                    {0, 1, 0, 1},
    //                    {0, 0, 1, 0},
    //                    {0, 0, 0, 1}
    //                });
    //        measurementMatrix = new Matrix<float>(new float[,]
    //                {
    //                    { 1, 0, 0, 0 },
    //                    { 0, 1, 0, 0 }
    //                });
    //        measurementMatrix.SetIdentity();
    //        processNoise = new Matrix<float>(4, 4); //Linked to the size of the transition matrix
    //        processNoise.SetIdentity(new MCvScalar(1.0e-4)); //The smaller the value the more resistance to noise 
    //        measurementNoise = new Matrix<float>(2, 2); //Fixed accordiong to input data 
    //        measurementNoise.SetIdentity(new MCvScalar(1.0e-1));
    //        errorCovariancePost = new Matrix<float>(4, 4); //Linked to the size of the transition matrix
    //        errorCovariancePost.SetIdentity();
    //    }

    //    public Matrix<float> GetMeasurement()
    //    {
    //        Matrix<float> measurementNoise = new Matrix<float>(2, 1);
    //        measurementNoise.SetRandNormal(new MCvScalar(), new MCvScalar(Math.Sqrt(measurementNoise[0, 0])));
    //        return measurementMatrix * state + measurementNoise;
    //    }

    //    public void GoToNextState()
    //    {
    //        Matrix<float> processNoise = new Matrix<float>(4, 1);
    //        processNoise.SetRandNormal(new MCvScalar(), new MCvScalar(processNoise[0, 0]));
    //        state = transitionMatrix * state + processNoise;
    //    }
    //}
    #endregion

    //Setup Kalman Filter and predict methods
    //public void KalmanFilter()
    //{
    //    mousePoints = new List<PointF>();
    //    kalmanPoints = new List<PointF>();
    //    kal = new Kalman(4, 2, 0);
    //    syntheticData = new SyntheticData();
    //    Matrix<float> state = new Matrix<float>(new float[]
    //        {
    //            0.0f, 0.0f, 0.0f, 0.0f
    //        });
    //    kal.CorrectedState = state;
    //    kal.TransitionMatrix = syntheticData.transitionMatrix;
    //    kal.MeasurementNoiseCovariance = syntheticData.measurementNoise;
    //    kal.ProcessNoiseCovariance = syntheticData.processNoise;
    //    kal.ErrorCovariancePost = syntheticData.errorCovariancePost;
    //    kal.MeasurementMatrix = syntheticData.measurementMatrix;
    //}
    //public PointF[] filterPoints(PointF pt)
    //{
    //    syntheticData.state[0, 0] = pt.X;
    //    syntheticData.state[1, 0] = pt.Y;
    //    Matrix<float> prediction = kal.Predict();
    //    PointF predictPoint = new PointF(prediction[0, 0], prediction[1, 0]);
    //    PointF measurePoint = new PointF(syntheticData.GetMeasurement()[0, 0],
    //        syntheticData.GetMeasurement()[1, 0]);
    //    Matrix<float> estimated = kal.Correct(syntheticData.GetMeasurement());
    //    PointF estimatedPoint = new PointF(estimated[0, 0], estimated[1, 0]);
    //    syntheticData.GoToNextState();
    //    PointF[] results = new PointF[2];
    //    results[0] = predictPoint;
    //    results[1] = estimatedPoint;
    //    px = predictPoint.X;
    //    py = predictPoint.Y;
    //    cx = estimatedPoint.X;
    //    cy = estimatedPoint.Y;
    //    return results;
    //}
}
