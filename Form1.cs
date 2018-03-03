using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO ; //引用System.IO命名空間

namespace ReadWriteText
{
    public partial class Form1 : Form
    {

        //public ushort[] Data = new ushort[0x4000];
        public Byte[] Data = new Byte[0x40000];

        public uint FileLen = 0;

        public Form1()
        {
            InitializeComponent();
        }


        private void btnRead_Click(object sender, EventArgs e)
        {
            uint i=0;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo f = new FileInfo(openFileDialog1 .FileName );
                FileStream input = new FileStream(openFileDialog1.FileName, FileMode.Open);
                BinaryReader reader = new BinaryReader(input);

                 i=0;

                 while (reader.BaseStream.Position < reader.BaseStream.Length)
                 {
                     Data[i]=reader.ReadByte();
                     i++;
                 }

                 FileLen = i;
   
                 richTextBox1.Text += "0x";
                 richTextBox1.Text += Convert.ToString(i,16);
                 richTextBox1.Text += "  讀檔成功\n";

                 reader.Close();
                 input.Close();
                //MessageBox.Show("讀檔成功");
            }
        }

        private void btnWrite_Click(object sender, EventArgs e)
        {
            uint i = 0 , j = 0;
            string hexOutput = "";
            string AsciiOutput = "";
            string addrOutput = "";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK )
            {
                FileInfo f = new FileInfo(saveFileDialog1.FileName );
                FileStream output = File.Create(saveFileDialog1.FileName);
                //BinaryWriter writer = new BinaryWriter(output);
                StreamWriter writer = new StreamWriter(output);

                writer.WriteLine("const char Table[] {");

                i = 68;
                while (i < FileLen)
                {
                    if ((j % 10) == 0)
                        writer.Write("    ");
                    
                    writer.Write("0x");
                    hexOutput = String.Format("{0:X}", Data[i]);
                    if (hexOutput.Length == 1)
                        hexOutput = "0" + hexOutput;
                    writer.Write(hexOutput);
                    //writer.Write(Data[i])
                    writer.Write(" ,");

                    if (Data[i] >= 0x21 && Data[i] <= 0x7f)
                        AsciiOutput += Char.ConvertFromUtf32(Data[i]);
                    else
                        AsciiOutput += ".";

                    i++;
                    j++;



                    if ((j % 10) == 0)
                    {
                        writer.Write("   // ");

                        //addrOutput = String.Format("{0:X}", i);
                        addrOutput = (j/10).ToString();

                        while (addrOutput.Length < 8)
                            addrOutput = "0" + addrOutput;
                        writer.Write(addrOutput);

                        writer.Write("    ");
                        writer.Write(AsciiOutput);
                        AsciiOutput = "";
                        writer.Write(Environment.NewLine);
                    }
//                    else
//                    if ((j % 10) == 0)
//                    {
//                        writer.Write(Environment.NewLine);
//                    }
                }

                writer.WriteLine("}");

                richTextBox1.Text += "寫入成功\n";

                writer.Flush();
                writer.Close();
                output.Close();
                //MessageBox.Show("寫入成功");
            }
        }

        private void btnCls_Click(object sender, EventArgs e)
        {
            uint i=0;
            int j = 0;
            Byte[] b = new Byte[20];                       

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo f = new FileInfo(openFileDialog1.FileName);
                FileStream input = new FileStream(openFileDialog1.FileName, FileMode.Open);
                BinaryReader reader = new BinaryReader(input);

                i = 0x1000;

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    Data[i] = reader.ReadByte();
                    i++;
                }

                richTextBox1.Text += "0x";
                richTextBox1.Text += Convert.ToString(i-0x1000, 16);
                richTextBox1.Text += "  讀檔成功\n";

/*
                //b = Encoding.UTF8.GetBytes(textBox1.Text.Substring(0, textBox1.Text.Length));
                b = System.Text.Encoding.Default.GetBytes(textBox1.Text.Substring(0, textBox1.Text.Length));

                j = 0;
                while (j < textBox1.Text.Length)
                {
                    Data[16368 + j] = b[j];
                    j++;
                }
*/
                reader.Close();
                input.Close();
                //MessageBox.Show("讀檔成功");
            }
        }
    }
}
