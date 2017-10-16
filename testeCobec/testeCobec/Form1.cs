using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Globalization;

namespace testeCobec
{
    public partial class Form1 : Form
    {

        
        //TODO coletar dados para verificar o drift em Z
        //salvar os dados em arquivo
        StreamWriter file;
        StreamWriter fileQuat;


        StreamReader readFile;
        string path = @"C:/Users/ADRIELLE/Desktop/MyFile.txt";

        private void Form1_Load(object sender, EventArgs e)
        {
            t = 1.77;
            string mensagem;
            readFile = new StreamReader(path);
            while ((mensagem = readFile.ReadLine()) != null)
            {
                chart1.Series[0].Points.AddXY(t, Convert.ToDouble(mensagem.Replace('.',','))/7.0f);
                t += 1.0 / 4000f;
            }
        }

        //Serial port
        string portName = "COM3"; //by default
        int BaudRate = 115200;
        int sampFreq = 100;


        //vetores que armazenam os valores da calibração dos quaternions
        double[] quatCima = new double[4]; //cima
        double[] quatRef = new double[4]; //referência
        double[] quatBaixo = new double[4]; //baixo
        double[] quatEsquerda = new double[4]; //esquerda
        double[] quatDireita = new double[4]; //direita


        //valores da calibração do emg
        const int tam = 400; //tamanho da janela de calibração do emg
        float[] resultadoEMG = new float[tam]; //vetor que armazena os dados da calibração
        double limiar = 0; //limiar da calibração
        bool estadoAnteriorEMG = false; //determina o estado anterior da eletromiografia (em processo de contração ou nível basal)
        bool estadoAtualEMG = false; // determina o estado atual da eletromiografia(em contração ou não)

        //vetor para armazenar os quaternions atuais
        double[] quat;

        //thread to handle data
        Thread trhGetData; //gets the data from serial port
        Thread trhForEMG; //do something with emg data
        Thread trhForquat;   //do something with quat data     
        bool working = false; //control thread


        //manipula os dados que chegam na porta serial
        ReceiveData CoordenaDados;
     
        public Form1()
        {
            InitializeComponent();
            ComPorts();

            lbEMG.Text = "Média: ---\nDesvio Padrão: --- \nLimiar: ---";
            lbQAtual.Text = "\n W: --- " + "\n X: ---" + "\n Y: ---" + "\n Z: ---";
            lbQRef.Text = "\n W: --- " + "\n X: ---" + "\n Y: ---" + "\n Z: ---";
            lbQCima.Text = "\n W: --- " + "\n X: ---" + "\n Y: ---" + "\n Z: ---";
            lbQBaixo.Text = "\n W: --- " + "\n X: ---" + "\n Y: ---" + "\n Z: ---";
            lbQEsquerda.Text = "\n W: --- " + "\n X: ---" + "\n Y: ---" + "\n Z: ---";
            lbQDireita.Text = "\n W: --- " + "\n X: ---" + "\n Y: ---" + "\n Z: ---";
        }

        void Conectar()
        {
            try
            {
                string portName = cbPortNames.Items[cbPortNames.SelectedIndex].ToString(); //get value from combo box
            }
            catch
            {

                return;
            }

             portName = cbPortNames.Items[cbPortNames.SelectedIndex].ToString(); //get value from combo box                                                        

            if (btConnect.Text == "Conectar")
            {
                CoordenaDados = new ReceiveData(portName, BaudRate);
                btPause.Enabled = false;
                btReceive.Enabled = true;
                CoordenaDados.run("OPEN"); //comando de abir a porta serial
                btConnect.Text = "Desconectar";
                tsStatus.Text = "Connected to: " + portName;
                btReceive.Enabled = true;
            }
            //port is open
            else
            {
                CoordenaDados.run("CLOSE"); //comando para fechar a porta
                btReceive.Enabled = false;
                btPause.Enabled = false;
                btConnect.Text = "Conectar";
                tsStatus.Text = "Desconectar";
            }
        }

        //receive data from serial port
        private void receiveData()
        {
            while (working)
            {
                CoordenaDados.receiveData();             
            }
            Console.WriteLine("Aquisição encerrada!");
        }


        int num=0;
        string calibra = "";
        double tc = 0; //tempo de calibração
        double t =0; //tempo atual
        //plot data from circular buffer
        private void DoSomething()
        {
            while (working)
            {
                if (!CoordenaDados.circularBufferEMG.IsEmpty) //se o buffer não está vazio podemos remover dados
                {
                    float buff;
                    int samples = CoordenaDados.circularBufferEMG.SamplesToRead; //numero de amostras disponíveis
                    for (int i = 0; i < samples; i++)
                    {
                        buff = CoordenaDados.HandleEMG()/205.0f; //retira um valor do buffer circular

                        //verifica se está no modo coleta ou calibração
                        switch (calibra)
                        {

                            //modo calibração
                            case "calibrando":
                                if (num < tam) //se ainda não chegou na quantidade de amotras/ tempo suficiente
                                {
                                    tc += 1.0f / sampFreq;
                                    resultadoEMG[num] = buff; //insere o valor no vetor
                                    file.WriteLine(buff.ToString()); //armazena em arquivo
                                    this.chart2.BeginInvoke(new Action(() => this.chart2.Series[0].Points.AddXY(tc,buff))); //plota o valor
                                    num++;
                                }
                                else
                                {
                                    calculos();    //efetua a media e o desvio padrão para calcular o limiar                                 
                                    Disconnect(); //fecha as conexões com a porta e para de receber dados
                                }
                                break;

                                //modo coleta
                            case "coletando":
                                file.WriteLine(buff.ToString()); //armazena o dado no arquivo
                                //se o sinal ultrapassou o limiar e o estado mudou
                                if (buff > limiar)
                                {
                                    estadoAtualEMG = true;                                                  
                                }
                                else
                                {
                                    estadoAtualEMG = false;
                                }
                                //se os estados são diferentes e se encontram acima do limiar
                                if (estadoAnteriorEMG != estadoAtualEMG && estadoAtualEMG)
                                {
                                    SendKeys.SendWait(" ");//envia a tecla espaço
                                }                               
                                estadoAnteriorEMG = estadoAtualEMG;
                                //t += 1.0f / sampFreq; //contagem do tempo de plotagem
                                //this.chart1.BeginInvoke(new Action(() => this.chart1.Series[0].Points.AddXY(t,buff))); //plota o valor
                                //caminhando com o gráfico
                                if (t > chart1.ChartAreas[0].AxisX.Maximum)
                                {

                                    chart1.Invoke(new Action(() => chart1.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisX.Maximum + 0.1));
                                    chart1.Invoke(new Action(() => chart1.ChartAreas[0].AxisX.Minimum = chart1.ChartAreas[0].AxisX.Minimum + 0.1));
                                }
                                break;
                        }
                    }
                }
            }
            //terminado o processo...
           //salva o tempo final da coleta
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

            file.WriteLine(timestamp);
            file.Close();//fecha o arquivo
            Console.WriteLine("EMG encerrado! ");
        }

        
        /// <summary>
        /// Efetua os calculos de média, desvio padrão para encontrar o limiar
        /// </summary>
        private void calculos()
        {
            double media = 0;
            double desvioPad = 0;
            //calculo média
            media = resultadoEMG.Average();
            //calculo desvio padrão
            for (int i = 0; i < resultadoEMG.Length - 1; i++)
            {
                desvioPad += Math.Pow(media - resultadoEMG[i], 2);
            }
            desvioPad = Math.Sqrt(desvioPad / resultadoEMG.Length);

            //definição do limiar
            limiar = media + 2 * desvioPad;

            //mostra o resultado
            string resultado =
                "\n Média: " + media.ToString("0.00") +
                "\n Desvio Padrão: " + desvioPad.ToString("0.00") +
                "\n Limiar: " + limiar.ToString("0.00");
            lbEMG.Invoke(new Action(() => lbEMG.Text = resultado));
            file.WriteLine(resultado); //armazena em arquivo
            Console.WriteLine("EMG encerrado! ");
        }


        /// <summary>
        /// Recebe os valores de quat armazenados
        /// </summary>
        void HandleQuat()
        {
            quat = new double[4];
            double tq = 0;
            while (working)
            {
                if (!CoordenaDados.circularBufferQ.IsEmpty)
                {
                    quat = CoordenaDados.HandleQuat();
                    
                    //corrigindo W
                    quat[0] = Math.Abs(Math.Sqrt(Math.Pow(quat[1],2) + Math.Pow(quat[2],2) + Math.Pow(quat[3],2) ));
                    quat[0] = Math.Cos(Math.Asin(quat[0]));

                    fileQuat.WriteLine(quat[0] + "\t" + quat[1] + "\t" + quat[2] + "\t" + quat[3]);

                    //show values
                    lbPhi.Invoke(new Action(() => lbPhi.Text = "W: " + quat[0].ToString("0.000")));
                    lbTheta.Invoke(new Action(() => lbTheta.Text = "X: " + quat[1].ToString("0.000")));
                    lbPsi.Invoke(new Action(() => lbPsi.Text = "Y: " + quat[2].ToString("0.000")));
                    lbQuatZ.Invoke(new Action(() => lbQuatZ.Text = "Z: " + quat[3].ToString("0.000")));


                    //trackBar_phi.Invoke(new Action(() => trackBar_phi.Value = (int)(quat[0] * 100.0)));
                    //trackBar_theta.Invoke(new Action(() => trackBar_theta.Value = (int)(quat[1] * 100.0)));
                    //trackBar_psi.Invoke(new Action(() => trackBar_psi.Value = (int)(quat[2] * 100.0)));
                    //trackBar_Z.Invoke(new Action(() => trackBar_Z.Value = (int)(quat[3] * 100.0)));


                    //show values
                    string resultado =
                        "\n W: " + quat[0].ToString("0.00") +
                        "\n X: " + quat[1].ToString("0.00") +
                        "\n Y: " + quat[2].ToString("0.00") +
                        "\n Z: " + quat[3].ToString("0.00");
                    lbQAtual.BeginInvoke(new Action(() => lbQAtual.Text = resultado));
                    //switch (estadoAtual)
                    //{
                    //    case "cima":
                    //        this.pictureBox1.Invoke(new Action(() => this.pictureBox1.Image = Properties.Resources.cima));
                    //        break;
                    //    case "baixo":
                    //        this.pictureBox1.Invoke(new Action(() => this.pictureBox1.Image = Properties.Resources.baixo));
                    //        break;
                    //    case "direita":
                    //        this.pictureBox1.Invoke(new Action(() => this.pictureBox1.Image = Properties.Resources.direita));
                    //        break;
                    //    case "esquerda":
                    //        this.pictureBox1.Invoke(new Action(() => this.pictureBox1.Image = Properties.Resources.esquerda));
                    //        break;
                    //    default:
                    //        this.pictureBox1.Invoke(new Action(() => this.pictureBox1.Image = null));
                    //        break;
                    //}
                    SetDirecao();
                }

                
                
                
               
            }
            
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            fileQuat.WriteLine(timestamp);
            fileQuat.Close();
            Console.WriteLine("Quat encerrado! ");
        }

        //estados
        string estadoAtual = "nada";
        string estadoAnterior = "nada";
        bool saiuRef = true;

        private void DefineEstados( )
        {
            //se o estado mudou e se o ultimo estado foi saindo da referência 
            if (estadoAtual != estadoAnterior)
            {
                switch (estadoAtual)
                {
                    case "cima":
                        if (saiuRef)
                        {
                            SendKeys.SendWait("{UP}");
                            SendKeys.SendWait("{UP}");
                            SendKeys.SendWait("{UP}");
                            SendKeys.SendWait("{UP}");
                            SendKeys.SendWait("{UP}");
                            saiuRef = false;
                        }                    
                        break;

                    case "baixo":
                        if (saiuRef)
                        {

                            SendKeys.SendWait("{DOWN}");
                            SendKeys.SendWait("{DOWN}");
                            SendKeys.SendWait("{DOWN}");
                            SendKeys.SendWait("{DOWN}");
                            SendKeys.SendWait("{DOWN}");
                            saiuRef = false;
                        }
                        
                        break;
                    case "esquerda":
                        if (saiuRef)
                        {
                            SendKeys.SendWait("{LEFT}");
                            SendKeys.SendWait("{LEFT}");
                            SendKeys.SendWait("{LEFT}");
                            SendKeys.SendWait("{LEFT}");
                            SendKeys.SendWait("{LEFT}");
                            saiuRef = false;
                        }
                        
                        break;
                    case "direita":
                        if (saiuRef)
                        {
                            SendKeys.SendWait("{RIGHT}");
                            SendKeys.SendWait("{RIGHT}");
                            SendKeys.SendWait("{RIGHT}");
                            SendKeys.SendWait("{RIGHT}");
                            SendKeys.SendWait("{RIGHT}");
                            saiuRef = false;
                        }
                        
                        break;

                    case "ref":
                            saiuRef = true;
                        break;
                    default:
                        break;
                }
                estadoAnterior = estadoAtual;
                this.lbDir.BeginInvoke(new Action(() => this.lbDir.Text = estadoAtual));
            }
            
        }

        private void SetDirecao()
        {
            
            double[] q = quat;

            if (EstaNaFaixa(q[0],quatRef[0]) && EstaNaFaixa(q[1], quatRef[1]) && EstaNaFaixa(q[2], quatRef[2]) && EstaNaFaixa(q[3], quatRef[3]))
            {
                estadoAtual = "ref";
            }

            else if (EstaNaFaixa(q[0], quatBaixo[0]) && EstaNaFaixa(q[1], quatBaixo[1]) && EstaNaFaixa(q[2], quatBaixo[2]) && EstaNaFaixa(q[3], quatBaixo[3]))
            {
                estadoAtual = "baixo";
            }
            else if (EstaNaFaixa(q[0], quatCima[0]) && EstaNaFaixa(q[1], quatCima[1]) && EstaNaFaixa(q[2], quatCima[2]) && EstaNaFaixa(q[3], quatCima[3]))
            {
                estadoAtual = "cima";
            }
            else if (EstaNaFaixa(q[0], quatDireita[0]) && EstaNaFaixa(q[1], quatDireita[1]) && EstaNaFaixa(q[2], quatDireita[2]) && EstaNaFaixa(q[3], quatDireita[3]))
            {
                estadoAtual = "direita";
            }

            else if (EstaNaFaixa(q[0], quatEsquerda[0]) && EstaNaFaixa(q[1], quatEsquerda[1]) && EstaNaFaixa(q[2], quatEsquerda[2]) && EstaNaFaixa(q[3], quatEsquerda[3]))
            {
                estadoAtual = "esquerda";
            }
            else
            {
                estadoAtual = "ref";
            }         
            DefineEstados();
        }

        private bool EstaNaFaixa(double q0, double q)
        {
            if (Math.Abs(q0 - q) <= 0.2f)
            {
                return true;
            }
            return false;
        }

        void Receber()
        {
            if (CoordenaDados.isOpen)
            {
                working = true;
                //thread properties
                trhGetData = new Thread(receiveData);
                trhForEMG = new Thread(DoSomething);
                trhForquat = new Thread(HandleQuat);

                //define thread priority
                trhGetData.Priority = ThreadPriority.Normal;
                trhForEMG.Priority = ThreadPriority.Normal;
                trhForquat.Priority = ThreadPriority.BelowNormal;
                //start threads
                trhGetData.Start();
                trhForquat.Start();
                trhForEMG.Start();
                

                btPause.Enabled = true;
                btConnect.Enabled = false;
                btReceive.Enabled = false;
                CoordenaDados.run("START");
                tsReceiving.Text = "Receiving data...";
            }
            else
            {
                MessageBox.Show("Serial port is closed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //timer to refresh port changes
        private void timerPorts_Tick(object sender, EventArgs e)
        {
           ComPorts();
        }

        public void ComPorts( )
        {
            int i = 0;
            bool isDifferent = false;

            //if the length is equal
            if (cbPortNames.Items.Count == SerialPort.GetPortNames().Length)
            {
                foreach (string s in SerialPort.GetPortNames())
                {
                    //but the strings are different
                    if (!cbPortNames.Items[i++].Equals(s))
                    {
                        isDifferent = true;
                    }

                }
            }
            else
            {
                isDifferent = true;
            }

            //if nothing changed, nothing to do
            if (!isDifferent)
            {
                return;
            }

            //clear cb items and add new values
            cbPortNames.Items.Clear();
            foreach (string s in SerialPort.GetPortNames())
            {
                cbPortNames.Items.Add(s);
                //change selected index
                cbPortNames.SelectedIndex = 0;
            }
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (CoordenaDados.isOpen)
                {
                    file.Close();
                    fileQuat.Close();
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    fileQuat.WriteLine(timestamp);
                    file.WriteLine(timestamp);
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
            menuStrip1.Invoke(new Action(() => btConnect.Enabled = true));
            menuStrip1.Invoke(new Action(() => btReceive.Enabled = true));
            if (working)
            {
                CoordenaDados.run("STOP");
                menuStrip1.Invoke(new Action(() => btConnect.Text = "Conectar"));
                
                CoordenaDados.run("CLOSE");
                working = false;
                menuStrip1.Invoke(new Action(() => btReceive.Text = "Receber"));
                statusStrip1.Invoke(new Action(() => tsReceiving.Text = ""));            
            }
            menuStrip1.Invoke(new Action(() => btPause.Enabled = false));
            menuStrip1.Invoke(new Action(() => btReceive.Enabled = false));
        }

        private void conectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Conectar();
        }



        private void receberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //definição dos diretórios
            string path = @"C:\Users\ADRIELLE\Google Drive\Competição COBEC SEB 2017\testeCobec\";
            string path2 = @"C:\Users\ADRIELLE\Google Drive\Competição COBEC SEB 2017\testeCobec\arquivoQuat.txt";


            //se estiver entre as páginas 0 e 1 (coleta e quaternion)
            if (tabControl1.SelectedTab == tabControl1.TabPages[0] || tabControl1.SelectedTab == tabControl1.TabPages[2])
            {
                calibra = "coletando";
                path = path + "arquivoColeta.txt";
            }
            else if(tabControl1.SelectedTab == tabControl1.TabPages[1])
            {
                path += "arquivoCalibra.txt";
                calibra = "calibrando";
            }           

            //adequando as strings do diretório
            path = path.Replace(@"\", "/");
            path2 = path2.Replace(@"\", "/");

           
            //criando os arquivos de texto
            file = new StreamWriter(path, false);
            fileQuat = new StreamWriter(path2, false);

            //adiciona o horário atual no arquivo
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            fileQuat.WriteLine(timestamp);
            file.WriteLine(timestamp);
            file.WriteLine(calibra);

            //recebe os dados
            Receber();
        }

        private void pararToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //para o recebimento de dados
            Disconnect();
        }

        /// <summary>
        /// função que indica qual o quaternion atual
        /// </summary>
        /// <returns></returns>
        string direcao;
        string QuatAtual()
        {
            selecionaTecla();
            double[] q = quat;
            switch (direcao)
            {
                case "cima":
                    quatCima = q;
                    break;
                case "baixo":
                    quatBaixo = q;
                    break;
                case "esquerda":
                    quatEsquerda = q;
                    break;
                case "direita":
                    quatDireita = q;
                    break;
                case "ref":
                    quatRef = q;
                    break;
            }
            return
                    "\n W: " + q[0].ToString("0.00") +
                    "\n X: " + q[1].ToString("0.00") +
                    "\n Y: " + q[2].ToString("0.00") +
                    "\n Z: " + q[3].ToString("0.00");
        }

        private void btQRef_Click(object sender, EventArgs e)
        {
            direcao = "ref";
            lbQRef.Text = QuatAtual();
        }

        private void btQCima_Click(object sender, EventArgs e)
        {
            direcao = "cima";
            lbQCima.Text = QuatAtual();
        }

        private void btQDireita_Click(object sender, EventArgs e)
        {
            direcao = "direita";
            lbQDireita.Text = QuatAtual();
        }

        private void btQBaixo_Click(object sender, EventArgs e)
        {
            direcao = "baixo";
            lbQBaixo.Text = QuatAtual();
        }

        private void btQEsq_Click(object sender, EventArgs e)
        {
            direcao = "esquerda";
            lbQEsquerda.Text = QuatAtual();
        }

        void selecionaTecla()
        {
            timerTeclas.Start();
            switch (direcao)
            {
                case "cima":
                    this.pictureBoxCima.Invoke(new Action(() => this.pictureBoxCima.Image = Properties.Resources.cimaVerde));
                    break;
                case "baixo":
                    this.pictureBoxBaixo.Invoke(new Action(() => this.pictureBoxBaixo.Image = Properties.Resources.baixoVerde));
                    break;
                case "esquerda":
                    this.pictureBoxEsquerda.Invoke(new Action(() => this.pictureBoxEsquerda.Image = Properties.Resources.esquerdaVerde));
                    break;
                case "direita":
                    this.pictureBoxDireita.Invoke(new Action(() => this.pictureBoxDireita.Image = Properties.Resources.direitaVerde));
                    break;
            }   
            
        }

        private void timerTeclas_Tick(object sender, EventArgs e)
        {            
                timerTeclas.Stop();
                pictureBoxCima.Image = Properties.Resources.cima;
                pictureBoxBaixo.Image = Properties.Resources.baixo;
                pictureBoxDireita.Image = Properties.Resources.direita;
                pictureBoxEsquerda.Image = Properties.Resources.esquerda;

        }

        private void iniciarJogoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("C:/Users/ADRIELLE/Desktop/JOGOdemo.exe");//um executável "não windows" 
        }

        
    }
}
