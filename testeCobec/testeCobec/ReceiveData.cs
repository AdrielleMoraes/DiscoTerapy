using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;

namespace testeCobec
{
    class ReceiveData
    {
        SerialPort port;
        public bool isOpen = false;
        //Incoming data
        private const byte ST = 0x24; //start tranmition byte
        private const byte quit = 0x04;
        private const byte ET = 0x0A; //end transmition byte
        private const byte EMG = 0x45; //emg identifier
        private const byte Q = 0x51; // quaternion identifier
        private byte[] buffer; //buffer to store samples

        private int NE = 1; //number of samples to receive for emg
        private int NQ = 4; //number of samples to receive for quat       


        //circular buffer to hold data
        private const int bufferSize = 10000;
        public CircularBuffer<int> circularBufferEMG = new CircularBuffer<int>(bufferSize); //emg circular buffer
        public CircularBuffer<int[]> circularBufferQ = new CircularBuffer<int[]>(bufferSize); //quaternion circular buffer

        public ReceiveData(string portName, int baudRate)
        { 
            port = new SerialPort();
            port.PortName = portName;
            port.BaudRate = baudRate;
        }

        /// <summary>
        /// Executa um comando na porta serial
        /// </summary>
        /// <param name="comando">OPEN - CLOSE - START - STOP</param>
        public void run(string comando)
        {
            switch (comando)
            {
                case "OPEN":
                    if (!port.IsOpen)
                    {
                        port.Open();
                        port.DiscardOutBuffer();
                        port.DiscardInBuffer();
                        isOpen = true;
                    }
                    break;
                case "CLOSE":
                    if (port.IsOpen)
                    {
                        isOpen = false;
                        port.Close();
                    }
                    break;
                case "START":
                    if (port.IsOpen)
                    {
                        port.Write("CMDSTART");
                    }
                    break;
                case "STOP":
                    if (port.IsOpen)
                    {
                        port.Write("CMDSTOP");
                    }
                    break;
                default:
                    Console.WriteLine("Comando errado");
                    return;
            }
        }

        /// <summary>
        /// Waits byte to arrive
        /// </summary>
        /// <param name="num">number of bytes to wait for</param>
        /// <returns>received byte</returns>
        private byte[] WaitIncomingByte(int num)
        {
            //check how many bytes are coming
            int byteAvailable=0;

            //wait until you get num bytes
            while (byteAvailable < num && port.IsOpen)
            {
                byteAvailable = port.BytesToRead;
            }

            //byte to receive data according to num
            byte[] buff = new byte[num];
            //read bytes
            if (port.IsOpen)
                port.Read(buff, 0, num);

            return buff;
        }

        /// <summary>
        /// receive data from serial port
        /// </summary>
        public void receiveData()
        {
            if (port.IsOpen)
            {
                //waiting for the ST byte
                buffer = WaitIncomingByte(1);

                if (ST == buffer[0])
                {
                    buffer = WaitIncomingByte(1); //this bytes represent the kind of samples we are going to receive

                    if (buffer[0] == EMG)
                    { ReceiveEMG(); } //is a emg data

                    else { ReceiveQuat(); }//is a quat data
                }
            }         
        }

        /// <summary>
        /// Handle emg data
        /// </summary>
        private void ReceiveEMG()
        {
            int[] data = new int[NE];
            buffer = WaitIncomingByte(NE * 2); //this bytes represent the samples we are going to receive

            //join lsb and msb
            data[0] = buffer[0] << 8 | buffer[1]; //storing data

            //waiting for the ET byte
            buffer = WaitIncomingByte(1);
            if (buffer[0] == ET)
            {
                //store data
                circularBufferEMG.Enqueue(data[0]);
            }
            //if not ignore data
        }

        /// <summary>
        /// Handle quat data
        /// </summary>
        private void ReceiveQuat()
        {
            int[] data = new int[NQ];
            buffer = WaitIncomingByte(NQ * 2); //this bytes represent the samples we are going to receive
            int j = 0;
            for (int i = 0; i < data.Length; i++)//read bytes
            {
                data[i] = to_signed(buffer[j] << 8 | buffer[j + 1]);
                //Console.WriteLine(data[i]);
                j += 2;
            }
            //waiting for the ET byte
            buffer = WaitIncomingByte(1);
            if (buffer[0] == ET)
            {
                //store data
                circularBufferQ.Enqueue(data);
            }
            //if not ignore data

        }

        /// <summary>
        /// Get data from emg circular buffer
        /// </summary>
        /// <returns></returns>
       public int HandleEMG()
        {
          return circularBufferEMG.Dequeue();         
        }

        /// <summary>
        /// Get data from quat after processing
        /// </summary>
        /// <returns></returns>
        public double[] HandleQuat()
        {

            double[] quat = new double[4];
            quat = ConvertToDouble(circularBufferQ.Dequeue());
            //double[] ang = toEuler(quat); //get values in angle            
            return quat;
        }

        #region Adequa quaternion
        private double[] ConvertToDouble(int[] valorfeio)
        {
            double[] valorbonito = new double[4];
            for (int i = 0; i < valorbonito.Length; i++)
            {
                valorbonito[i] = valorfeio[i] / 16384.00;
                valorbonito[i] = valorbonito[i] > 2 ? 4 - valorbonito[i] : valorbonito[i];
            }
            return valorbonito;
        }

        private int to_signed(int num)
        {
            if ((num & 0x8000) != 0)
            {
                return -(inverter(num)) - 1;
            }
            else
            {
                return num & 0x7fff;
            }
        }

        private Int16 inverter(int num)
        {
            string entrada = Convert.ToString(Convert.ToUInt16(num), 2);
            string saida = entrada.Replace('0', '2').Replace('1', '0').Replace('2', '1');
            return Convert.ToInt16(saida, 2);
        }


        private double[] toEuler(double[] quat)
        {
            double[] dataQuat = new double[3]; //[0] phi [1] theta [2] psi
            double[] R = new double[] { 0, 0, 0, 0, 0 };
            R[0] = 2.0 * Math.Pow(quat[0], 2) - 1 + 2.0 * Math.Pow(quat[1], 2);
            R[1] = 2.0 * (quat[1] * quat[2] - quat[0] * quat[3]);
            R[2] = 2.0 * (quat[1] * quat[3] + quat[0] * quat[2]);
            R[3] = 2.0 * (quat[2] * quat[3] - quat[0] * quat[1]);
            R[4] = 2.0 * Math.Pow(quat[0], 2) - 1 + 2.0 * Math.Pow(quat[3], 2);
            double phi = Math.Atan2(R[3], R[4]);
            double theta = -Math.Atan(R[2] / Math.Sqrt(1 - Math.Pow(R[2], 2)));
            double psi = Math.Atan2(R[1], R[0]);
            phi = rad2deg(phi);
            theta = rad2deg(theta);
            psi = rad2deg(psi);
            return new double[] { phi, theta, psi };
        }

        /// <summary>
        /// Parse rad to degrees
        /// </summary>
        /// <param name="rad">ang in rad</param>
        /// <returns>ang in degrees</returns>
        private double rad2deg(double rad)
        {
            return rad * 180.0 / Math.PI;
        }
        #endregion
    }
}
