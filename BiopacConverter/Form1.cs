using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using Biopac.API.AcqFile;
using System.IO;

namespace BiopacConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            CreateFileButton.Enabled = false;
            textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
        }
        AcqFileInfo.ACQFILESTRUCT fileStruct = new AcqFileInfo.ACQFILESTRUCT();

        // information about particular data channel
        AcqFileInfo.CHANNELSTRUCT_A channelStruct = new AcqFileInfo.CHANNELSTRUCT_A();

        // information about marker
        AcqFileInfo.MARKERSTRUCT markerStruct = new AcqFileInfo.MARKERSTRUCT();

        List<List<double>> channels = new List<List<double>>();

        string outputDirectory = "";
        public decimal progress = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            bool bResult;
            CreateFileButton.Enabled = false;

            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                bResult = AcqFileInfo.initACQFile_A(openFileDialog1.FileName, ref fileStruct);
                if (!bResult)
                {
                    MessageBox.Show("Cannot initialize data file.");
                    return;
                }

                if (fileStruct.numChannels > 0)
                {
                    // load information about channel #0

                    for(int c = 0; c < fileStruct.numChannels; c++)
                    {
                        textBox1.AppendText("Processing channel " + c);
                        List<double> samples = new List<double>();
                        bResult = AcqFileInfo.getChannelInfo_A(c, ref fileStruct, ref channelStruct);
                        if (!bResult)
                        {
                            MessageBox.Show("Cannot get channel info.");
                            return;
                        }


                        if (channelStruct.numSamples > 0)
                        {
                            // get all samples of the channel #0
                            // allocate array to store double values of the data samples


                            int blockSize = 1000000;

                            int numSamples = channelStruct.numSamples;

                            // load data samples 
                            for (int i = 0; i < channelStruct.numSamples; i += blockSize)
                            {
                                int samplesLeft = numSamples - i;
                                if (blockSize - samplesLeft >= 0)
                                {
                                    blockSize = samplesLeft - 1;
                                }

                                if (blockSize == 0)
                                {
                                    progressBar1.Value = 100;
                                    break;
                                }

                                double[] segment = new double[blockSize];
                                bResult = AcqFileInfo.getSampleSegment(ref fileStruct, ref channelStruct, segment, i, i + blockSize);
                                if (!bResult)
                                {
                                    MessageBox.Show("Failed to get " + blockSize + " samples ...");
                                    bResult = AcqFileInfo.closeACQFile(ref fileStruct);
                                    return;
                                }

                                for (int j = 0; j < blockSize; j++)
                                {
                                    samples.Add(segment[j]);
                                }
                                progress = (decimal)i / (decimal)numSamples;
                                progressBar1.Value = (int)(progress * 100);
                            }
                            channels.Add(samples);

                        }
                    }
                    
                }

                CreateFileButton.Enabled = true;
            }
        }

        private void CreateFileButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    outputDirectory = fbd.SelectedPath;
                }
            }

            using (StreamWriter outputFile = new StreamWriter(outputDirectory + "\\" + openFileDialog1.SafeFileName + ".csv"))
            {
                string titleLine = "";
                for (int j = 0; j < channels.Count(); j++)
                {
                    titleLine += "Channel " + j + ",";
                }
                outputFile.WriteLine(titleLine);

                for (int i = 0; i < channels[0].Count(); i++)
                {
                    string line = "";
                    for(int j = 0;  j< channels.Count(); j++)
                    {
                        line += channels[j][i] + ",";
                    }
                    outputFile.WriteLine(line);
                }
                textBox1.AppendText("DONE!!");
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
