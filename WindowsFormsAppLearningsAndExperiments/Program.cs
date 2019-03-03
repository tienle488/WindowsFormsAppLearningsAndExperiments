using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppLearningsAndExperiments
{
    public class OpenFileDialogForm : Form
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        public static void Main()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Application.Run(new OpenFileDialogForm()); //user needs to click the Select File button - need to automate clicking this.  How?? (20190302T1237: Why is it looping??  Test doesn't end until i click Stop test.  Hum...
            //Also, I keep getting "Changes not allowed to code while application is running"? And I'm not changing anything that I can think of.

            //Application.Run(new SendKeys("Test_didata_de.valid_Booking1Line.csv")); Error CS1729  'SendKeys' does not contain a constructor that takes 1 arguments 
            //SendKeys("Test_didata_de.valid_Booking1Line.csv"); Error CS1955  Non - invocable member 'SendKeys' cannot be used like a method.	
        }

        #region click Select File to open File dialog then user selects file then program reads file and output it to Windows Form
        private Button selectButton;
        private OpenFileDialog openFileDialog1;
        private TextBox textBox1;

        public OpenFileDialogForm()
        {
            openFileDialog1 = new OpenFileDialog();
            selectButton = new Button
            {
                Size = new Size(100, 23),
                Location = new Point(15, 15),
                Text = "Select file"
            };
            //original solution:
            selectButton.Click += new EventHandler(SelectButton_Click);
            //selectButton.Click += new EventHandler(button1_Click); //doing the button1_Click instead of the original SelectButton_Click
            textBox1 = new TextBox
            {
                Size = new Size(800, 600),
                Location = new Point(15, 40),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            ClientSize = new Size(830, 860);
            Controls.Add(selectButton);
            Controls.Add(textBox1);
        }
        
        private void SetText(string text)
        {
            textBox1.Text = text;
        }
        #endregion

        #region private async void button1_Click(object sender, EventArgs e)
        //{
        //    string initialDir = "directory\\";
        //    string FileName = "filename.smthng";
        //    string combinedDir = initialDir + FileName;
        //    if (File.Exists(combinedDir)) // if there is a file with that name at that directory
        //    {
        //        openFileDialog1.InitialDirectory = initialDir; // setting directory name
        //        openFileDialog1.FileName = FileName; // filename
        //        BeginInvoke((Action)(() => openFileDialog1.ShowDialog())); // we need to use BeginInvoke to continue to the following code.
        //        await SendKey(FileName); // Sends Key to Dialog 
        //    }
        //    else // if there is not file with that name works here because no keys need to send.
        //    {
        //        openFileDialog1.InitialDirectory = initialDir;
        //        openFileDialog1.FileName = FileName;
        //        openFileDialog1.ShowDialog();
        //    }

        //}
        #endregion

        private async Task SendKey(string FileName)
        {
            await Task.Delay(250); // Wait for the Dialog shown at the screen
            SendKeys.SendWait("+{TAB}"); // First Shift + Tab moves to Header of DataGridView of OpenFileDialog
            SendKeys.SendWait("+{TAB}"); // Second Shift + Tab moves to first item of list
            SendKeys.SendWait(FileName); // after sending filename will directly moves it to the file that we are looking for
        }

        Thread t;
        private const string initialDir = "C:\\testData\\DTL\\";
        private const string FileName = "Test_didata_de.valid_Booking1Line.csv"; //"test.txt";
        private readonly object childWindow;

        private void SelectButton_Click(object sender, EventArgs e)
        {
            //need to add code to open the file with file path: "c:\testData\DTL\Test_didata_de.valid_Booking1Line.csv" or any other file and file path per initialDir = "C:\\testData\\DTL\\" and FileName = "Test_didata_de.valid_Booking1Line.csv"
            string combinedDir = initialDir + FileName;
            if (File.Exists(combinedDir)) // if there is a file with that name at that directory
            {
                openFileDialog1.InitialDirectory = initialDir; // setting directory name
                openFileDialog1.FileName = FileName; // filename
                BeginInvoke((Action)(() => openFileDialog1.ShowDialog())); // we need to use BeginInvoke to continue to the following code.  NEED TO BE ABLE MAKE AUTO-CLICK OPEN BUTTON HERE
                //openFileDialog1.//childWindow.Get(SearchCriteria.ByAutomationId("1148")).Select(1);
                //var OpenButton = childWindow.Get(SearchCriteria.ByClassName("Button").AndAutomationId("1"));
                //OpenButton.DoubleClick();
                t = new Thread(new ThreadStart(SendKey)); // Sends Key to Dialog with an seperate Thread.
                t.Start(); // Thread starts.

                if (openFileDialog1.ShowDialog() == DialogResult.OK) //If what?? User has selected file and then clicked OK, read file and output to text box!!
                {
                    try
                    {
                        var sr = new StreamReader(openFileDialog1.FileName);
                        SetText(sr.ReadToEnd());
                    }
                    catch (SecurityException ex)
                    {
                        MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                                        $"Details:\n\n{ex.StackTrace}");
                    }
                }

            }
            else // if there is not file with that name works here because no keys need to send.
            {
                openFileDialog1.InitialDirectory = initialDir;
                openFileDialog1.FileName = FileName;
                openFileDialog1.ShowDialog();
            }
        }

        #region private void button1_Click(object sender, EventArgs e)
        //{
        //    string combinedDir = initialDir + FileName;
        //    if (File.Exists(combinedDir)) // if there is a file with that name at that directory
        //    {
        //        openFileDialog1.InitialDirectory = initialDir; // setting directory name
        //        openFileDialog1.FileName = FileName; // filename
        //        BeginInvoke((Action)(() => openFileDialog1.ShowDialog())); // we need to use BeginInvoke to continue to the following code.
        //        t = new Thread(new ThreadStart(SendKey)); // Sends Key to Dialog with an seperate Thread.
        //        t.Start(); // Thread starts.

        //        if (openFileDialog1.ShowDialog() == DialogResult.OK) //If what?? User has selected file and then clicked OK, read file and output to text box!!
        //        {
        //            try
        //            {
        //                var sr = new StreamReader(openFileDialog1.FileName);
        //                SetText(sr.ReadToEnd());
        //            }
        //            catch (SecurityException ex)
        //            {
        //                MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
        //                                $"Details:\n\n{ex.StackTrace}");
        //            }
        //        }

        //    }
        //    else // if there is not file with that name works here because no keys need to send.
        //    {
        //        openFileDialog1.InitialDirectory = initialDir;
        //        openFileDialog1.FileName = FileName;
        //        openFileDialog1.ShowDialog();
        //    }
        //}
        #endregion

        private void SendKey()
        {
            Thread.Sleep(100); // Wait for the Dialog shown at the screen
            SendKeys.SendWait("+{TAB}"); // First Shift + Tab moves to Header of DataGridView of OpenFileDialog
            SendKeys.SendWait("+{TAB}"); // Second Shift + Tab moves to first item of list
            SendKeys.SendWait(FileName); // after sending filename will directly moves it to the file that we are looking for
            //SendKeys.Send("{Open}");
        }
    }
}