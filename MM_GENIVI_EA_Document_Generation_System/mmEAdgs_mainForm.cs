/* ***** BEGIN LICENSE BLOCK *****
 * Author: Guido Pennella
 * Copyright ©2013, Magneti Marelli S.p.A. All rights reserved
 * Version: MPL 2.0
 *
 * The contents of this file are subject to the Mozilla Public License Version
 * 2.0 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 * http://www.mozilla.org/MPL/
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
 * for the specific language governing rights and limitations under the Licence.
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

namespace MM_GENIVI_EA_Document_Generation_System
{
    public partial class mmEAdgs_mainForm : Form
    {
        // Registry key to store data
        private RegistryKey regBase = Registry.LocalMachine;
        private String regProduct = "SOFTWARE\\" + Application.ProductName.ToString();

        // List of extraction functions fond
        private List<extractionFunctions> extractionFunctions_List = new List<extractionFunctions>();

        // Data form instance
        public mmEAdgs_dataForm frm_instance;

        public mmEAdgs_mainForm()
        {
            InitializeComponent();
        }

        public void resetProgress()
        {
            genProgress.Value = 0;
        }

        public void setMaxMinProgress(int minv, int maxv)
        {
            genProgress.Maximum = maxv;
            genProgress.Minimum = minv;
        }

        public void progProgess()
        {
            genProgress.Value = genProgress.Value + 1;
        }

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
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About abx = new About();
            abx.Show();
        }

        private void saveProjectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // under development
            MessageBox.Show("Function under development", "Development in progress", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void loadProjectFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // under development
            MessageBox.Show("Function under development", "Development in progress", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // under development
            MessageBox.Show("Function under development", "Development in progress", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
