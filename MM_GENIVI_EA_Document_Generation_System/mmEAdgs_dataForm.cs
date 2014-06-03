/* SPDX license identifier: MPL-2.0
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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MM_GENIVI_EA_Document_Generation_System
{
    public partial class mmEAdgs_dataForm : UserControl
    {
        // Registry key to store data
        private RegistryKey regBase = Registry.CurrentUser;
        private String regProduct = "SOFTWARE\\" + Application.ProductName.ToString();

        // Reference to the main form
        mmEAdgs_mainForm main = null;

        // List of the DLL references, to be called during document scan
        private List<extractionFunctions> extractionFunctions_List;

        // Form init function - automatically generated
        public mmEAdgs_dataForm()
        {
            InitializeComponent();
        }

        // Function to load passed parameters into class private variables, used by the caller operation
        // to fully initialize the form
        public void init(List<extractionFunctions> extractionFunctions, mmEAdgs_mainForm frm)
        {
            extractionFunctions_List = extractionFunctions;
            main = frm;
        }

        // Function that instantiate a file chooser dialog to get the DOCX template file
        private void btnTemplate_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = "%USERPROFILE%";
            openFile.RestoreDirectory = true;
            openFile.Filter = "Docx Files (.docx)|*.docx|All Files (*.*)|*.*";
            openFile.FilterIndex = 1;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                // to-do: verify that the file is a DOCX
                txtTemplate.Text = openFile.FileName.ToString();
            }
        }

        // Function to set the output file via a file chooser
        private void btnOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = "%USERPROFILE%";
            saveFile.RestoreDirectory = true;
            saveFile.Filter = "Docx Files (.docx)|*.docx|All Files (*.*)|*.*";
            saveFile.FilterIndex = 1;
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                txtOutput.Text = saveFile.FileName.ToString();
            }
        }

        // Function to set the temporary directory via a directory chooser dialog
        private void btnImages_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog imgDir = new FolderBrowserDialog();
            if (imgDir.ShowDialog() == DialogResult.OK)
            {
                txtImages.Text = imgDir.SelectedPath.ToString();
            }
        }

        private void btnEAP_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = "%USERPROFILE%";
            openFile.RestoreDirectory = true;
            openFile.Filter = "EAP Link Files (.eap)|*.eap|All Files (*.*)|*.*";
            openFile.FilterIndex = 1;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                txtEAP.Text = openFile.FileName.ToString();
            }
        }

        /*! \brief Method to find the entry in the list that correspond a certain extraction function
        * 
        * This method will find the index in the list that correspond to the passed extraction
        * function name. If the function is not find in the list, the index returned will be -1
        * The the seek will be case insensitive
        * \param function_name name of the function to seek
        */
        public int findFunctionIndex(String function_name)
        {
            int val = -1;
            for (int i = 0; i < extractionFunctions_List.Count(); i++)
            {
                if (String.Compare(extractionFunctions_List[i].extractionFunctionName.ToUpper(), function_name.ToUpper()) == 0)
                {
                    val = i;
                    break;
                }
            }

            return val;
        }

        private Boolean generateDocumentation(String output, int id)
        {
            // Acquire the EAP file to be used
            String eapFile = txtEAP.Text;

            XNamespace w = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";
            XNamespace r = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";

            // To do: insert a try/catch here to manage the case in which the file is open
            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(output, true))
            {
                MainDocumentPart mainPart = myDoc.MainDocumentPart;

                var sdtBlocks = mainPart.Document.Body.ChildElements.OfType<SdtBlock>();
                foreach (SdtBlock block in sdtBlocks)
                {
                    if (block.SdtProperties != null)
                    {
                        var blockTag = block.SdtProperties.ChildElements.OfType<Tag>();
                        if (blockTag.Count() != 0)
                        {
                            Tag tg = blockTag.ElementAt(0);

                            // Found external_docx tag - this foreseen a generation DLL that will produce a DOCX file to be taken
                            if (tg.Val.Value == "external_docx")
                            {
                                String altChunkId = "AltChunkId" + id;
                                // Decode the requested command
                                String command = block.InnerText.ToString();
                                String[] param = command.Split('|');
                                String funame = param[0];
                                // Build the parameter to send to the DLL, based on the command and the file of the EAP DB
                                String parameter = command + "|" + eapFile + "|" + txtImages.Text;
                                String image_name = param[2];
                                String pngFile = image_name.Replace(" ", "_");
                                String file = txtImages.Text + "\\" + pngFile + ".docx";

                                // Call the function
                                int idx = findFunctionIndex(funame);

                                object result = extractionFunctions_List[idx].dllMethod.Invoke(extractionFunctions_List[idx].dllInstance, new object[] { parameter });

                                // Manage any error

                                id++;
                                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(
                                      AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                                // Substitute the IMAGE tag with the file in the innertext
                                
                                using (FileStream fileStream = System.IO.File.Open(file, FileMode.Open))
                                    chunk.FeedData(fileStream);
                                
                                AltChunk altChunk = new AltChunk();
                                altChunk.Id = altChunkId;
                                OpenXmlElement parent = block.Parent;
                                parent.InsertAfter(altChunk, block);
                                block.Remove();
                            }

                            // Found local doc tag - a DOCX file in the inner text places
                            if (tg.Val.Value == "local")
                            {
                                String altChunkId = "AltChunkId" + id;
                                String filename = block.InnerText.ToString();
                                

                                id++;
                                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(
                                      AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                                if (System.IO.File.Exists(filename))
                                {
                                    // Substitute the IMAGE tag with the file in the innertext

                                    using (FileStream fileStream = System.IO.File.Open(filename, FileMode.Open))
                                        chunk.FeedData(fileStream);

                                }
                                else
                                {
                                    // The user requested file that cannot be found
                                }
                                AltChunk altChunk = new AltChunk();
                                altChunk.Id = altChunkId;
                                OpenXmlElement parent = block.Parent;
                                parent.InsertAfter(altChunk, block);
                                block.Remove();
                            }

                            // Found local_rtf doc tag - a RTF file in the inner text places
                            if (tg.Val.Value == "local_rtf")
                            {
                                String altChunkId = "AltChunkId" + id;
                                String filename = block.InnerText.ToString();


                                id++;
                                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(
                                      AlternativeFormatImportPartType.Rtf, altChunkId);
                                if (System.IO.File.Exists(filename))
                                {
                                    // Substitute the IMAGE tag with the file in the innertext

                                    using (FileStream fileStream = System.IO.File.Open(filename, FileMode.Open))
                                        chunk.FeedData(fileStream);

                                }
                                else
                                {
                                    // The user requested file that cannot be found
                                }
                                AltChunk altChunk = new AltChunk();
                                altChunk.Id = altChunkId;
                                OpenXmlElement parent = block.Parent;
                                parent.InsertAfter(altChunk, block);
                                block.Remove();
                            }

                            // Found local_xhtml doc tag - a XHTML file in the inner text places
                            if (tg.Val.Value == "local_xhtml")
                            {
                                String altChunkId = "AltChunkId" + id;
                                String filename = block.InnerText.ToString();

                                id++;
                                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(
                                      AlternativeFormatImportPartType.Xhtml, altChunkId);
                                if (System.IO.File.Exists(filename))
                                {
                                    // Substitute the IMAGE tag with the file in the innertext

                                    using (FileStream fileStream = System.IO.File.Open(filename, FileMode.Open))
                                        chunk.FeedData(fileStream);

                                }
                                else
                                {
                                    // The user requested file that cannot be found
                                }
                                AltChunk altChunk = new AltChunk();
                                altChunk.Id = altChunkId;
                                OpenXmlElement parent = block.Parent;
                                parent.InsertAfter(altChunk, block);
                                block.Remove();
                            }


                            // Found external tag - request external data
                            if (tg.Val.Value == "external_xhtml")
                            {
                                
                                String altChunkId = "AltChunkId" + id;

                                // Decode the requested command
                                String command = block.InnerText.ToString();
                                String[] param = command.Split('|');
                                String funame = param[0];
                                // Build the parameter to send to the DLL, based on the command and the file of the EAP DB
                                String parameter = command + "|" + eapFile + "|" + txtImages.Text;

                                // Call the function
                                int idx = findFunctionIndex(funame);

                                object result = extractionFunctions_List[idx].dllMethod.Invoke(extractionFunctions_List[idx].dllInstance, new object[] { parameter });

                                // Manage any error

                                id++;
                                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(
                                      AlternativeFormatImportPartType.Xhtml, altChunkId);
                                String filename = txtImages.Text + "\\" + funame + ".xhtml";

                                using (FileStream fileStream = System.IO.File.Open(filename, FileMode.Open))
                                    chunk.FeedData(fileStream);

                                AltChunk altChunk = new AltChunk();
                                altChunk.Id = altChunkId;
                                OpenXmlElement parent = block.Parent;
                                parent.InsertAfter(altChunk, block);
                                block.Remove();
                            }
                        }
                    }
                }
                myDoc.Close();
            }

            return true;
        }

        public void updateRegistry()
        {
            this.txtDllDir_TextChanged(null, null);
            this.txtEAP_TextChanged(null, null);
            this.txtImages_TextChanged(null, null);
            this.txtOutput_TextChanged(null, null);
            this.txtTemplate_TextChanged(null, null);
        }

        private void btnGen_Click(object sender, EventArgs e)
        {
            // To do - check params

            String output = txtOutput.Text;

            if (null == output || "" == output)
            {
                // Error: no output file is present
                MessageBox.Show("Error: no output file defined", "Generation step 1 - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Step 2: Control if the template file i 
            if (null == txtTemplate.Text || "" == txtTemplate.Text)
            {
                // Error: no output file is present
                MessageBox.Show("Error: no template file defined", "Generation step 2 - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Step 3: Control capability to copy the template to the outfile
            try
            {
                System.IO.File.Copy(txtTemplate.Text, txtOutput.Text, true);
            }
            catch (Exception ex)
            {
                // Error in coping file
                MessageBox.Show(ex.Message.ToString(), "Generation step 3 - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Iter variable will contains the number of elements to be substituted
            int iter = 0;
            using (WordprocessingDocument myDoc = WordprocessingDocument.Open(output, true))
            {
                MainDocumentPart mainPart = myDoc.MainDocumentPart;
                

                var sdtBlocks = mainPart.Document.Body.ChildElements.OfType<SdtBlock>();
                iter = sdtBlocks.Count();
                /*
                 * TO DO: manage also header and footer - work in progress
                foreach (HeaderPart header in mainPart.GetPartsOfType<HeaderPart>())
                {
                    var sdtBlocks2 = header.RootElement.Descendants<SdtBlock>();
                    //Console.Write(sdtBlocks2.Count());
                    iter += sdtBlocks2.Count();
                }
                 * */

            }

            // Reset the progress bar
            main.setMaxMinProgress(0, iter);
            main.resetProgress();

            for (int i = 0; i < iter; i++)
            {
                main.progProgess();
                int id = i + 1;
                this.generateDocumentation(output, id);
            }

            // End of generation message
            MessageBox.Show("Document generation done", "Generation OK", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog imgDir = new FolderBrowserDialog();
            if (imgDir.ShowDialog() == DialogResult.OK)
            {
                txtDllDir.Text = imgDir.SelectedPath.ToString();
            }
        }

        private void mmEAdgs_dataForm_Load(object sender, EventArgs e)
        {
            // Load data from registry - txtTemplate
            // if data not present, then the widget remains empty
            RegistryKey reg = regBase.OpenSubKey(regProduct);
            try
            {
                txtTemplate.Text = (string)reg.GetValue("txtTemplate".ToUpper());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                txtTemplate.Text = "";
            }
            // Load data from registry - txtOutput
            try
            {
                txtOutput.Text = (string)reg.GetValue("txtOutput".ToUpper());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                txtOutput.Text = "";
            }
            // Load data from registry - txtImages
            try
            {
                txtImages.Text = (string)reg.GetValue("txtImages".ToUpper());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                txtImages.Text = "";
            }
            // Load data from registry - txtEAP
            try
            {
                txtEAP.Text = (string)reg.GetValue("txtEAP".ToUpper());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                txtEAP.Text = "";
            }
            // Load data from registry - txtDllDir
            try
            {
                txtDllDir.Text = (string)reg.GetValue("txtDllDir".ToUpper());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                txtDllDir.Text = "";
            }
            //--------------------------------------------------
            
            //String parameter = "FUNZIONE2|D:\\users\\F57165A\\Downloads\\NavigationCore-api-genivi-navigationcore-locationinput.xml|1|0|D:\\EA10Prjs\\Alibi_process.eap|D:\\Temp";
            //String parameter = "FUNZIONE2|D:\\users\\F57165A\\Downloads\\node-startup-controller-node-startup-controller-dbus.xml|1|0|D:\\EA10Prjs\\Alibi_process.eap|D:\\Temp";
            //String parameter = "FUNZIONE3|D:\\users\\F57165A\\Downloads\\NodeStateAccess-model-org.genivi.NodeStateManager.LifecycleConsumer.xml|1|0|D:\\EA10Prjs\\Alibi_process.eap|D:\\Temp";
            
            //int idx = findFunctionIndex("extractDBusXML");

            //object result = extractionFunctions_List[idx].dllMethod.Invoke(extractionFunctions_List[idx].dllInstance, new object[] { parameter });

        }

        private void txtTemplate_TextChanged(object sender, EventArgs e)
        {
            RegistryKey reg = regBase.CreateSubKey(regProduct);
            reg.SetValue("txtTemplate".ToUpper(), txtTemplate.Text);
        }

        private void txtOutput_TextChanged(object sender, EventArgs e)
        {
            RegistryKey reg = regBase.CreateSubKey(regProduct);
            reg.SetValue("txtOutput".ToUpper(), txtOutput.Text);
        }

        private void txtImages_TextChanged(object sender, EventArgs e)
        {
            RegistryKey reg = regBase.CreateSubKey(regProduct);
            reg.SetValue("txtImages".ToUpper(), txtImages.Text);
        }

        private void txtEAP_TextChanged(object sender, EventArgs e)
        {
            RegistryKey reg = regBase.CreateSubKey(regProduct);
            reg.SetValue("txtEAP".ToUpper(), txtEAP.Text);
        }

        private void txtDllDir_TextChanged(object sender, EventArgs e)
        {
            RegistryKey reg = regBase.CreateSubKey(regProduct);
            reg.SetValue("txtDllDir".ToUpper(), txtDllDir.Text);
        }
    }
}
