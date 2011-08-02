﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Adastra
{
    /// <summary>
    /// Recorded data is saved so that the very same data can be re-used to repeat the training
    /// </summary>
    public partial class ManageRecordedData : Form
    {
        EEGRecord saveRecord;
        private BackgroundWorker AsyncWorkerLoadEEGRecords;
        private BackgroundWorker AsyncWorkerSaveEEGRecord;

        List<EEGRecord> records;

        public ManageRecordedData(EEGRecord record)
        {
            InitializeComponent();

            AsyncWorkerLoadEEGRecords = new BackgroundWorker();
            AsyncWorkerLoadEEGRecords.DoWork += new DoWorkEventHandler(AsyncWorkerLoadEEGRecords_DoWork);
            AsyncWorkerLoadEEGRecords.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerLoadEEGRecords_RunWorkerCompleted);

            AsyncWorkerSaveEEGRecord = new BackgroundWorker();
            AsyncWorkerSaveEEGRecord.DoWork += new DoWorkEventHandler(AsyncWorkerSaveEEGRecord_DoWork);
            AsyncWorkerSaveEEGRecord.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AsyncWorkerSaveEEGRecord_RunWorkerCompleted);

            saveRecord = record;
            
            listBox1.DisplayMember = "Name";
            toolStripStatusLabel1.Text = "Loading EGG records ... please wait";

            buttonLoad.Enabled = false;
          
            AsyncWorkerLoadEEGRecords.RunWorkerAsync();
        }

        void AsyncWorkerSaveEEGRecord_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonSave.Enabled = true;
            if (e.Error != null)
            {
                MessageBox.Show("Error:" + e.Error.Message);
                toolStripStatusLabel1.Text = "Saving '" + saveRecord.Name + "' has failed!";
                return;
            }

            records.Add(saveRecord);
            listBox1.DataSource = null;
            listBox1.DataSource = records;
            listBox1.DisplayMember = "Name";

            toolStripStatusLabel1.Text = "EEG record '" + saveRecord.Name +"' saved.";
        }

        void AsyncWorkerSaveEEGRecord_DoWork(object sender, DoWorkEventArgs e)
        {
            if (saveRecord.vrpnIncomingSignal.Count == 0)
            {
                MessageBox.Show("Save operation aborted. Record seems empty!");
                return;
            }

            saveRecord.Name = textBoxName.Text;
            EEGRecordStorage.SaveRecord(saveRecord);
        }

        void AsyncWorkerLoadEEGRecords_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            { 
                MessageBox.Show("Error:" + e.Error.Message);
                toolStripStatusLabel1.Text = "Loading EEG records has failed.";
                return;
            }

            if (records.Count > 0)
            {
                buttonLoad.Enabled = true;
                listBox1.DataSource = records;
            }
            toolStripStatusLabel1.Text = listBox1.Items.Count + " EEG records loaded.";
        }

        void AsyncWorkerLoadEEGRecords_DoWork(object sender, DoWorkEventArgs e)
        {
            records = EEGRecordStorage.LoadModels();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text != "")
            {
                buttonSave.Enabled = false;
                AsyncWorkerSaveEEGRecord.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("Please enter record name!");
            }
        }

        public delegate void ChangedEventHandler(EEGRecord record);

        public virtual event ChangedEventHandler ReocordSelected;

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem!=null)
            {
                ReocordSelected((EEGRecord)listBox1.SelectedItem);
                System.Threading.Thread.Sleep(500);
                this.Close();
            }
            else
            {
                MessageBox.Show("No record selected!");
            }
        }
    }
}