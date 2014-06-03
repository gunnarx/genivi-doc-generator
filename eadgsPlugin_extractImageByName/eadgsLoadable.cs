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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using EA;
using Word = Microsoft.Office.Interop.Word;

namespace eadgsPlugin_extractImageByName
{
    public class eadgsLoadable
    {
        private String connectionString = "";
        private OdbcConnection eapConnection = null;

        public Boolean extractImageByName(String prm)
        {
            Console.WriteLine(prm);
            // extract parameters
            String[] param = prm.Split('|');
            // Decode parameters
            String[] pkg_tree = param[1].Split('.');
            String image_name = param[2];
            String eapFile = param[3];
            String auxdir = param[4];

            String pngFile = image_name.Replace(" ", "_");
            String caption = "";

            connectionString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq="+eapFile+";Uid=Admin;Pwd=";
            //connectionString = @"Driver={Microsoft Access Driver (*.mdb)};" + "Dbq=D:\\EA10Prjs\\Alibi_process.eap;Uid=Admin;Pwd=";

            eapConnection = new OdbcConnection();
            eapConnection.ConnectionString = connectionString;
            eapConnection.Open();

            // Recursive acquisition of the final package ID

            String pkgID = eadgsPlugin_Utilies.eadgsPlugin_Utilies.acquirePackageID(0, pkg_tree, "0", eapConnection);

            // Acquire the pacakge ea_guid and its notes
            String sql = "select ea_guid, Notes from t_diagram where Package_ID = " + pkgID + " and Name ='" + image_name + "'";

            OdbcCommand eapCommand = eapConnection.CreateCommand();
            eapCommand.CommandText = sql;
            OdbcDataReader eapReader = eapCommand.ExecuteReader();
            eapReader.Read();
            String ea_guid = eapReader.GetString(0);
            // Check to verify that the caption is not null
            try
            {
                caption = eapReader.GetString(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (caption == "")
            {
                caption = "Diagram " + image_name;
            }
            caption = caption.Replace("_", " ");
            
            eapReader.Close();
            eapCommand.Dispose();

            // Export the diagram image to the temp dir via EA API
            Repository eapRep = new Repository();
            eapRep.OpenFile(eapFile);
            Project eapPrj = new Project();
            eapPrj = eapRep.GetProjectInterface();
            eapPrj.PutDiagramImageToFile(ea_guid, auxdir + "\\" + pngFile + ".png", 1);

            // Build the DOCX file
            // Prepare WORD interoperability to generate the WORD files for the images
            object missing = Type.Missing;
            object notTrue = false;

            Word.Application oWord = null;
            Word.Documents oDocs = null;
            Word.Document oDoc = null;
            Word.Range range = null;

            object fileName = null;
            object fileFormat = Word.WdSaveFormat.wdFormatXMLDocument;

            String file = auxdir + "\\" + pngFile + ".docx";

            // Generate a new word file
            oWord = new Word.Application();
            oWord.Visible = false;


            // Create a new Document and add it to document collection.                
            oDoc = oWord.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            // Add the image
            oDoc.InlineShapes.AddPicture(auxdir + "\\" + pngFile + ".png");
            range = oDoc.Range();

            range.InsertCaption("Figure", ": " + caption);

            // Save the word document created

            fileName = auxdir + "\\" + pngFile + ".docx";

            oDoc.SaveAs(ref fileName, ref fileFormat, ref missing,
              ref missing, ref missing, ref missing, ref missing,
              ref missing, ref missing, ref missing, ref missing,
              ref missing, ref missing, ref missing, ref missing,
              ref missing);
            ((Word._Document)oDoc).Close(ref missing, ref missing,
                ref missing);

            ((Word._Application)oWord).Quit(ref notTrue, ref missing,
               ref missing);

            // Clear environment
            oWord = null;
            oDocs = null;
            oDoc = null;

            eapConnection.Close();
            return true;
        }
    }
}
