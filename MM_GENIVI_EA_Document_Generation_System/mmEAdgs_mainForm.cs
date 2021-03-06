﻿/* SPDX license identifier: MPL-2.0
 * Author: Guido Pennella
 * Copyright ©2014, Magneti Marelli S.p.A. All rights reserved
 * Version: MPL 2.0
 * 
 * This file is part of Magneti Marelli Genivi Document Generator
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 2.0. If a copy of the MPL was not distributed with this file, you can obtain one at http://mozilla.org/MPL/2.0/. 
 * 
 * For further information see http://www.genivi.org/. 
 *
 * ***** END LICENSE BLOCK *****/
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
using Microsoft.Win32;
using System.IO;
using System.Xml;

namespace MM_GENIVI_EA_Document_Generation_System
{
    public partial class mmEAdgs_mainForm : Form
    {
        // Registry key to store data
        private RegistryKey regBase = Registry.CurrentUser;
        private String regProduct = "SOFTWARE\\" + Application.ProductName.ToString();

        // List of extraction functions fond
        private List<extractionFunctions> extractionFunctions_List = new List<extractionFunctions>();

        // Data form instance
        public mmEAdgs_dataForm frm_instance;

        // Form init - automatically generated
        public mmEAdgs_mainForm()
        {
            InitializeComponent();
        }

        // Function used to reset the progress bar
        public void resetProgress()
        {
            genProgress.Value = 0;
        }

        // Function that allows to set the minimum and maximum values of the progress bar
        public void setMaxMinProgress(int minv, int maxv)
        {
            genProgress.Maximum = maxv;
            genProgress.Minimum = minv;
        }

        // Function that allows to update the progress bar by one step
        public void progProgess()
        {
            genProgress.Value = genProgress.Value + 1;
        }

        // Function that perform the on the fly DLL load based on the given path
        // the DLL instance reference will be loaded into a list, to be used later
        private Boolean loadDLL(String deafultPath)
        {
            
            // Length of the path
            int pathlen = deafultPath.Length;
            if (pathlen == 1)
            {
                // Path set to . -> add \\ to it
                pathlen = 2;
            }

            // Acquire the list of DLL
            string[] filePaths = Directory.GetFiles(deafultPath, "eadgsPlugin_extract*.dll");

            // clear the list if extraction functions
            extractionFunctions_List.Clear();

            foreach (String filename in filePaths)
            {
                // Extract the function name from the file name
                String local_filename = filename.Substring(pathlen);
                String functionName = local_filename.Substring(local_filename.IndexOf("_") + 1);
                int fillen = local_filename.Length;
                int funlen = functionName.Length;
                functionName = functionName.Remove(funlen - 4);
                String className = local_filename.Remove(fillen - 4);

                // Load the DLL
                Assembly assembly = Assembly.LoadFrom(filename);

                // Get the class
                Type classType = assembly.GetType(className + ".eadgsLoadable");

                // Get the obejct instance
                object instace = Activator.CreateInstance(classType);

                // Get the method
                MethodInfo method = classType.GetMethod(functionName);

                // create a new entry in the list with the acquired values
                extractionFunctions_List.Add(new extractionFunctions
                {
                    fileName = filename,
                    extractionFunctionName = functionName,
                    extractionClassName = className,
                    dllInstance = instace,
                    dllMethod = method
                }
                    );
            }

            return true;
        }

        /*! \brief  Method to be called with form load
         * 
         * This method will try to load the DLL from the pre-defined dir
         * stored in the registry, calling the appropriate methods and
         * instantiate the data form
         */
        private void mmEAdgs_mainForm_Load(object sender, EventArgs e)
        {
            // varialble to hold the path where the DLL shall be seeked
            // set to default value of the current directory of the application
            String path = ".";
            // Load data from registry - txtDllDir
            RegistryKey reg = regBase.OpenSubKey(regProduct);
            try
            {
                path = (string)reg.GetValue("txtDllDir".ToUpper());
            }
            catch (Exception ex)
            {
                // The registry key was null, so the path is re-set to the default value
                Console.WriteLine(ex.ToString());
                path = ".";
            }

            // Additional check, to verify that the path is not null or set to empty value
            if (path == null || path == "")
            {
                path = ".";
            }
            
            // Now call the function to populate the list
            loadDLL(path);

            // Populate the form
            frm_instance = new mmEAdgs_dataForm();
            frm_instance.init(extractionFunctions_List, this);
            this.splitContainer.Panel1.Controls.Add(frm_instance);

            // set the version visible
            this.Text = "Magneti Marelli - GENIVI - EA Document Generation System - 1.0c";
        }

        // Function to show the about form
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About abx = new About();
            abx.Show();
        }

        // Work in progress: function to save the current parameters for future retreival
        private void saveProjectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = "%USERPROFILE%";
            saveFile.RestoreDirectory = true;
            saveFile.Filter = "MM EA DGS project (.dgs)|*.dgs|All Files (*.*)|*.*";
            saveFile.FilterIndex = 1;
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                // Export project file
                // openFile.FileName.ToString();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                XmlWriter writer = XmlWriter.Create(saveFile.FileName.ToString(), settings);

                writer.WriteStartDocument();

                writer.WriteComment("MM EA DOCUMENT GENERATION SYSTEM SAVEFILE");

                writer.WriteStartElement("Project");
                // Data save
                // Template DOCX
                writer.WriteElementString("DOCXTEMPLATE", frm_instance.txtTemplate.Text);

                // Outfile DOCX
                writer.WriteElementString("DOCXOUTPUT", frm_instance.txtOutput.Text);

                // Images
                writer.WriteElementString("IMAGES", frm_instance.txtImages.Text);

                // EAP
                writer.WriteElementString("EAP", frm_instance.txtEAP.Text);

                // DLL
                writer.WriteElementString("DLL", frm_instance.txtDllDir.Text);

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();

            }
        }

        // Work in progress: function to load saved parameters
        private void loadProjectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = "%USERPROFILE%";
            openFile.RestoreDirectory = true;
            openFile.Filter = "MM EA DGS project (.dgs)|*.dgs|All Files (*.*)|*.*";
            openFile.FilterIndex = 1;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                XmlDocument xmlFile = new XmlDocument();
                try
                {
                    xmlFile.Load(openFile.FileName.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                XmlNodeList trans = xmlFile.GetElementsByTagName("Project");
                // load data

                foreach (XmlNode node in trans)
                {

                    frm_instance.txtTemplate.Text = node["DOCXTEMPLATE"].InnerText.ToString();
                    frm_instance.txtOutput.Text = node["DOCXOUTPUT"].InnerText.ToString();
                    frm_instance.txtImages.Text = node["IMAGES"].InnerText.ToString();
                    frm_instance.txtEAP.Text = node["EAP"].InnerText.ToString();
                    frm_instance.txtDllDir.Text = node["DLL"].InnerText.ToString();
                }
                // update the registry
                frm_instance.updateRegistry();
            }
        }

        // Work in progress: function to show the chm manual
        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // under development
            MessageBox.Show("Function under development", "Development in progress", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
