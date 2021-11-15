using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Firebase.Storage;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Net;

namespace WindowsForms_Plaque
{
    public partial class Form1 : Form
    {
        
        //Firebase
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "dOWStR25kASgKll7A2RxEb9ekI6IDyaRxXQQJSNw",
            BasePath = "https://plaque-assay-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };
        IFirebaseClient client;

        string imgRFolder = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result";
        string Time;
        //Image
        string imgFile;
        HObject ho_Image;
        HObject ho_RImage;
        HTuple Width, Height;
        HTuple hv_ImageFiles = new HTuple();
        HTuple hv_ImgName;
        HObject ho_Result;
        HObject ho_Resultk;
        HObject ho_ROI;
        HTuple num_Result, num_Resultk;
        int hv_Diff;
        int Silhouette_bool;
        int num_All;
        int Mod_Index = 1;
        bool Loaded;
        int multiX;
        int multiY;
        
        int Index;
        int NonIndex;
        int num_Image;
        int StatusR;
        HTuple folderPath = "";

        //Call Procedure
        HDevEngine myEngine = new HDevEngine();
        private string ProcedurePathString = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Setup";
        private HDevProcedureCall myProcedureCall; // Procedure
        private HDevProcedureCall myProcedureCall1; // Procedure2

        //Parameter
        private string Param_file = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Setup/Parameter.txt";
        HTuple Radius_T;
        HTuple Grid_Size;
        HTuple Radius_S;
        HTuple Radius_K;

        public Form1()
        {
            InitializeComponent();

            Time = "";

            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_Result);
            HOperatorSet.GenEmptyObj(out ho_Resultk);
            HOperatorSet.GenEmptyObj(out ho_ROI);
            Width = 0;
            Height = 0;
            Silhouette_bool = 0;

            Index = 0;
            NonIndex = 0;
            num_Image = 0;
            StatusR = 0;

            Radius_T = 100;
            Grid_Size = 17;
            Radius_S = 70;
            Radius_K = 150;
            num_Result = 0;
            num_Resultk = 0;
            imgFile = "";
            hv_Diff = 0;

            multiX = 1;
            multiY = 1;

            Loaded = false;
            Run_btn.Enabled = false;
            Sparameter_btn.Enabled = false;

            pA1.Enabled = false;
            pA2.Enabled = false;
            pA3.Enabled = false;
            pA4.Enabled = false;
            pA5.Enabled = false;
            pA6.Enabled = false;
            pA7.Enabled = false;
            pA8.Enabled = false;
            pA9.Enabled = false;
            pA10.Enabled = false;
            pA11.Enabled = false;
            pA12.Enabled = false;
            pB1.Enabled = false;
            pB2.Enabled = false;
            pB3.Enabled = false;
            pB4.Enabled = false;
            pB5.Enabled = false;
            pB6.Enabled = false;
            pB7.Enabled = false;
            pB8.Enabled = false;
            pB9.Enabled = false;
            pB10.Enabled = false;
            pB11.Enabled = false;
            pB12.Enabled = false;
            pC1.Enabled = false;
            pC2.Enabled = false;
            pC3.Enabled = false;
            pC4.Enabled = false;
            pC5.Enabled = false;
            pC6.Enabled = false;
            pC7.Enabled = false;
            pC8.Enabled = false;
            pC9.Enabled = false;
            pC10.Enabled = false;
            pC11.Enabled = false;
            pC12.Enabled = false;
            pD1.Enabled = false;
            pD2.Enabled = false;
            pD3.Enabled = false;
            pD4.Enabled = false;
            pD5.Enabled = false;
            pD6.Enabled = false;
            pD7.Enabled = false;
            pD8.Enabled = false;
            pD9.Enabled = false;
            pD10.Enabled = false;
            pD11.Enabled = false;
            pD12.Enabled = false;
            pE1.Enabled = false;
            pE2.Enabled = false;
            pE3.Enabled = false;
            pE4.Enabled = false;
            pE5.Enabled = false;
            pE6.Enabled = false;
            pE7.Enabled = false;
            pE8.Enabled = false;
            pE9.Enabled = false;
            pE10.Enabled = false;
            pE11.Enabled = false;
            pE12.Enabled = false;
            pF1.Enabled = false;
            pF2.Enabled = false;
            pF3.Enabled = false;
            pF4.Enabled = false;
            pF5.Enabled = false;
            pF6.Enabled = false;
            pF7.Enabled = false;
            pF8.Enabled = false;
            pF9.Enabled = false;
            pF10.Enabled = false;
            pF11.Enabled = false;
            pF12.Enabled = false;
            pG1.Enabled = false;
            pG2.Enabled = false;
            pG3.Enabled = false;
            pG4.Enabled = false;
            pG5.Enabled = false;
            pG6.Enabled = false;
            pG7.Enabled = false;
            pG8.Enabled = false;
            pG9.Enabled = false;
            pG10.Enabled = false;
            pG11.Enabled = false;
            pG12.Enabled = false;
            pH1.Enabled = false;
            pH2.Enabled = false;
            pH3.Enabled = false;
            pH4.Enabled = false;
            pH5.Enabled = false;
            pH6.Enabled = false;
            pH7.Enabled = false;
            pH8.Enabled = false;
            pH9.Enabled = false;
            pH10.Enabled = false;
            pH11.Enabled = false;
            pH12.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Firebase
            client = new FireSharp.FirebaseClient(config);
            if (CheckForInternetConnection())
            {
                Status_lab.Text = "FireBase connected";
            }
            else
            {
                Status_lab.Text = "FireBase does not connected";
            }

            myEngine.SetProcedurePath(ProcedurePathString);
            HDevProcedure Procedure = new HDevProcedure("count_plaque");
            myProcedureCall = new HDevProcedureCall(Procedure);
            HDevProcedure Procedure2 = new HDevProcedure("Modify_ROI");
            myProcedureCall1 = new HDevProcedureCall(Procedure2);

            Linechart.ChartAreas[0].AxisX.Maximum = 12;
            Linechart.ChartAreas[0].AxisX.Minimum = 0;
            Linechart.ChartAreas[0].AxisY.Maximum = Double.NaN;
            Linechart.ChartAreas[0].AxisY.Minimum = 0;
            Linechart.Series[0].Points.AddXY(0, 0);
            Linechart.Series[1].Points.AddXY(0, 0);
            Linechart.Series[2].Points.AddXY(0, 0);
            Linechart.Series[3].Points.AddXY(0, 0);
            Linechart.Series[4].Points.AddXY(0, 0);
            Linechart.Series[5].Points.AddXY(0, 0);
            Linechart.Series[6].Points.AddXY(0, 0);
            Linechart.Series[7].Points.AddXY(0, 0);
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private async void Upload(int mode ,Uploaddata Object)
        {
            try
            {
                if (chDis.Checked == false)
                {
                    if (mode == 1)
                    {
                        //var stream = File.Open(@"C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Setup/Image.jpg", System.IO.FileMode.Open);
                        var stream = File.Open(Object.Image, System.IO.FileMode.Open);
                        var downloadUrl = "";
                        var task = new FirebaseStorage("plaque-assay.appspot.com").Child(Time).Child(Object.ImageName).PutAsync(stream);
                        Console.WriteLine(Index + hv_ImgName.ToString());
                        try
                        {
                            downloadUrl = await task;
                        }
                        catch { Console.WriteLine(downloadUrl); }
                        Console.WriteLine(downloadUrl);
                        Console.WriteLine(Index + hv_ImgName.ToString());
                        var Plaque_count = new Plaqueassay
                        {
                            pid = Time + " " + Object.ImageName,
                            count = Object.count,
                            img = downloadUrl
                        };
                        SetResponse response = await client.SetAsync("Plaque/" + Time + "/" + "Seperate/" + Object.ImageName, Plaque_count);
                    }
                    else
                    {
                        //var stream = File.Open(@"C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Setup/ImageAll.jpg", System.IO.FileMode.Open);
                        var stream = File.Open(Object.Image, System.IO.FileMode.Open);
                        var downloadUrl = "";
                        var task = new FirebaseStorage("plaque-assay.appspot.com").Child(Time).Child("All").PutAsync(stream);
                        try
                        {
                            downloadUrl = await task;
                        }
                        catch { Console.WriteLine(downloadUrl); }

                        var Plaque_count = new Plaqueassay
                        {
                            pid = Time + "  All",
                            count = Object.count,
                            img = downloadUrl
                        };
                        SetResponse response = await client.SetAsync("Plaque/" + Time + "/" + "All/", Plaque_count);
                    }
                }
            }
            catch
            {

            }
            
        }

        private void Lparameter_btn_Click(object sender, EventArgs e)
        {
            Load_param();
            Sparameter_btn.Enabled = true;
        }

        private void Load_param()
        {
            Loaded = true;
            string[] lines = System.IO.File.ReadAllLines(Param_file);
            foreach (string line in lines)
            {
                var cols = line.Split(' ');
                Radius_T = cols[0];
                Grid_Size = cols[1];
                Radius_S = cols[2];
                Radius_K = cols[3];
            }
            RT_txt.Text = Radius_T;
            GS_txt.Text = Grid_Size;
            RS_txt.Text = Radius_S;
            RK_txt.Text = Radius_K;
        }

        private void Sparameter_btn_Click(object sender, EventArgs e)
        {
            Sparameter_btn.Enabled = false;
            Radius_T = Convert.ToDouble(RT_txt.Text);
            Grid_Size = Convert.ToDouble(GS_txt.Text);
            Radius_S = Convert.ToDouble(RS_txt.Text);
            Radius_K = Convert.ToDouble(RK_txt.Text);
            string write = Radius_T + " " + Grid_Size + " " + Radius_S + " " + Radius_K;
            System.IO.File.WriteAllText(Param_file,write);
        }

        private void DisplayImg()
        {
            HOperatorSet.ReadImage(out ho_Image, imgFile);
            HOperatorSet.GetImageSize(ho_Image,out Width,out Height);
            HOperatorSet.SetPart(hWinCon.HalconWindow, 0, 0, Height, Width);
            HOperatorSet.DispObj(ho_Image, hWinCon.HalconWindow);
        }

        #region Tumbnail Click
        private void pA1_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A1.jpg";
            DisplayImg();
            Count_lab.Text = A1_lab.Text.ToString();
        }

        private void pA2_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A2.jpg";
            DisplayImg();
            Count_lab.Text = A2_lab.Text.ToString();
        }
        private void pA3_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A3.jpg";
            DisplayImg();
            Count_lab.Text = A3_lab.Text.ToString();
        }

        private void pA4_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A4.jpg";
            DisplayImg();
            Count_lab.Text = A4_lab.Text.ToString();
        }

        private void pA5_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A5.jpg";
            DisplayImg();
            Count_lab.Text = A5_lab.Text.ToString();
        }

        private void pA6_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A6.jpg";
            DisplayImg();
            Count_lab.Text = A6_lab.Text.ToString();
        }

        private void pA7_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A7.jpg";
            DisplayImg();
            Count_lab.Text = A7_lab.Text.ToString();
        }

        private void pA8_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A8.jpg";
            DisplayImg();
            Count_lab.Text = A8_lab.Text.ToString();
        }

        private void pA9_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A9.jpg";
            DisplayImg();
            Count_lab.Text = A9_lab.Text.ToString();
        }

        private void pA10_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A10.jpg";
            DisplayImg();
            Count_lab.Text = A10_lab.Text.ToString();
        }

        private void pA11_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A11.jpg";
            DisplayImg();
            Count_lab.Text = A11_lab.Text.ToString();
        }

        private void pA12_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/A12.jpg";
            DisplayImg();
            Count_lab.Text = A12_lab.Text.ToString();
        }

        private void pB1_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B1.jpg";
            DisplayImg();
            Count_lab.Text = B1_lab.Text.ToString();
        }

        private void pB2_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B2.jpg";
            DisplayImg();
            Count_lab.Text = B2_lab.Text.ToString();
        }

        private void pB3_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B3.jpg";
            DisplayImg();
            Count_lab.Text = B3_lab.Text.ToString();
        }

        private void pB4_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B4.jpg";
            DisplayImg();
            Count_lab.Text = B4_lab.Text.ToString();
        }

        private void pB5_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B5.jpg";
            DisplayImg();
            Count_lab.Text = B5_lab.Text.ToString();
        }

        private void pB6_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B6.jpg";
            DisplayImg();
            Count_lab.Text = B6_lab.Text.ToString();
        }

        private void pB7_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B7.jpg";
            DisplayImg();
            Count_lab.Text = B7_lab.Text.ToString();
        }

        private void pB8_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B8.jpg";
            DisplayImg();
            Count_lab.Text = B8_lab.Text.ToString();
        }

        private void pB9_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B9.jpg";
            DisplayImg();
            Count_lab.Text = B9_lab.Text.ToString();
        }

        private void pB10_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B10.jpg";
            DisplayImg();
            Count_lab.Text = B10_lab.Text.ToString();
        }

        private void pB11_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B11.jpg";
            DisplayImg();
            Count_lab.Text = B11_lab.Text.ToString();
        }

        private void pB12_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/B12.jpg";
            DisplayImg();
            Count_lab.Text = B12_lab.Text.ToString();
        }

        private void pC1_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C1.jpg";
            DisplayImg();
            Count_lab.Text = C1_lab.Text.ToString();
        }

        private void pC2_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C2.jpg";
            DisplayImg();
            Count_lab.Text = C2_lab.Text.ToString();
        }

        private void pC3_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C3.jpg";
            DisplayImg();
            Count_lab.Text = C3_lab.Text.ToString();
        }

        private void pC4_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C4.jpg";
            DisplayImg();
            Count_lab.Text = C4_lab.Text.ToString();
        }

        private void pC5_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C5.jpg";
            DisplayImg();
            Count_lab.Text = C5_lab.Text.ToString();
        }

        private void pC6_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C6.jpg";
            DisplayImg();
            Count_lab.Text = C6_lab.Text.ToString();
        }

        private void pC7_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C7.jpg";
            DisplayImg();
            Count_lab.Text = C7_lab.Text.ToString();
        }

        private void pC8_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C8.jpg";
            DisplayImg();
            Count_lab.Text = C8_lab.Text.ToString();
        }

        private void pC9_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C9.jpg";
            DisplayImg();
            Count_lab.Text = C9_lab.Text.ToString();
        }

        private void pC10_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C10.jpg";
            DisplayImg();
            Count_lab.Text = C10_lab.Text.ToString();
        }

        private void pC11_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C11.jpg";
            DisplayImg();
            Count_lab.Text = C11_lab.Text.ToString();
        }

        private void pC12_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/C12.jpg";
            DisplayImg();
            Count_lab.Text = C12_lab.Text.ToString();
        }

        private void pD1_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D1.jpg";
            DisplayImg();
            Count_lab.Text = D1_lab.Text.ToString();
        }

        private void pD2_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D2.jpg";
            DisplayImg();
            Count_lab.Text = D2_lab.Text.ToString();
        }

        private void pD3_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D3.jpg";
            DisplayImg();
            Count_lab.Text = D3_lab.Text.ToString();
        }

        private void pD4_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D4.jpg";
            DisplayImg();
            Count_lab.Text = D4_lab.Text.ToString();
        }

        private void pD5_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D5.jpg";
            DisplayImg();
            Count_lab.Text = D5_lab.Text.ToString();
        }

        private void pD6_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D6.jpg";
            DisplayImg();
            Count_lab.Text = D6_lab.Text.ToString();
        }

        private void pD7_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D7.jpg";
            DisplayImg();
            Count_lab.Text = D7_lab.Text.ToString();
        }

        private void pD8_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D8.jpg";
            DisplayImg();
            Count_lab.Text = D8_lab.Text.ToString();
        }

        private void pD9_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D9.jpg";
            DisplayImg();
            Count_lab.Text = D9_lab.Text.ToString();
        }

        private void pD10_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D10.jpg";
            DisplayImg();
            Count_lab.Text = D10_lab.Text.ToString();
        }

        private void pD11_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D11.jpg";
            DisplayImg();
            Count_lab.Text = D11_lab.Text.ToString();
        }

        private void pD12_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/D12.jpg";
            DisplayImg();
            Count_lab.Text = D12_lab.Text.ToString();
        }

        private void pE1_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E1.jpg";
            DisplayImg();
            Count_lab.Text = E1_lab.Text.ToString();
        }

        private void pE2_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E2.jpg";
            DisplayImg();
            Count_lab.Text = E2_lab.Text.ToString();
        }

        private void pE3_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E3.jpg";
            DisplayImg();
            Count_lab.Text = E3_lab.Text.ToString();
        }

        private void pE4_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E4.jpg";
            DisplayImg();
            Count_lab.Text = E4_lab.Text.ToString();
        }

        private void pE5_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E5.jpg";
            DisplayImg();
            Count_lab.Text = E5_lab.Text.ToString();
        }

        private void pE6_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E6.jpg";
            DisplayImg();
            Count_lab.Text = E6_lab.Text.ToString();
        }

        private void pE7_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E7.jpg";
            DisplayImg();
            Count_lab.Text = E7_lab.Text.ToString();
        }

        private void pE8_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E8.jpg";
            DisplayImg();
            Count_lab.Text = E8_lab.Text.ToString();
        }

        private void pE9_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E9.jpg";
            DisplayImg();
            Count_lab.Text = E9_lab.Text.ToString();
        }

        private void pE10_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E10.jpg";
            DisplayImg();
            Count_lab.Text = E10_lab.Text.ToString();
        }

        private void pE11_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E11.jpg";
            DisplayImg();
            Count_lab.Text = E11_lab.Text.ToString();
        }

        private void pE12_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/E12.jpg";
            DisplayImg();
            Count_lab.Text = E12_lab.Text.ToString();
        }

        private void pF1_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F1.jpg";
            DisplayImg();
            Count_lab.Text = F1_lab.Text.ToString();
        }

        private void pF2_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F2.jpg";
            DisplayImg();
            Count_lab.Text = F2_lab.Text.ToString();
        }

        private void pF3_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F3.jpg";
            DisplayImg();
            Count_lab.Text = F3_lab.Text.ToString();
        }

        private void pF4_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F4.jpg";
            DisplayImg();
            Count_lab.Text = F4_lab.Text.ToString();
        }

        private void pF5_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F5.jpg";
            DisplayImg();
            Count_lab.Text = F5_lab.Text.ToString();
        }

        private void pF6_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F6.jpg";
            DisplayImg();
            Count_lab.Text = F6_lab.Text.ToString();
        }

        private void pF7_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F7.jpg";
            DisplayImg();
            Count_lab.Text = F7_lab.Text.ToString();
        }

        private void pF8_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F8.jpg";
            DisplayImg();
            Count_lab.Text = F8_lab.Text.ToString();
        }

        private void pF9_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F9.jpg";
            DisplayImg();
            Count_lab.Text = F9_lab.Text.ToString();
        }

        private void pF10_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F10.jpg";
            DisplayImg();
            Count_lab.Text = F10_lab.Text.ToString();
        }

        private void pF11_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F11.jpg";
            DisplayImg();
            Count_lab.Text = F11_lab.Text.ToString();
        }

        private void pF12_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/F12.jpg";
            DisplayImg();
            Count_lab.Text = F12_lab.Text.ToString();
        }

        private void pG1_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G1.jpg";
            DisplayImg();
            Count_lab.Text = G1_lab.Text.ToString();
        }

        private void pG2_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G2.jpg";
            DisplayImg();
            Count_lab.Text = G2_lab.Text.ToString();
        }

        private void pG3_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G3.jpg";
            DisplayImg();
            Count_lab.Text = G3_lab.Text.ToString();
        }

        private void pG4_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G4.jpg";
            DisplayImg();
            Count_lab.Text = G4_lab.Text.ToString();
        }

        private void pG5_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G5.jpg";
            DisplayImg();
            Count_lab.Text = G5_lab.Text.ToString();
        }

        private void pG6_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G6.jpg";
            DisplayImg();
            Count_lab.Text = G6_lab.Text.ToString();
        }

        private void pG7_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G7.jpg";
            DisplayImg();
            Count_lab.Text = G7_lab.Text.ToString();
        }

        private void pG8_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G8.jpg";
            DisplayImg();
            Count_lab.Text = G8_lab.Text.ToString();
        }

        private void pG9_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G9.jpg";
            DisplayImg();
            Count_lab.Text = G9_lab.Text.ToString();
        }

        private void pG10_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G10.jpg";
            DisplayImg();
            Count_lab.Text = G10_lab.Text.ToString();
        }

        private void pG11_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G11.jpg";
            DisplayImg();
            Count_lab.Text = G11_lab.Text.ToString();
        }

        private void pG12_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/G12.jpg";
            DisplayImg();
            Count_lab.Text = G12_lab.Text.ToString();
        }
        private void pH1_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H1.jpg";
            DisplayImg();
            Count_lab.Text = H1_lab.Text.ToString();
        }

        private void pH2_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H2.jpg";
            DisplayImg();
            Count_lab.Text = H2_lab.Text.ToString();
        }

        private void pH3_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H3.jpg";
            DisplayImg();
            Count_lab.Text = H3_lab.Text.ToString();
        }

        private void pH4_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H4.jpg";
            DisplayImg();
            Count_lab.Text = H4_lab.Text.ToString();
        }

        private void pH5_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H5.jpg";
            DisplayImg();
            Count_lab.Text = H5_lab.Text.ToString();
        }

        private void pH6_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H6.jpg";
            DisplayImg();
            Count_lab.Text = H6_lab.Text.ToString();
        }

        private void pH7_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H7.jpg";
            DisplayImg();
            Count_lab.Text = H7_lab.Text.ToString();
        }

        private void pH8_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H8.jpg";
            DisplayImg();
            Count_lab.Text = H8_lab.Text.ToString();
        }

        private void pH9_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H9.jpg";
            DisplayImg();
            Count_lab.Text = H9_lab.Text.ToString();
        }

        private void pH10_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H10.jpg";
            DisplayImg();
            Count_lab.Text = H10_lab.Text.ToString();
        }

        private void pH11_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H11.jpg";
            DisplayImg();
            Count_lab.Text = H11_lab.Text.ToString();
        }

        private void pH12_Click(object sender, EventArgs e)
        {
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/H12.jpg";
            DisplayImg();
            Count_lab.Text = H12_lab.Text.ToString();
        }
        #endregion

        private void cmd_txt_TextChanged(object sender, EventArgs e)
        {
            cmd_txt.SelectionStart = cmd_txt.Text.Length;
            // scroll it automatically
            cmd_txt.ScrollToCaret();
        }

        private void RT_txt_TextChanged(object sender, EventArgs e)
        {
            Sparameter_btn.Enabled = true;
        }

        private void GS_txt_TextChanged(object sender, EventArgs e)
        {
            Sparameter_btn.Enabled = true;
        }

        private void RS_txt_TextChanged(object sender, EventArgs e)
        {
            Sparameter_btn.Enabled = true;
        }

        private void RK_txt_TextChanged(object sender, EventArgs e)
        {
            Sparameter_btn.Enabled = true;
        }

        private void BarMod_ValueChanged(object sender, EventArgs e)
        {
            Diff_txt.Text = BarMod.Value.ToString();
            hv_Diff = BarMod.Value;
        }

        private void Mod_btn_Click(object sender, EventArgs e)
        {
            while (true)
            {
                HOperatorSet.ClearWindow(hWinCon.HalconWindow);
                ho_ROI.Dispose();

                BarMod.Value = hv_Diff;
                myProcedureCall1.SetInputCtrlParamTuple("Diff", hv_Diff);

                try
                {
                    myProcedureCall1.Execute();
                }
                catch
                {

                }

                ho_ROI = myProcedureCall1.GetOutputIconicParamObject("ROI");


                HOperatorSet.ReadImage(out ho_Image, "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Setup/Master.tif");
                HOperatorSet.GetImageSize(ho_Image, out Width, out Height);
                HOperatorSet.SetPart(hWinCon.HalconWindow, 0, 0, Height, Width);
                HOperatorSet.DispObj(ho_Image, hWinCon.HalconWindow);

                HOperatorSet.SetColor(hWinCon.HalconWindow, "red");
                HOperatorSet.SetDraw(hWinCon.HalconWindow, "margin");
                HOperatorSet.SetLineWidth(hWinCon.HalconWindow, 3);
                if(Mod_Index != 1)
                {
                    HOperatorSet.WaitSeconds(0.5);
                }
                try
                {
                    HOperatorSet.DispObj(ho_ROI, hWinCon.HalconWindow);
                }
                catch
                {

                }


                if(Mod_Index == 2)
                {
                    break;
                }
                Mod_Index = 2;
            }
        }

        private void PreprocessImage(HObject Image)
        {
            HOperatorSet.ZoomImageSize(Image,out Image, 1280, 1024, "constant");
        }

        private void Run_btn_Click(object sender, EventArgs e)
        {
            chDis.Enabled = false;
            tabControl2.TabPages.Remove(tabPage2);
            tabControl2.TabPages.Add(tabPage2);
            #region Setting Up
            Run_btn.Enabled = false;
            Lparameter_btn.Enabled = false;
            Sparameter_btn.Enabled = false;
            
            Time = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt");

            HOperatorSet.WaitSeconds(0.5);

            DirectoryInfo directory = new DirectoryInfo(imgRFolder);

            foreach (FileInfo file in directory.EnumerateFiles())
            {
                try
                {
                    file.Delete();
                }
                catch
                {

                }
            }

            foreach (DirectoryInfo dir in directory.EnumerateDirectories())
            {
                dir.Delete(true);
            }
            Directory.CreateDirectory(imgRFolder);

            
            #endregion

            HTuple Length = 0;
            HTuple mod = 0;
            multiX = 1;
            multiY = 1;
            if (Loaded == false)
            {
                Load_param();
            }

            while (Index <= num_Image-1)
            {
                ho_Image.Dispose();
                ho_Result.Dispose();
                ho_Resultk.Dispose();
                num_All = 0;
                
                HOperatorSet.ReadImage(out ho_Image, hv_ImageFiles.TupleSelect(Index));
                HOperatorSet.TupleSplit(hv_ImageFiles.TupleSelect(Index),"\\", out hv_ImgName);
                HOperatorSet.TupleLength(hv_ImgName, out Length);
                hv_ImgName = hv_ImgName.TupleSelect(Length-1);
                HOperatorSet.TupleSplit(hv_ImgName, ".",out hv_ImgName);
                hv_ImgName = hv_ImgName.TupleSelect(0);

                PreprocessImage(ho_Image);
                HOperatorSet.GetImageSize(ho_Image, out Width, out Height);
                HOperatorSet.SetPart(hWinCon.HalconWindow, 0, 0, Height, Width);

                if (Silhouette_ch.Checked == true)
                {
                    Silhouette_bool = 1;
                }
                else
                {
                    Silhouette_bool = 0;
                }
                int RT = int.Parse(Radius_T);
                int GS = int.Parse(Grid_Size);
                int RS = int.Parse(Radius_S);
                int RK = int.Parse(Radius_K);

                myProcedureCall.SetInputIconicParamObject(1, ho_Image);
                myProcedureCall.SetInputCtrlParamTuple("Radius", RT);
                myProcedureCall.SetInputCtrlParamTuple("GridSize", GS);
                myProcedureCall.SetInputCtrlParamTuple("RadiusS", RS);
                myProcedureCall.SetInputCtrlParamTuple("RadiusK", RK);
                myProcedureCall.SetInputCtrlParamTuple("Silhouette", Silhouette_bool);
                myProcedureCall.SetInputCtrlParamTuple("Diff", hv_Diff);

                try
                {
                    myProcedureCall.Execute();

                    ho_Result = myProcedureCall.GetOutputIconicParamObject("Result");
                    ho_Resultk = myProcedureCall.GetOutputIconicParamObject("Resultk");
                }
                catch
                {
                    HOperatorSet.GenEmptyObj(out ho_Result);
                    HOperatorSet.GenEmptyObj(out ho_Resultk);
                }

                HOperatorSet.SetDraw(hWinCon.HalconWindow, "margin");
                HOperatorSet.SetLineWidth(hWinCon.HalconWindow, 3);
                HOperatorSet.SetColor(hWinCon.HalconWindow, "red");
                HOperatorSet.DispObj(ho_Image, hWinCon.HalconWindow);
                HOperatorSet.DispObj(ho_Result, hWinCon.HalconWindow);
                HOperatorSet.SetColor(hWinCon.HalconWindow, "yellow");
                HOperatorSet.DispObj(ho_Resultk, hWinCon.HalconWindow);

                HOperatorSet.CountObj(ho_Result, out num_Result);
                HOperatorSet.CountObj(ho_Resultk, out num_Resultk);
                num_All = num_Result + num_Resultk;
                Count_lab.Text = num_All.ToString();

                HOperatorSet.DumpWindow(hWinCon.HalconWindow,"jpeg", "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/"+hv_ImgName+".jpg");
                HOperatorSet.WaitSeconds(0.5);
                HOperatorSet.DumpWindow(hWinCon.HalconWindow, "jpeg", "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Setup/Image.jpg");
                HOperatorSet.WaitSeconds(0.3);
                imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/" + hv_ImgName + ".jpg";
                #region Tumbnail
                if (hv_ImgName == "A1")
                {
                    pA1.Image = new Bitmap(imgFile);
                    pA1.SizeMode = PictureBoxSizeMode.StretchImage;
                    A1_lab.Text = num_All.ToString();
                    pA1.Enabled = true;

                    Linechart.Series[0].Points.AddXY(1, num_All);
                }
                else if(hv_ImgName == "A10")
                {
                    pA10.Image = new Bitmap(imgFile);
                    pA10.SizeMode = PictureBoxSizeMode.StretchImage;
                    A10_lab.Text = num_All.ToString();
                    pA10.Enabled = true;

                    Linechart.Series[0].Points.AddXY(10, num_All);
                }
                else if (hv_ImgName == "A11")
                {
                    pA11.Image = new Bitmap(imgFile);
                    pA11.SizeMode = PictureBoxSizeMode.StretchImage;
                    A11_lab.Text = num_All.ToString();
                    pA11.Enabled = true;

                    Linechart.Series[0].Points.AddXY(11, num_All);
                }
                else if (hv_ImgName == "A12")
                {
                    pA12.Image = new Bitmap(imgFile);
                    pA12.SizeMode = PictureBoxSizeMode.StretchImage;
                    A12_lab.Text = num_All.ToString();
                    pA12.Enabled = true;

                    Linechart.Series[0].Points.AddXY(12, num_All);
                }
                else if (hv_ImgName == "A2")
                {
                    pA2.Image = new Bitmap(imgFile);
                    pA2.SizeMode = PictureBoxSizeMode.StretchImage;
                    A2_lab.Text = num_All.ToString();
                    pA2.Enabled = true;

                    Linechart.Series[0].Points.AddXY(2, num_All);
                }
                else if (hv_ImgName == "A3")
                {
                    pA3.Image = new Bitmap(imgFile);
                    pA3.SizeMode = PictureBoxSizeMode.StretchImage;
                    A3_lab.Text = num_All.ToString();
                    pA3.Enabled = true;

                    Linechart.Series[0].Points.AddXY(3, num_All);
                }
                else if (hv_ImgName == "A4")
                {
                    pA4.Image = new Bitmap(imgFile);
                    pA4.SizeMode = PictureBoxSizeMode.StretchImage;
                    A4_lab.Text = num_All.ToString();
                    pA4.Enabled = true;

                    Linechart.Series[0].Points.AddXY(4, num_All);
                }
                else if (hv_ImgName == "A5")
                {
                    pA5.Image = new Bitmap(imgFile);
                    pA5.SizeMode = PictureBoxSizeMode.StretchImage;
                    A5_lab.Text = num_All.ToString();
                    pA5.Enabled = true;

                    Linechart.Series[0].Points.AddXY(5, num_All);
                }
                else if (hv_ImgName == "A6")
                {
                    pA6.Image = new Bitmap(imgFile);
                    pA6.SizeMode = PictureBoxSizeMode.StretchImage;
                    A6_lab.Text = num_All.ToString();
                    pA6.Enabled = true;

                    Linechart.Series[0].Points.AddXY(6, num_All);
                }
                else if (hv_ImgName == "A7")
                {
                    pA7.Image = new Bitmap(imgFile);
                    pA7.SizeMode = PictureBoxSizeMode.StretchImage;
                    A7_lab.Text = num_All.ToString();
                    pA7.Enabled = true;

                    Linechart.Series[0].Points.AddXY(7, num_All);
                }
                else if (hv_ImgName == "A8")
                {
                    pA8.Image = new Bitmap(imgFile);
                    pA8.SizeMode = PictureBoxSizeMode.StretchImage;
                    A8_lab.Text = num_All.ToString();
                    pA8.Enabled = true;

                    Linechart.Series[0].Points.AddXY(8, num_All);
                }
                else if (hv_ImgName == "A9")
                {
                    pA9.Image = new Bitmap(imgFile);
                    pA9.SizeMode = PictureBoxSizeMode.StretchImage;
                    A9_lab.Text = num_All.ToString();
                    pA9.Enabled = true;

                    Linechart.Series[0].Points.AddXY(9, num_All);
                }
                else if (hv_ImgName == "B1")
                {
                    pB1.Image = new Bitmap(imgFile);
                    pB1.SizeMode = PictureBoxSizeMode.StretchImage;
                    B1_lab.Text = num_All.ToString();
                    pB1.Enabled = true;

                    Linechart.Series[1].Points.AddXY(1, num_All);
                }
                else if (hv_ImgName == "B10")
                {
                    pB10.Image = new Bitmap(imgFile);
                    pB10.SizeMode = PictureBoxSizeMode.StretchImage;
                    B10_lab.Text = num_All.ToString();
                    pB10.Enabled = true;

                    Linechart.Series[1].Points.AddXY(10, num_All);
                }
                else if (hv_ImgName == "B11")
                {
                    pB11.Image = new Bitmap(imgFile);
                    pB11.SizeMode = PictureBoxSizeMode.StretchImage;
                    B11_lab.Text = num_All.ToString();
                    pB11.Enabled = true;

                    Linechart.Series[1].Points.AddXY(11, num_All);
                }
                else if (hv_ImgName == "B12")
                {
                    pB12.Image = new Bitmap(imgFile);
                    pB12.SizeMode = PictureBoxSizeMode.StretchImage;
                    B12_lab.Text = num_All.ToString();
                    pB12.Enabled = true;

                    Linechart.Series[1].Points.AddXY(12, num_All);
                }
                else if (hv_ImgName == "B2")
                {
                    pB2.Image = new Bitmap(imgFile);
                    pB2.SizeMode = PictureBoxSizeMode.StretchImage;
                    B2_lab.Text = num_All.ToString();
                    pB2.Enabled = true;

                    Linechart.Series[1].Points.AddXY(2, num_All);
                }
                else if (hv_ImgName == "B3")
                {
                    pB3.Image = new Bitmap(imgFile);
                    pB3.SizeMode = PictureBoxSizeMode.StretchImage;
                    B3_lab.Text = num_All.ToString();
                    pB3.Enabled = true;

                    Linechart.Series[1].Points.AddXY(3, num_All);
                }
                else if (hv_ImgName == "B4")
                {
                    pB4.Image = new Bitmap(imgFile);
                    pB4.SizeMode = PictureBoxSizeMode.StretchImage;
                    B4_lab.Text = num_All.ToString();
                    pB4.Enabled = true;

                    Linechart.Series[1].Points.AddXY(4, num_All);
                }
                else if (hv_ImgName == "B5")
                {
                    pB5.Image = new Bitmap(imgFile);
                    pB5.SizeMode = PictureBoxSizeMode.StretchImage;
                    B5_lab.Text = num_All.ToString();
                    pB5.Enabled = true;

                    Linechart.Series[1].Points.AddXY(5, num_All);
                }
                else if (hv_ImgName == "B6")
                {
                    pB6.Image = new Bitmap(imgFile);
                    pB6.SizeMode = PictureBoxSizeMode.StretchImage;
                    B6_lab.Text = num_All.ToString();
                    pB6.Enabled = true;

                    Linechart.Series[1].Points.AddXY(6, num_All);
                }
                else if (hv_ImgName == "B7")
                {
                    pB7.Image = new Bitmap(imgFile);
                    pB7.SizeMode = PictureBoxSizeMode.StretchImage;
                    B7_lab.Text = num_All.ToString();
                    pB7.Enabled = true;

                    Linechart.Series[1].Points.AddXY(7, num_All);
                }
                else if (hv_ImgName == "B8")
                {
                    pB8.Image = new Bitmap(imgFile);
                    pB8.SizeMode = PictureBoxSizeMode.StretchImage;
                    B8_lab.Text = num_All.ToString();
                    pB8.Enabled = true;

                    Linechart.Series[1].Points.AddXY(8, num_All);
                }
                else if (hv_ImgName == "B9")
                {
                    pB9.Image = new Bitmap(imgFile);
                    pB9.SizeMode = PictureBoxSizeMode.StretchImage;
                    B9_lab.Text = num_All.ToString();
                    pB9.Enabled = true;

                    Linechart.Series[1].Points.AddXY(9, num_All);
                }
                else if (hv_ImgName == "C1")
                {
                    pC1.Image = new Bitmap(imgFile);
                    pC1.SizeMode = PictureBoxSizeMode.StretchImage;
                    C1_lab.Text = num_All.ToString();
                    pC1.Enabled = true;

                    Linechart.Series[2].Points.AddXY(1, num_All);
                }
                else if (hv_ImgName == "C10")
                {
                    pC10.Image = new Bitmap(imgFile);
                    pC10.SizeMode = PictureBoxSizeMode.StretchImage;
                    C10_lab.Text = num_All.ToString();
                    pC10.Enabled = true;

                    Linechart.Series[2].Points.AddXY(10, num_All);
                }
                else if (hv_ImgName == "C11")
                {
                    pC11.Image = new Bitmap(imgFile);
                    pC11.SizeMode = PictureBoxSizeMode.StretchImage;
                    C11_lab.Text = num_All.ToString();
                    pC11.Enabled = true;

                    Linechart.Series[2].Points.AddXY(11, num_All);
                }
                else if (hv_ImgName == "C12")
                {
                    pC12.Image = new Bitmap(imgFile);
                    pC12.SizeMode = PictureBoxSizeMode.StretchImage;
                    C12_lab.Text = num_All.ToString();
                    pC12.Enabled = true;

                    Linechart.Series[2].Points.AddXY(12, num_All);
                }
                else if (hv_ImgName == "C2")
                {
                    pC2.Image = new Bitmap(imgFile);
                    pC2.SizeMode = PictureBoxSizeMode.StretchImage;
                    C2_lab.Text = num_All.ToString();
                    pC2.Enabled = true;

                    Linechart.Series[2].Points.AddXY(2, num_All);
                }
                else if (hv_ImgName == "C3")
                {
                    pC3.Image = new Bitmap(imgFile);
                    pC3.SizeMode = PictureBoxSizeMode.StretchImage;
                    C3_lab.Text = num_All.ToString();
                    pC3.Enabled = true;

                    Linechart.Series[2].Points.AddXY(3, num_All);
                }
                else if (hv_ImgName == "C4")
                {
                    pC4.Image = new Bitmap(imgFile);
                    pC4.SizeMode = PictureBoxSizeMode.StretchImage;
                    C4_lab.Text = num_All.ToString();
                    pC4.Enabled = true;

                    Linechart.Series[2].Points.AddXY(4, num_All);
                }
                else if (hv_ImgName == "C5")
                {
                    pC5.Image = new Bitmap(imgFile);
                    pC5.SizeMode = PictureBoxSizeMode.StretchImage;
                    C5_lab.Text = num_All.ToString();
                    pC5.Enabled = true;

                    Linechart.Series[2].Points.AddXY(5, num_All);
                }
                else if (hv_ImgName == "C6")
                {
                    pC6.Image = new Bitmap(imgFile);
                    pC6.SizeMode = PictureBoxSizeMode.StretchImage;
                    C6_lab.Text = num_All.ToString();
                    pC6.Enabled = true;

                    Linechart.Series[2].Points.AddXY(6, num_All);
                }
                else if (hv_ImgName == "C7")
                {
                    pC7.Image = new Bitmap(imgFile);
                    pC7.SizeMode = PictureBoxSizeMode.StretchImage;
                    C7_lab.Text = num_All.ToString();
                    pC7.Enabled = true;

                    Linechart.Series[2].Points.AddXY(7, num_All);
                }
                else if (hv_ImgName == "C8")
                {
                    pC8.Image = new Bitmap(imgFile);
                    pC8.SizeMode = PictureBoxSizeMode.StretchImage;
                    C8_lab.Text = num_All.ToString();
                    pC8.Enabled = true;

                    Linechart.Series[2].Points.AddXY(8, num_All);
                }
                else if (hv_ImgName == "C9")
                {
                    pC9.Image = new Bitmap(imgFile);
                    pC9.SizeMode = PictureBoxSizeMode.StretchImage;
                    C9_lab.Text = num_All.ToString();
                    pC9.Enabled = true;

                    Linechart.Series[2].Points.AddXY(9, num_All);
                }
                else if (hv_ImgName == "D1")
                {
                    pD1.Image = new Bitmap(imgFile);
                    pD1.SizeMode = PictureBoxSizeMode.StretchImage;
                    D1_lab.Text = num_All.ToString();
                    pD1.Enabled = true;

                    Linechart.Series[3].Points.AddXY(1, num_All);
                }
                else if (hv_ImgName == "D10")
                {
                    pD10.Image = new Bitmap(imgFile);
                    pD10.SizeMode = PictureBoxSizeMode.StretchImage;
                    D10_lab.Text = num_All.ToString();
                    pD10.Enabled = true;

                    Linechart.Series[3].Points.AddXY(10, num_All);
                }
                else if (hv_ImgName == "D11")
                {
                    pD11.Image = new Bitmap(imgFile);
                    pD11.SizeMode = PictureBoxSizeMode.StretchImage;
                    D11_lab.Text = num_All.ToString();
                    pD11.Enabled = true;

                    Linechart.Series[3].Points.AddXY(11, num_All);
                }
                else if (hv_ImgName == "D12")
                {
                    pD12.Image = new Bitmap(imgFile);
                    pD12.SizeMode = PictureBoxSizeMode.StretchImage;
                    D12_lab.Text = num_All.ToString();
                    pD12.Enabled = true;

                    Linechart.Series[3].Points.AddXY(12, num_All);
                }
                else if (hv_ImgName == "D2")
                {
                    pD2.Image = new Bitmap(imgFile);
                    pD2.SizeMode = PictureBoxSizeMode.StretchImage;
                    D2_lab.Text = num_All.ToString();
                    pD2.Enabled = true;

                    Linechart.Series[3].Points.AddXY(2, num_All);
                }
                else if (hv_ImgName == "D3")
                {
                    pD3.Image = new Bitmap(imgFile);
                    pD3.SizeMode = PictureBoxSizeMode.StretchImage;
                    D3_lab.Text = num_All.ToString();
                    pD3.Enabled = true;

                    Linechart.Series[3].Points.AddXY(3, num_All);
                }
                else if (hv_ImgName == "D4")
                {
                    pD4.Image = new Bitmap(imgFile);
                    pD4.SizeMode = PictureBoxSizeMode.StretchImage;
                    D4_lab.Text = num_All.ToString();
                    pD4.Enabled = true;

                    Linechart.Series[3].Points.AddXY(4, num_All);
                }
                else if (hv_ImgName == "D5")
                {
                    pD5.Image = new Bitmap(imgFile);
                    pD5.SizeMode = PictureBoxSizeMode.StretchImage;
                    D5_lab.Text = num_All.ToString();
                    pD5.Enabled = true;

                    Linechart.Series[3].Points.AddXY(5, num_All);
                }
                else if (hv_ImgName == "D6")
                {
                    pD6.Image = new Bitmap(imgFile);
                    pD6.SizeMode = PictureBoxSizeMode.StretchImage;
                    D6_lab.Text = num_All.ToString();
                    pD6.Enabled = true;

                    Linechart.Series[3].Points.AddXY(6, num_All);
                }
                else if (hv_ImgName == "D7")
                {
                    pD7.Image = new Bitmap(imgFile);
                    pD7.SizeMode = PictureBoxSizeMode.StretchImage;
                    D7_lab.Text = num_All.ToString();
                    pD7.Enabled = true;

                    Linechart.Series[3].Points.AddXY(7, num_All);
                }
                else if (hv_ImgName == "D8")
                {
                    pD8.Image = new Bitmap(imgFile);
                    pD8.SizeMode = PictureBoxSizeMode.StretchImage;
                    D8_lab.Text = num_All.ToString();
                    pD8.Enabled = true;

                    Linechart.Series[3].Points.AddXY(8, num_All);
                }
                else if (hv_ImgName == "D9")
                {
                    pD9.Image = new Bitmap(imgFile);
                    pD9.SizeMode = PictureBoxSizeMode.StretchImage;
                    D9_lab.Text = num_All.ToString();
                    pD9.Enabled = true;

                    Linechart.Series[3].Points.AddXY(9, num_All);
                }
                else if (hv_ImgName == "E1")
                {
                    pE1.Image = new Bitmap(imgFile);
                    pE1.SizeMode = PictureBoxSizeMode.StretchImage;
                    E1_lab.Text = num_All.ToString();
                    pE1.Enabled = true;

                    Linechart.Series[4].Points.AddXY(1, num_All);
                }
                else if (hv_ImgName == "E10")
                {
                    pE10.Image = new Bitmap(imgFile);
                    pE10.SizeMode = PictureBoxSizeMode.StretchImage;
                    E10_lab.Text = num_All.ToString();
                    pE10.Enabled = true;

                    Linechart.Series[4].Points.AddXY(10, num_All);
                }
                else if (hv_ImgName == "E11")
                {
                    pE11.Image = new Bitmap(imgFile);
                    pE11.SizeMode = PictureBoxSizeMode.StretchImage;
                    E11_lab.Text = num_All.ToString();
                    pE11.Enabled = true;

                    Linechart.Series[4].Points.AddXY(11, num_All);
                }
                else if (hv_ImgName == "E12")
                {
                    pE12.Image = new Bitmap(imgFile);
                    pE12.SizeMode = PictureBoxSizeMode.StretchImage;
                    E12_lab.Text = num_All.ToString();
                    pE12.Enabled = true;

                    Linechart.Series[4].Points.AddXY(12, num_All);
                }
                else if (hv_ImgName == "E2")
                {
                    pE2.Image = new Bitmap(imgFile);
                    pE2.SizeMode = PictureBoxSizeMode.StretchImage;
                    E2_lab.Text = num_All.ToString();
                    pE2.Enabled = true;

                    Linechart.Series[4].Points.AddXY(2, num_All);
                }
                else if (hv_ImgName == "E3")
                {
                    pE3.Image = new Bitmap(imgFile);
                    pE3.SizeMode = PictureBoxSizeMode.StretchImage;
                    E3_lab.Text = num_All.ToString();
                    pE3.Enabled = true;

                    Linechart.Series[4].Points.AddXY(3, num_All);
                }
                else if (hv_ImgName == "E4")
                {
                    pE4.Image = new Bitmap(imgFile);
                    pE4.SizeMode = PictureBoxSizeMode.StretchImage;
                    E4_lab.Text = num_All.ToString();
                    pE4.Enabled = true;

                    Linechart.Series[4].Points.AddXY(4, num_All);
                }
                else if (hv_ImgName == "E5")
                {
                    pE5.Image = new Bitmap(imgFile);
                    pE5.SizeMode = PictureBoxSizeMode.StretchImage;
                    E5_lab.Text = num_All.ToString();
                    pE5.Enabled = true;

                    Linechart.Series[4].Points.AddXY(5, num_All);
                }
                else if (hv_ImgName == "E6")
                {
                    pE6.Image = new Bitmap(imgFile);
                    pE6.SizeMode = PictureBoxSizeMode.StretchImage;
                    E6_lab.Text = num_All.ToString();
                    pE6.Enabled = true;

                    Linechart.Series[4].Points.AddXY(6, num_All);
                }
                else if (hv_ImgName == "E7")
                {
                    pE7.Image = new Bitmap(imgFile);
                    pE7.SizeMode = PictureBoxSizeMode.StretchImage;
                    E7_lab.Text = num_All.ToString();
                    pE7.Enabled = true;

                    Linechart.Series[4].Points.AddXY(7, num_All);
                }
                else if (hv_ImgName == "E8")
                {
                    pE8.Image = new Bitmap(imgFile);
                    pE8.SizeMode = PictureBoxSizeMode.StretchImage;
                    E8_lab.Text = num_All.ToString();
                    pE8.Enabled = true;

                    Linechart.Series[4].Points.AddXY(8, num_All);
                }
                else if (hv_ImgName == "E9")
                {
                    pE9.Image = new Bitmap(imgFile);
                    pE9.SizeMode = PictureBoxSizeMode.StretchImage;
                    E9_lab.Text = num_All.ToString();
                    pE9.Enabled = true;

                    Linechart.Series[4].Points.AddXY(9, num_All);
                }
                else if (hv_ImgName == "F1")
                {
                    pF1.Image = new Bitmap(imgFile);
                    pF1.SizeMode = PictureBoxSizeMode.StretchImage;
                    F1_lab.Text = num_All.ToString();
                    pF1.Enabled = true;

                    Linechart.Series[5].Points.AddXY(1, num_All);
                }
                else if (hv_ImgName == "F10")
                {
                    pF10.Image = new Bitmap(imgFile);
                    pF10.SizeMode = PictureBoxSizeMode.StretchImage;
                    F10_lab.Text = num_All.ToString();
                    pF10.Enabled = true;

                    Linechart.Series[5].Points.AddXY(10, num_All);
                }
                else if (hv_ImgName == "F11")
                {
                    pF11.Image = new Bitmap(imgFile);
                    pF11.SizeMode = PictureBoxSizeMode.StretchImage;
                    F11_lab.Text = num_All.ToString();
                    pF11.Enabled = true;

                    Linechart.Series[5].Points.AddXY(11, num_All);
                }
                else if (hv_ImgName == "F12")
                {
                    pF12.Image = new Bitmap(imgFile);
                    pF12.SizeMode = PictureBoxSizeMode.StretchImage;
                    F12_lab.Text = num_All.ToString();
                    pF12.Enabled = true;

                    Linechart.Series[5].Points.AddXY(12, num_All);
                }
                else if (hv_ImgName == "F2")
                {
                    pF2.Image = new Bitmap(imgFile);
                    pF2.SizeMode = PictureBoxSizeMode.StretchImage;
                    F2_lab.Text = num_All.ToString();
                    pF2.Enabled = true;

                    Linechart.Series[5].Points.AddXY(2, num_All);
                }
                else if (hv_ImgName == "F3")
                {
                    pF3.Image = new Bitmap(imgFile);
                    pF3.SizeMode = PictureBoxSizeMode.StretchImage;
                    F3_lab.Text = num_All.ToString();
                    pF3.Enabled = true;

                    Linechart.Series[5].Points.AddXY(3, num_All);
                }
                else if (hv_ImgName == "F4")
                {
                    pF4.Image = new Bitmap(imgFile);
                    pF4.SizeMode = PictureBoxSizeMode.StretchImage;
                    F4_lab.Text = num_All.ToString();
                    pF4.Enabled = true;

                    Linechart.Series[5].Points.AddXY(4, num_All);
                }
                else if (hv_ImgName == "F5")
                {
                    pF5.Image = new Bitmap(imgFile);
                    pF5.SizeMode = PictureBoxSizeMode.StretchImage;
                    F5_lab.Text = num_All.ToString();
                    pF5.Enabled = true;

                    Linechart.Series[5].Points.AddXY(5, num_All);
                }
                else if (hv_ImgName == "F6")
                {
                    pF6.Image = new Bitmap(imgFile);
                    pF6.SizeMode = PictureBoxSizeMode.StretchImage;
                    F6_lab.Text = num_All.ToString();
                    pF6.Enabled = true;

                    Linechart.Series[5].Points.AddXY(6, num_All);
                }
                else if (hv_ImgName == "F7")
                {
                    pF7.Image = new Bitmap(imgFile);
                    pF7.SizeMode = PictureBoxSizeMode.StretchImage;
                    F7_lab.Text = num_All.ToString();
                    pF7.Enabled = true;

                    Linechart.Series[5].Points.AddXY(7, num_All);
                }
                else if (hv_ImgName == "F8")
                {
                    pF8.Image = new Bitmap(imgFile);
                    pF8.SizeMode = PictureBoxSizeMode.StretchImage;
                    F8_lab.Text = num_All.ToString();
                    pF8.Enabled = true;

                    Linechart.Series[5].Points.AddXY(8, num_All);
                }
                else if (hv_ImgName == "F9")
                {
                    pF9.Image = new Bitmap(imgFile);
                    pF9.SizeMode = PictureBoxSizeMode.StretchImage;
                    F9_lab.Text = num_All.ToString();
                    pF9.Enabled = true;

                    Linechart.Series[5].Points.AddXY(9, num_All);
                }
                else if (hv_ImgName == "G1")
                {
                    pG1.Image = new Bitmap(imgFile);
                    pG1.SizeMode = PictureBoxSizeMode.StretchImage;
                    G1_lab.Text = num_All.ToString();
                    pG1.Enabled = true;

                    Linechart.Series[6].Points.AddXY(1, num_All);
                }
                else if (hv_ImgName == "G10")
                {
                    pG10.Image = new Bitmap(imgFile);
                    pG10.SizeMode = PictureBoxSizeMode.StretchImage;
                    G10_lab.Text = num_All.ToString();
                    pG10.Enabled = true;

                    Linechart.Series[6].Points.AddXY(10, num_All);
                }
                else if (hv_ImgName == "G11")
                {
                    pG11.Image = new Bitmap(imgFile);
                    pG11.SizeMode = PictureBoxSizeMode.StretchImage;
                    G11_lab.Text = num_All.ToString();
                    pG11.Enabled = true;

                    Linechart.Series[6].Points.AddXY(11, num_All);
                }
                else if (hv_ImgName == "G12")
                {
                    pG12.Image = new Bitmap(imgFile);
                    pG12.SizeMode = PictureBoxSizeMode.StretchImage;
                    G12_lab.Text = num_All.ToString();
                    pG12.Enabled = true;

                    Linechart.Series[6].Points.AddXY(12, num_All);
                }
                else if (hv_ImgName == "G2")
                {
                    pG2.Image = new Bitmap(imgFile);
                    pG2.SizeMode = PictureBoxSizeMode.StretchImage;
                    G2_lab.Text = num_All.ToString();
                    pG2.Enabled = true;

                    Linechart.Series[6].Points.AddXY(2, num_All);
                }
                else if (hv_ImgName == "G3")
                {
                    pG3.Image = new Bitmap(imgFile);
                    pG3.SizeMode = PictureBoxSizeMode.StretchImage;
                    G3_lab.Text = num_All.ToString();
                    pG3.Enabled = true;

                    Linechart.Series[6].Points.AddXY(3, num_All);
                }
                else if (hv_ImgName == "G4")
                {
                    pG4.Image = new Bitmap(imgFile);
                    pG4.SizeMode = PictureBoxSizeMode.StretchImage;
                    G4_lab.Text = num_All.ToString();
                    pG4.Enabled = true;

                    Linechart.Series[6].Points.AddXY(4, num_All);
                }
                else if (hv_ImgName == "G5")
                {
                    pG5.Image = new Bitmap(imgFile);
                    pG5.SizeMode = PictureBoxSizeMode.StretchImage;
                    G5_lab.Text = num_All.ToString();
                    pG5.Enabled = true;

                    Linechart.Series[6].Points.AddXY(5, num_All);
                }
                else if (hv_ImgName == "G6")
                {
                    pG6.Image = new Bitmap(imgFile);
                    pG6.SizeMode = PictureBoxSizeMode.StretchImage;
                    G6_lab.Text = num_All.ToString();
                    pG6.Enabled = true;

                    Linechart.Series[6].Points.AddXY(6, num_All);
                }
                else if (hv_ImgName == "G7")
                {
                    pG7.Image = new Bitmap(imgFile);
                    pG7.SizeMode = PictureBoxSizeMode.StretchImage;
                    G7_lab.Text = num_All.ToString();
                    pG7.Enabled = true;

                    Linechart.Series[6].Points.AddXY(7, num_All);
                }
                else if (hv_ImgName == "G8")
                {
                    pG8.Image = new Bitmap(imgFile);
                    pG8.SizeMode = PictureBoxSizeMode.StretchImage;
                    G8_lab.Text = num_All.ToString();
                    pG8.Enabled = true;

                    Linechart.Series[6].Points.AddXY(8, num_All);
                }
                else if (hv_ImgName == "G9")
                {
                    pG9.Image = new Bitmap(imgFile);
                    pG9.SizeMode = PictureBoxSizeMode.StretchImage;
                    G9_lab.Text = num_All.ToString();
                    pG9.Enabled = true;

                    Linechart.Series[6].Points.AddXY(9, num_All);
                }
                else if (hv_ImgName == "H1")
                {
                    pH1.Image = new Bitmap(imgFile);
                    pH1.SizeMode = PictureBoxSizeMode.StretchImage;
                    H1_lab.Text = num_All.ToString();
                    pH1.Enabled = true;

                    Linechart.Series[7].Points.AddXY(1, num_All);
                }
                else if (hv_ImgName == "H10")
                {
                    pH10.Image = new Bitmap(imgFile);
                    pH10.SizeMode = PictureBoxSizeMode.StretchImage;
                    H10_lab.Text = num_All.ToString();
                    pH10.Enabled = true;

                    Linechart.Series[7].Points.AddXY(10, num_All);
                }
                else if (hv_ImgName == "H11")
                {
                    pH11.Image = new Bitmap(imgFile);
                    pH11.SizeMode = PictureBoxSizeMode.StretchImage;
                    H11_lab.Text = num_All.ToString();
                    pH11.Enabled = true;

                    Linechart.Series[7].Points.AddXY(11, num_All);
                }
                else if (hv_ImgName == "H12")
                {
                    pH12.Image = new Bitmap(imgFile);
                    pH12.SizeMode = PictureBoxSizeMode.StretchImage;
                    H12_lab.Text = num_All.ToString();
                    pH12.Enabled = true;

                    Linechart.Series[7].Points.AddXY(12, num_All);
                }
                else if (hv_ImgName == "H2")
                {
                    pH2.Image = new Bitmap(imgFile);
                    pH2.SizeMode = PictureBoxSizeMode.StretchImage;
                    H2_lab.Text = num_All.ToString();
                    pH2.Enabled = true;

                    Linechart.Series[7].Points.AddXY(2, num_All);
                }
                else if (hv_ImgName == "H3")
                {
                    pH3.Image = new Bitmap(imgFile);
                    pH3.SizeMode = PictureBoxSizeMode.StretchImage;
                    H3_lab.Text = num_All.ToString();
                    pH3.Enabled = true;

                    Linechart.Series[7].Points.AddXY(3, num_All);
                }
                else if (hv_ImgName == "H4")
                {
                    pH4.Image = new Bitmap(imgFile);
                    pH4.SizeMode = PictureBoxSizeMode.StretchImage;
                    H4_lab.Text = num_All.ToString();
                    pH4.Enabled = true;

                    Linechart.Series[7].Points.AddXY(4, num_All);
                }
                else if (hv_ImgName == "H5")
                {
                    pH5.Image = new Bitmap(imgFile);
                    pH5.SizeMode = PictureBoxSizeMode.StretchImage;
                    H5_lab.Text = num_All.ToString();
                    pH5.Enabled = true;

                    Linechart.Series[7].Points.AddXY(5, num_All);
                }
                else if (hv_ImgName == "H6")
                {
                    pH6.Image = new Bitmap(imgFile);
                    pH6.SizeMode = PictureBoxSizeMode.StretchImage;
                    H6_lab.Text = num_All.ToString();
                    pH6.Enabled = true;

                    Linechart.Series[7].Points.AddXY(6, num_All);
                }
                else if (hv_ImgName == "H7")
                {
                    pH7.Image = new Bitmap(imgFile);
                    pH7.SizeMode = PictureBoxSizeMode.StretchImage;
                    H7_lab.Text = num_All.ToString();
                    pH7.Enabled = true;

                    Linechart.Series[7].Points.AddXY(7, num_All);
                }
                else if (hv_ImgName == "H8")
                {
                    pH8.Image = new Bitmap(imgFile);
                    pH8.SizeMode = PictureBoxSizeMode.StretchImage;
                    H8_lab.Text = num_All.ToString();
                    pH8.Enabled = true;

                    Linechart.Series[7].Points.AddXY(8, num_All);
                }
                else if (hv_ImgName == "H9")
                {
                    pH9.Image = new Bitmap(imgFile);
                    pH9.SizeMode = PictureBoxSizeMode.StretchImage;
                    H9_lab.Text = num_All.ToString();
                    pH9.Enabled = true;

                    Linechart.Series[7].Points.AddXY(9, num_All);
                }
                else
                {
                    cmd_txt.AppendText("'"+ hv_ImgName + "'"+ "This name is not correct format.\n");

                    if(NonIndex % 12 != 0)
                    {
                        multiX = 90 + multiX;
                    }
                    else if(NonIndex == 0)
                    {
                        multiX = 0;
                        multiY = 0;
                    }
                    else
                    {
                        multiX = 0;
                        multiY = 100 + multiY;
                    }

                    var picture = new PictureBox
                    {
                        Name = hv_ImgName,
                        Size = new Size(83, 62),
                        Location = new Point(30+multiX, 40+multiY),
                        Image = new Bitmap(imgFile),
                        SizeMode = PictureBoxSizeMode.StretchImage,

                    };
                    var Label = new Label
                    {
                        Name = hv_ImgName + "_lab",
                        Font = new Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                        Location = new Point(65 + multiX, 105 + multiY),
                        Size = new Size(18, 19),
                        Text = num_All.ToString()
                };
                    NonIndex = NonIndex + 1;
                    tabPage2.Controls.Add(picture);
                    tabPage2.Controls.Add(Label);
                    picture.Click += pic_Click;
                    HOperatorSet.WaitSeconds(1);
                }
                #endregion
                if(Index == 0)
                {
                    A1_lab.Refresh();
                }
                
                HOperatorSet.WaitSeconds(0.3);
                if (Index == 0)
                {
                    A1_lab.Refresh();
                }

                var Uploaddata = new Uploaddata
                {
                    UIndex = Index,
                    Image = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Setup/Image.jpg",
                    ImageName = hv_ImgName,
                    count = num_All

                };
                Upload(1,Uploaddata);
                if (Index == 0)
                {
                    A1_lab.Refresh();
                }

                HOperatorSet.WaitSeconds(1);
                StatusR = 1;
                Index = Index + 1;
                Linechart.ChartAreas[0].RecalculateAxesScale();

            }
            Run_btn.Enabled = false;
            Load_btn.Enabled = true;
            this.tabControl1.SelectedTab = tabControl1.TabPages["GraphPage"];
            HOperatorSet.WaitSeconds(1.5);
            Rectangle bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
                }
                bitmap.Save("C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Setup/ImageAll.jpg", ImageFormat.Jpeg);
            }
            HOperatorSet.WaitSeconds(0.3);
            var UploaddataAll = new Uploaddata
            {
                UIndex = Index,
                Image = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Setup/ImageAll.jpg",
                ImageName = "All",
                count = num_Image

            };
            Upload(0,UploaddataAll);
            cmd_txt.AppendText("Done " + num_Image + " images.\n");
            Load_btn.Enabled = true;
            Lparameter_btn.Enabled = true;
            ImgFolder_txt.Text = "";
            chDis.Enabled = true;
        }
        private void pic_Click(object sender, EventArgs e)
        {
            PictureBox oPictureBox = (PictureBox)sender;
            string Name = oPictureBox.Name;
            imgFile = "C:/Solitech/Web App Plaque/Plaque Assay/WindowsForms Plaque/Result/" + Name + ".jpg";
            DisplayImg();
            string num = "-";
            foreach (Control c in tabPage2.Controls)
            {
                if (c.Name == Name + "_lab")
                {
                    num = c.Text;
                }
            }
            Count_lab.Text = num;
        }

        private void Load_btn_Click(object sender, EventArgs e)
        {
            Load_btn.Enabled = false;
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog1.SelectedPath))
            {
                folderPath = folderBrowserDialog1.SelectedPath;
                ImgFolder_txt.Text = folderPath;
            }
            else
            {
                Load_btn.Enabled = true;
                MessageBox.Show("Folder did not select","Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Run_btn.Enabled = false;
                return;
            }
            hv_ImageFiles.Dispose();
            HOperatorSet.ListFiles(folderPath, (new HTuple("files")).TupleConcat("follow_links"), out hv_ImageFiles);

            HTuple ExpTmpOutVar_0;
            HOperatorSet.TupleRegexpSelect(hv_ImageFiles, (new HTuple("\\.(tif|tiff|gif|bmp|jpg|jpeg|jp2|png|pcx|pgm|ppm|pbm|xwd|ima|hobj)$")).TupleConcat(
                "ignore_case"), out ExpTmpOutVar_0);
            hv_ImageFiles.Dispose();
            hv_ImageFiles = ExpTmpOutVar_0;
            string[] Imgfile = hv_ImageFiles;
            var sorted = (from fn in Imgfile
                          let m = Regex.Match(fn, @"(?<order>\d+)")
                          where m.Success
                          let n = int.Parse(m.Groups["order"].Value)
                          orderby n
                          select fn).ToList();

            foreach (var fn in sorted) Console.WriteLine(fn);
            Imgfile = sorted.ToArray();
            hv_ImageFiles = Imgfile;
            num_Image = hv_ImageFiles.TupleLength();
            cmd_txt.AppendText("Number of image is "+num_Image+ ".\n");
            #region Setup
            pA1.Enabled = false;
            pA2.Enabled = false;
            pA3.Enabled = false;
            pA4.Enabled = false;
            pA5.Enabled = false;
            pA6.Enabled = false;
            pA7.Enabled = false;
            pA8.Enabled = false;
            pA9.Enabled = false;
            pA10.Enabled = false;
            pA11.Enabled = false;
            pA12.Enabled = false;
            pB1.Enabled = false;
            pB2.Enabled = false;
            pB3.Enabled = false;
            pB4.Enabled = false;
            pB5.Enabled = false;
            pB6.Enabled = false;
            pB7.Enabled = false;
            pB8.Enabled = false;
            pB9.Enabled = false;
            pB10.Enabled = false;
            pB11.Enabled = false;
            pB12.Enabled = false;
            pC1.Enabled = false;
            pC2.Enabled = false;
            pC3.Enabled = false;
            pC4.Enabled = false;
            pC5.Enabled = false;
            pC6.Enabled = false;
            pC7.Enabled = false;
            pC8.Enabled = false;
            pC9.Enabled = false;
            pC10.Enabled = false;
            pC11.Enabled = false;
            pC12.Enabled = false;
            pD1.Enabled = false;
            pD2.Enabled = false;
            pD3.Enabled = false;
            pD4.Enabled = false;
            pD5.Enabled = false;
            pD6.Enabled = false;
            pD7.Enabled = false;
            pD8.Enabled = false;
            pD9.Enabled = false;
            pD10.Enabled = false;
            pD11.Enabled = false;
            pD12.Enabled = false;
            pE1.Enabled = false;
            pE2.Enabled = false;
            pE3.Enabled = false;
            pE4.Enabled = false;
            pE5.Enabled = false;
            pE6.Enabled = false;
            pE7.Enabled = false;
            pE8.Enabled = false;
            pE9.Enabled = false;
            pE10.Enabled = false;
            pE11.Enabled = false;
            pE12.Enabled = false;
            pF1.Enabled = false;
            pF2.Enabled = false;
            pF3.Enabled = false;
            pF4.Enabled = false;
            pF5.Enabled = false;
            pF6.Enabled = false;
            pF7.Enabled = false;
            pF8.Enabled = false;
            pF9.Enabled = false;
            pF10.Enabled = false;
            pF11.Enabled = false;
            pF12.Enabled = false;
            pG1.Enabled = false;
            pG2.Enabled = false;
            pG3.Enabled = false;
            pG4.Enabled = false;
            pG5.Enabled = false;
            pG6.Enabled = false;
            pG7.Enabled = false;
            pG8.Enabled = false;
            pG9.Enabled = false;
            pG10.Enabled = false;
            pG11.Enabled = false;
            pG12.Enabled = false;
            pH1.Enabled = false;
            pH2.Enabled = false;
            pH3.Enabled = false;
            pH4.Enabled = false;
            pH5.Enabled = false;
            pH6.Enabled = false;
            pH7.Enabled = false;
            pH8.Enabled = false;
            pH9.Enabled = false;
            pH10.Enabled = false;
            pH11.Enabled = false;
            pH12.Enabled = false;

            if (StatusR == 1)
            {
                Index = 0;
                if (pA1.Image != null)
                {
                    pA1.Image.Dispose();
                    pA1.Image = null;
                    A1_lab.Text = "-";
                }
                if (pA2.Image != null)
                {
                    pA2.Image.Dispose();
                    pA2.Image = null;
                    A2_lab.Text = "-";
                }
                if (pA3.Image != null)
                {
                    pA3.Image.Dispose();
                    pA3.Image = null;
                    A3_lab.Text = "-";
                }
                if (pA4.Image != null)
                {
                    pA4.Image.Dispose();
                    pA4.Image = null;
                    A4_lab.Text = "-";
                }
                if (pA5.Image != null)
                {
                    pA5.Image.Dispose();
                    pA5.Image = null;
                    A5_lab.Text = "-";
                }
                if (pA6.Image != null)
                {
                    pA6.Image.Dispose();
                    pA6.Image = null;
                    A6_lab.Text = "-";
                }
                if (pA7.Image != null)
                {
                    pA7.Image.Dispose();
                    pA7.Image = null;
                    A7_lab.Text = "-";
                }
                if (pA8.Image != null)
                {
                    pA8.Image.Dispose();
                    pA8.Image = null;
                    A8_lab.Text = "-";
                }
                if (pA9.Image != null)
                {
                    pA9.Image.Dispose();
                    pA9.Image = null;
                    A9_lab.Text = "-";
                }
                if (pA10.Image != null)
                {
                    pA10.Image.Dispose();
                    pA10.Image = null;
                    A10_lab.Text = "-";
                }
                if (pA11.Image != null)
                {
                    pA11.Image.Dispose();
                    pA11.Image = null;
                    A11_lab.Text = "-";
                }
                if (pA12.Image != null)
                {
                    pA12.Image.Dispose();
                    pA12.Image = null;
                    A12_lab.Text = "-";
                }
                if (pB1.Image != null)
                {
                    pB1.Image.Dispose();
                    pB1.Image = null;
                    B1_lab.Text = "-";
                }
                if (pB2.Image != null)
                {
                    pB2.Image.Dispose();
                    pB2.Image = null;
                    B2_lab.Text = "-";
                }
                if (pB3.Image != null)
                {
                    pB3.Image.Dispose();
                    pB3.Image = null;
                    B3_lab.Text = "-";
                }
                if (pB4.Image != null)
                {
                    pB4.Image.Dispose();
                    pB4.Image = null;
                    B4_lab.Text = "-";
                }
                if (pB5.Image != null)
                {
                    pB5.Image.Dispose();
                    pB5.Image = null;
                    B5_lab.Text = "-";
                }
                if (pB6.Image != null)
                {
                    pB6.Image.Dispose();
                    pB6.Image = null;
                    B6_lab.Text = "-";
                }
                if (pB7.Image != null)
                {
                    pB7.Image.Dispose();
                    pB7.Image = null;
                    B7_lab.Text = "-";
                }
                if (pB8.Image != null)
                {
                    pB8.Image.Dispose();
                    pB8.Image = null;
                    B8_lab.Text = "-";
                }
                if (pB9.Image != null)
                {
                    pB9.Image.Dispose();
                    pB9.Image = null;
                    B9_lab.Text = "-";
                }
                if (pB10.Image != null)
                {
                    pB10.Image.Dispose();
                    pB10.Image = null;
                    B10_lab.Text = "-";
                }
                if (pB11.Image != null)
                {
                    pB11.Image.Dispose();
                    pB11.Image = null;
                    B11_lab.Text = "-";
                }
                if (pB12.Image != null)
                {
                    pB12.Image.Dispose();
                    pB12.Image = null;
                    B12_lab.Text = "-";
                }
                if (pC1.Image != null)
                {
                    pC1.Image.Dispose();
                    pC1.Image = null;
                    C1_lab.Text = "-";
                }
                if (pC2.Image != null)
                {
                    pC2.Image.Dispose();
                    pC2.Image = null;
                    C2_lab.Text = "-";
                }
                if (pC3.Image != null)
                {
                    pC3.Image.Dispose();
                    pC3.Image = null;
                    C3_lab.Text = "-";
                }
                if (pC4.Image != null)
                {
                    pC4.Image.Dispose();
                    pC4.Image = null;
                    C4_lab.Text = "-";
                }
                if (pC5.Image != null)
                {
                    pC5.Image.Dispose();
                    pC5.Image = null;
                    C5_lab.Text = "-";
                }
                if (pC6.Image != null)
                {
                    pC6.Image.Dispose();
                    pC6.Image = null;
                    C6_lab.Text = "-";
                }
                if (pC7.Image != null)
                {
                    pC7.Image.Dispose();
                    pC7.Image = null;
                    C7_lab.Text = "-";
                }
                if (pC8.Image != null)
                {
                    pC8.Image.Dispose();
                    pC8.Image = null;
                    C8_lab.Text = "-";
                }
                if (pC9.Image != null)
                {
                    pC9.Image.Dispose();
                    pC9.Image = null;
                    C9_lab.Text = "-";
                }
                if (pC10.Image != null)
                {
                    pC10.Image.Dispose();
                    pC10.Image = null;
                    C10_lab.Text = "-";
                }
                if (pC11.Image != null)
                {
                    pC11.Image.Dispose();
                    pC11.Image = null;
                    C11_lab.Text = "-";
                }
                if (pC12.Image != null)
                {
                    pC12.Image.Dispose();
                    pC12.Image = null;
                    C12_lab.Text = "-";
                }
                if (pD1.Image != null)
                {
                    pD1.Image.Dispose();
                    pD1.Image = null;
                    D1_lab.Text = "-";
                }
                if (pD2.Image != null)
                {
                    pD2.Image.Dispose();
                    pD2.Image = null;
                    D2_lab.Text = "-";
                }
                if (pD3.Image != null)
                {
                    pD3.Image.Dispose();
                    pD3.Image = null;
                    D3_lab.Text = "-";
                }
                if (pD4.Image != null)
                {
                    pD4.Image.Dispose();
                    pD4.Image = null;
                    D4_lab.Text = "-";
                }
                if (pD5.Image != null)
                {
                    pD5.Image.Dispose();
                    pD5.Image = null;
                    D5_lab.Text = "-";
                }
                if (pD6.Image != null)
                {
                    pD6.Image.Dispose();
                    pD6.Image = null;
                    D6_lab.Text = "-";
                }
                if (pD7.Image != null)
                {
                    pD7.Image.Dispose();
                    pD7.Image = null;
                    D7_lab.Text = "-";
                }
                if (pD8.Image != null)
                {
                    pD8.Image.Dispose();
                    pD8.Image = null;
                    D8_lab.Text = "-";
                }
                if (pD9.Image != null)
                {
                    pD9.Image.Dispose();
                    pD9.Image = null;
                    D9_lab.Text = "-";
                }
                if (pD10.Image != null)
                {
                    pD10.Image.Dispose();
                    pD10.Image = null;
                    D10_lab.Text = "-";
                }
                if (pD11.Image != null)
                {
                    pD11.Image.Dispose();
                    pD11.Image = null;
                    D11_lab.Text = "-";
                }
                if (pD12.Image != null)
                {
                    pD12.Image.Dispose();
                    pD12.Image = null;
                    D12_lab.Text = "-";
                }
                if (pE1.Image != null)
                {
                    pE1.Image.Dispose();
                    pE1.Image = null;
                    E1_lab.Text = "-";
                }
                if (pE2.Image != null)
                {
                    pE2.Image.Dispose();
                    pE2.Image = null;
                    E2_lab.Text = "-";
                }
                if (pE3.Image != null)
                {
                    pE3.Image.Dispose();
                    pE3.Image = null;
                    E3_lab.Text = "-";
                }
                if (pE4.Image != null)
                {
                    pE4.Image.Dispose();
                    pE4.Image = null;
                    E4_lab.Text = "-";
                }
                if (pE5.Image != null)
                {
                    pE5.Image.Dispose();
                    pE5.Image = null;
                    E5_lab.Text = "-";
                }
                if (pE6.Image != null)
                {
                    pE6.Image.Dispose();
                    pE6.Image = null;
                    E6_lab.Text = "-";
                }
                if (pE7.Image != null)
                {
                    pE7.Image.Dispose();
                    pE7.Image = null;
                    E7_lab.Text = "-";
                }
                if (pE8.Image != null)
                {
                    pE8.Image.Dispose();
                    pE8.Image = null;
                    E8_lab.Text = "-";
                }
                if (pE9.Image != null)
                {
                    pE9.Image.Dispose();
                    pE9.Image = null;
                    E9_lab.Text = "-";
                }
                if (pE10.Image != null)
                {
                    pE10.Image.Dispose();
                    pE10.Image = null;
                    E10_lab.Text = "-";
                }
                if (pE11.Image != null)
                {
                    pE11.Image.Dispose();
                    pE11.Image = null;
                    E11_lab.Text = "-";
                }
                if (pE12.Image != null)
                {
                    pE12.Image.Dispose();
                    pE12.Image = null;
                    E12_lab.Text = "-";
                }
                if (pF1.Image != null)
                {
                    pF1.Image.Dispose();
                    pF1.Image = null;
                    F1_lab.Text = "-";
                }
                if (pF2.Image != null)
                {
                    pF2.Image.Dispose();
                    pF2.Image = null;
                    F2_lab.Text = "-";
                }
                if (pF3.Image != null)
                {
                    pF3.Image.Dispose();
                    pF3.Image = null;
                    F3_lab.Text = "-";
                }
                if (pF4.Image != null)
                {
                    pF4.Image.Dispose();
                    pF4.Image = null;
                    F4_lab.Text = "-";
                }
                if (pF5.Image != null)
                {
                    pF5.Image.Dispose();
                    pF5.Image = null;
                    F5_lab.Text = "-";
                }
                if (pF6.Image != null)
                {
                    pF6.Image.Dispose();
                    pF6.Image = null;
                    F6_lab.Text = "-";
                }
                if (pF7.Image != null)
                {
                    pF7.Image.Dispose();
                    pF7.Image = null;
                    F7_lab.Text = "-";
                }
                if (pF8.Image != null)
                {
                    pF8.Image.Dispose();
                    pF8.Image = null;
                    F8_lab.Text = "-";
                }
                if (pF9.Image != null)
                {
                    pF9.Image.Dispose();
                    pF9.Image = null;
                    F9_lab.Text = "-";
                }
                if (pF10.Image != null)
                {
                    pF10.Image.Dispose();
                    pF10.Image = null;
                    F10_lab.Text = "-";
                }
                if (pF11.Image != null)
                {
                    pF11.Image.Dispose();
                    pF11.Image = null;
                    F11_lab.Text = "-";
                }
                if (pF12.Image != null)
                {
                    pF12.Image.Dispose();
                    pF12.Image = null;
                    F12_lab.Text = "-";
                }
                if (pG1.Image != null)
                {
                    pG1.Image.Dispose();
                    pG1.Image = null;
                    G1_lab.Text = "-";
                }
                if (pG2.Image != null)
                {
                    pG2.Image.Dispose();
                    pG2.Image = null;
                    G2_lab.Text = "-";
                }
                if (pG3.Image != null)
                {
                    pG3.Image.Dispose();
                    pG3.Image = null;
                    G3_lab.Text = "-";
                }
                if (pG4.Image != null)
                {
                    pG4.Image.Dispose();
                    pG4.Image = null;
                    G4_lab.Text = "-";
                }
                if (pG5.Image != null)
                {
                    pG5.Image.Dispose();
                    pG5.Image = null;
                    G5_lab.Text = "-";
                }
                if (pG6.Image != null)
                {
                    pG6.Image.Dispose();
                    pG6.Image = null;
                    G6_lab.Text = "-";
                }
                if (pG7.Image != null)
                {
                    pG7.Image.Dispose();
                    pG7.Image = null;
                    G7_lab.Text = "-";
                }
                if (pG8.Image != null)
                {
                    pG8.Image.Dispose();
                    pG8.Image = null;
                    G8_lab.Text = "-";
                }
                if (pG9.Image != null)
                {
                    pG9.Image.Dispose();
                    pG9.Image = null;
                    G9_lab.Text = "-";
                }
                if (pG10.Image != null)
                {
                    pG10.Image.Dispose();
                    pG10.Image = null;
                    G10_lab.Text = "-";
                }
                if (pG11.Image != null)
                {
                    pG11.Image.Dispose();
                    pG11.Image = null;
                    G11_lab.Text = "-";
                }
                if (pG12.Image != null)
                {
                    pG12.Image.Dispose();
                    pG12.Image = null;
                    G12_lab.Text = "-";
                }
                if (pH1.Image != null)
                {
                    pH1.Image.Dispose();
                    pH1.Image = null;
                    H1_lab.Text = "-";
                }
                if (pH2.Image != null)
                {
                    pH2.Image.Dispose();
                    pH2.Image = null;
                    H2_lab.Text = "-";
                }
                if (pH3.Image != null)
                {
                    pH3.Image.Dispose();
                    pH3.Image = null;
                    H3_lab.Text = "-";
                }
                if (pH4.Image != null)
                {
                    pH4.Image.Dispose();
                    pH4.Image = null;
                    H4_lab.Text = "-";
                }
                if (pH5.Image != null)
                {
                    pH5.Image.Dispose();
                    pH5.Image = null;
                    H5_lab.Text = "-";
                }
                if (pH6.Image != null)
                {
                    pH6.Image.Dispose();
                    pH6.Image = null;
                    H6_lab.Text = "-";
                }
                if (pH7.Image != null)
                {
                    pH7.Image.Dispose();
                    pH7.Image = null;
                    H7_lab.Text = "-";
                }
                if (pH8.Image != null)
                {
                    pH8.Image.Dispose();
                    pH8.Image = null;
                    H8_lab.Text = "-";
                }
                if (pH9.Image != null)
                {
                    pH9.Image.Dispose();
                    pH9.Image = null;
                    H9_lab.Text = "-";
                }
                if (pH10.Image != null)
                {
                    pH10.Image.Dispose();
                    pH10.Image = null;
                    H10_lab.Text = "-";
                }
                if (pH11.Image != null)
                {
                    pH11.Image.Dispose();
                    pH11.Image = null;
                    H11_lab.Text = "-";
                }
                if (pH12.Image != null)
                {
                    pH12.Image.Dispose();
                    pH12.Image = null;
                    H12_lab.Text = "-";
                }
                HOperatorSet.WaitSeconds(3);
            }

            #endregion
            Run_btn.Enabled = true;
        }
    }
}
