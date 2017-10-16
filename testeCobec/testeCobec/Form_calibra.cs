using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace testeCobec
{
    public partial class Form_calibra : Form
    {


        //Serial port
        string portName = "COM3"; //by default
        int BaudRate = 115200;

        //thread to handle data
        Thread trhGetData; //gets the data from serial port
        Thread trhForEMG; //do something with emg data
        Thread trhForquat;   //do something with quat data     
        bool working = false; //control thread

        //chart's things
        Series channel1Series; //1 channel
        ChartArea chartArea;

        ReceiveData CoordenaDados;

        int[] resultadoEMG;

        int fa = 100;

        public Form_calibra(string _portName)
        {
            InitializeComponent();
            portName = _portName;
            resultadoEMG = new int[fa * 2]; //vetor que armazena os dados da calibração do emg

            //chart properties
            chartArea = new ChartArea("Signal received");

            channel1Series = new Series("Channel 1");
            channel1Series.ChartType = SeriesChartType.FastLine;
            channel1Series.BorderWidth = 3;
            channel1Series.BorderColor = Color.LimeGreen;

            chart1.ChartAreas.Add(chartArea);
            chart1.Series.Add(channel1Series);

        }

        //receive data from serial port
        private void receiveData()
        {
            while (working)
            {
                CoordenaDados.receiveData();
                Application.DoEvents();
            }
            Console.WriteLine("Aquisição encerrada!");
        }


        //plot data from circular buffer
        private void DoSomething()
        {
            int num = 0;
            while (working)
            {
                if (!CoordenaDados.circularBufferEMG.IsEmpty)
                {
                    int buff;
                    int n = CoordenaDados.circularBufferEMG.SamplesToRead;
                   
                    for (int i = 0; i < n; i++)
                    {
                        num++;
                        if (num >= 200)
                        {
                            Disconnect();
                            break;
                        }
                        buff = CoordenaDados.HandleEMG();

                        resultadoEMG[i] = buff;
                                          
                    }                                                                                                
                }
            }           
            calculos();
            Console.WriteLine("EMG encerrado! ");
        }

        double[] quat;
        void HandleQuat()
        {
            quat = new double[4];
            while (working)
            {
                if (!CoordenaDados.circularBufferQ.IsEmpty)
                {
                    quat = CoordenaDados.HandleQuat();
                }
                //show values
                string resultado =
                    "\n W: " + quat[0].ToString("0.00") +
                    "\n X: " + quat[1].ToString("0.00") +
                    "\n Y: " + quat[2].ToString("0.00") +
                    "\n Z: " + quat[3].ToString("0.00");
                this.lbQAtual.BeginInvoke(new Action(() => this.lbQAtual.Text = resultado));
            }
            Console.WriteLine("Quat encerrado! ");
        }

        //plot data from circular buffer
        private void calculos()
        {         
            double media = 0;
            double desvioPad = 0;
            
            media = resultadoEMG.Average();
            //calculo desvio padrão
            for (int i = 0; i < resultadoEMG.Length -1; i++)
            {         
                desvioPad += Math.Pow(media - resultadoEMG[i], 2);
                this.chart1.BeginInvoke(new Action(() => this.channel1Series.Points.AddY(resultadoEMG[i])));
            }
            desvioPad = Math.Sqrt(desvioPad / resultadoEMG.Length);
            double limiar = media + 2 * desvioPad;
            toolStripProgressBar.Value = 0;
            string resultado =
                "\n Média: " + media.ToString("0.00") +
                "\n Desvio Padrão: " + desvioPad.ToString("0.00") +
                "\n Limiar: " + limiar.ToString("0.00");
            lbEMG.Invoke(new Action(() => lbEMG.Text = resultado));
            Console.WriteLine("EMG encerrado! ");
        }

        /// <summary>
        /// função que indica qual o quaternion atual
        /// </summary>
        /// <returns></returns>
        string QuatAtual()
        {
            return
                    "\n W: " + quat[0].ToString("0.00") +
                    "\n X: " + quat[1].ToString("0.00") +
                    "\n Y: " + quat[2].ToString("0.00") +
                    "\n Z: " + quat[3].ToString("0.00");
        }

        #region resultados das calibrações de quaternions

        private void btQRef_Click(object sender, EventArgs e)
        {
            lbQRef.Text = QuatAtual();
        }

        private void btQCima_Click(object sender, EventArgs e)
        {
           lbQCima.Text = QuatAtual();
        }

        private void btQDireita_Click(object sender, EventArgs e)
        {
           lbQDireita.Text = QuatAtual();
        }

        private void btQBaixo_Click(object sender, EventArgs e)
        {
           lbQBaixo.Text = QuatAtual();
        }

        private void btQEsq_Click(object sender, EventArgs e)
        {
           lbQEsquerda.Text = QuatAtual();
        }
        #endregion

        private void Form_calibra_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (CoordenaDados.isOpen)
                {
                    CoordenaDados.run("STOP");
                    working = false; //termina a thread
                    CoordenaDados.run("CLOSE"); //fecha a porta serial
                }
            }
            catch (Exception)
            {
            }
        }

        void Disconnect()
        {
           // btConnect.Enabled = true;
            //btReceive.Enabled = true;
            if (working)
            {
                working = false;
                CoordenaDados.run("STOP");
                //btConnect.Text = "Connect";
                CoordenaDados.run("CLOSE");
                
                //btReceive.Text = "Receive";
            }
        }


        private void btConnect_Click(object sender, EventArgs e)
        {
            if (btConnectEMG.Text == "Connect")
            {
                CoordenaDados = new ReceiveData(portName, BaudRate);
                CoordenaDados.run("OPEN");
                btConnectEMG.Text = "Disconnect";
                tsStatus.Text = "Connected to: " + portName;
                btReceive.Enabled = true;
            }
            //port is open
            else
            {
                CoordenaDados.run("CLOSE");
                btReceive.Enabled = false;
                btPause.Enabled = false;
                btConnectEMG.Text = "Connect";
                tsStatus.Text = "Disconnected";
            }
        }

        private void btReceive_Click(object sender, EventArgs e)
        {
            if (CoordenaDados.isOpen)
            {
                working = true;
                //thread properties
                trhGetData = new Thread(receiveData);
                trhForEMG = new Thread(DoSomething);
                trhForquat = new Thread(HandleQuat);

                //define thread priority
                trhGetData.Priority = ThreadPriority.AboveNormal;
                trhForEMG.Priority = ThreadPriority.Normal;
                trhForquat.Priority = ThreadPriority.Normal;
                //start threads
                trhGetData.Start();
                trhForEMG.Start();
                trhForquat.Start();

                btPause.Enabled = true;
                btConnectEMG.Enabled = false;
                btReceive.Enabled = false;
                CoordenaDados.run("START");
            }
            else
            {
                MessageBox.Show("Serial port is closed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btPause_Click(object sender, EventArgs e)
        {
            Disconnect();
            btPause.Enabled = false;
        }
    }
}
