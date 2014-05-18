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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace eadgsPlugin_extractDBusXML
{
    public class eadgsLoadable
    {
        private String output_to_html = "";

        private String current_comment = "";

        private String h1 = "";
        private String h2 = "";
        private String h3 = "";

        private int line = 1;

        private Boolean tabOpen = false;

        Dictionary<String, String> argComments = new Dictionary<string, string>();
        
        public Boolean extractDBusXML(String prm)
        {
            // extract parameters
            String[] param = prm.Split('|');
            // Decode parameters
            String funame = param[0];
            String xmlfilename = param[1];
            String baseheader = param[2];
            h1 = "h" + baseheader;
            h2 = "h" + ( Int16.Parse(baseheader) +1);
            h3 = "h" + (Int16.Parse(baseheader) + 2);
            String TBD = param[3];
            String eapFile = param[4];
            String auxdir = param[5];

            output_to_html = eadgsPlugin_Utilies.eadgsPlugin_Utilies.html_upper;

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlfilename);

            XmlNode itf = doc.DocumentElement.SelectSingleNode("/node");

            foreach (XmlNode node in itf.ChildNodes)
            {
                scanXML(node);
            }
            
            output_to_html += eadgsPlugin_Utilies.eadgsPlugin_Utilies.html_lower;

            String filename = auxdir + "\\" + funame + ".xhtml";

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.Write(output_to_html);
            file.Close();

            return true;
        }

        private void checkLine()
        {
            if (tabOpen)
            {
                output_to_html += "</td></tr>";
                tabOpen = false;
            }

            if (line == 1)
            {
            }
            else
            {
                // Line != 1 -> A table is open, close it
                output_to_html += "</table>";
                line = 1;
            }
            
        }

        private void scanXML(XmlNode curnode)
        {
            if (curnode.NodeType == XmlNodeType.Element)
            {
                switch (curnode.Name)
                {
                    case "interface":
                        checkLine();
                        output_to_html += "<" + h1 + "> Interface: " + curnode.Attributes["name"].InnerText + "</" + h1 + ">";
                        break;
                    case "method":
                        checkLine();
                        output_to_html += "<" + h2 + "> Method:" + curnode.Attributes["name"].InnerText + "</" + h2 + ">";
                        break;
                    case "arg":
                        String formatting = "";

                        if (tabOpen)
                        {
                            // table open, close it
                            output_to_html += "</td></tr>";
                            tabOpen = false;
                        }

                        if (line == 1)
                        {
                            // place the header
                            output_to_html += " Arguments:<br/>";
                            output_to_html += "\n<table border=\"1\" cellpadding=\"1\" cellspacing=\"2\" width=\"100%\">";
                            output_to_html += "<tr style=\"background-color:rgb(221,221,221)\"><td width=\"15%\"><b>Name</b></td><td width=\"30%\"><b>Type</b></td><td width=\"15%\"><b>Direction</b></td>";
                            output_to_html += "<td width=\"40%\"><b>Documentation</b></td></tr>";
                        }

                        // manage alternate line colors
                        if ((line % 2) != 0)
                        {
                            formatting = "";
                        }
                        else
                        {
                            formatting = "style=\"background-color:lightblue\"";
                        }
                        // open the table row
                        output_to_html += "<tr " + formatting + "><td width=\"15%\">" + curnode.Attributes["name"].InnerText + "</td><td width=\"30%\">" + curnode.Attributes["type"].InnerText + "</td><td width=\"15%\">" + curnode.Attributes["direction"].InnerText + "</td>";
                        output_to_html += "<td width=\"40%\">";
                        
                        // if the argComments dict has the comment, place it and close the row
                        if (argComments.ContainsKey(curnode.Attributes["name"].InnerText))
                        {
                            // Trovata chiave, inseriamo il commento
                            output_to_html += argComments[curnode.Attributes["name"].InnerText];
                            output_to_html += "</td></tr>";
                            tabOpen = false;
                        }
                        else
                        {
                            // file con altra struttura
                            tabOpen = true;
                        }

                        // update the line counter
                        line++;
                        break;
                    case "annotation":
                        checkLine();
                        break;
                    case "doc":
                        // nothing to do
                        break;
                    case "line":
                        // Documentation line
                        output_to_html += curnode.InnerText + "<br/>";
                        tabOpen = true;
                        break;
                    case "signal":
                        checkLine();
                        output_to_html += "<" + h2 + "> Signal:" + curnode.Attributes["name"].InnerText + "</" + h2 + ">";
                        break;
                    case "version":
                        checkLine();
                        output_to_html += "Version: " + curnode.InnerText + "<br/>";
                        break;
                }

                if (current_comment.CompareTo("") != 0)
                {
                    output_to_html += current_comment + "<br/>";
                    current_comment = "";
                }
                Console.WriteLine(curnode.Name);
            }
            else if (curnode.NodeType == XmlNodeType.Comment)
            {
                String[] lines = curnode.InnerText.ToString().Split('\n');
                current_comment = "";
                for (int i = 0; i < lines.Count(); i++)
                {
                    String line = lines[i];
                    if ( line.CompareTo("") != 0 )
                    {
                        if (line.IndexOf('@') != -1)
                        {
                            // found a specific section
                            int idxMax = line.IndexOf(':');
                            int idxMin = line.IndexOf('@');
                            String argName = line.Substring(idxMin + 1, idxMax - 1 - idxMin);
                            String argDoc = line.Substring(idxMax+1);
                            argComments.Add(argName, argDoc);
                        }
                        else
                        {
                            current_comment += line + "<br>";
                        }
                    }
                }
                
            }

            foreach (XmlNode node in curnode.ChildNodes)
            {
                scanXML(node);
            }
        }

    }
}
