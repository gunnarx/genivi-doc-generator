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
//using Word = Microsoft.Office.Interop.Word;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

namespace eadgsPlugin_extractImageByName
{
    public class eadgsLoadable
    {
        private String connectionString = "";
        private OdbcConnection eapConnection = null;

        public static void InsertAPicture(String document, String fileName, String CaptionType, String Caption)
        {
            using (WordprocessingDocument wordprocessingDocument =
                WordprocessingDocument.Open(document,true))
            {
                MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;

                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Png);

                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    imagePart.FeedData(stream);
                }

                AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart), CaptionType, Caption);
                
                wordprocessingDocument.Close();
            }
        }

        private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId, String CaptionType, String Caption)
        {
            int cx, cy;
            cx = 22;
            cy = cx/5*4;
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = cx * 261257L, Cy = cy * 261257L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.png"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                       "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = cx * 261257L, Cy = cy * 261257L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         ) { Preset = A.ShapeTypeValues.Rectangle }))
                             ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

           
            SimpleField cap = new SimpleField();
            Paragraph par = new Paragraph();
            
            par.ParagraphProperties = new ParagraphProperties(new ParagraphStyleId() { Val = "Caption" });
            par.AppendChild(new Run(new Text(CaptionType + " ")));
            par.AppendChild(cap);
            par.AppendChild(new Run(new Text(": "+ Caption)));

            cap.Instruction = @" SEQ Figure \* ARABIC ";
            
            
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));

            wordDoc.MainDocumentPart.Document.Body.AppendChild(par);
           
            
        }

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
            eapConnection.Close();

            String docxFile = auxdir + "\\" + pngFile + ".docx";
            String imageFile = auxdir + "\\" + pngFile + ".png";

            InsertAPicture(docxFile, imageFile, "Figure", caption);

            return true;
        }
    }
}
