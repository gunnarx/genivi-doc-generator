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
using System.Xml;
using System.Data.Odbc;

namespace eadgsPlugin_extractUseCase
{
    public class eadgsLoadable
    {
        private String connectionString = "";
        private OdbcConnection eapConnection = null;

        private Dictionary<string, eadgs_usecase> usecase_list = new Dictionary<string, eadgs_usecase>();

        public Boolean extractUseCase(String prm)
        {
            // extract parameters
            String[] param = prm.Split('|');
            // Decode parameters
            String funame = param[0];
            String[] pkg_tree = param[1].Split('.');
            String title_level = param[2];
            Int32 title1 = Int32.Parse(title_level);
            Int32 title2 = title1+1;
            String ucname = param[3];
            String insreq = param[4];
            String eapFile = param[5];
            String auxdir = param[6];
            String sql;
            connectionString = @"Driver={Microsoft Access Driver (*.mdb)};Dbq=" + eapFile + ";Uid=Admin;Pwd=";

            eapConnection = new OdbcConnection();
            eapConnection.ConnectionString = connectionString;
            eapConnection.Open();

            int row = 1;
            String formatting;

            // Recursive acquisition of the final package ID
            String pkgID = eadgsPlugin_Utilies.eadgsPlugin_Utilies.acquirePackageID(0, pkg_tree, "1", eapConnection);
            if (ucname.CompareTo("ALL") == 0)
            {
                sql = "select Object_ID, Note, Name from t_object where Object_Type = 'UseCase' and Package_ID = " + pkgID;
            }
            else
            {
                sql = "select Object_ID, Note, Name from t_object where Object_Type = 'UseCase' and Package_ID = " + pkgID + " and Name ='" + ucname + "'";
            }

            OdbcCommand eapCommand = eapConnection.CreateCommand();
            eapCommand.CommandText = sql;
            OdbcDataReader eapReader; 
            eapReader = eapCommand.ExecuteReader();

            while (eapReader.Read())
            {
                eadgs_usecase usecase = new eadgs_usecase();
                usecase.objectID = eapReader[0].ToString();
                usecase.uc_text = eapReader[1].ToString();
                usecase.uc_name = eapReader[2].ToString();
                usecase_list.Add(eapReader[0].ToString(), usecase);
            }
            String output_to_html = eadgsPlugin_Utilies.eadgsPlugin_Utilies.html_upper;
            eapReader.Close();

            foreach (KeyValuePair<string, eadgs_usecase> uc in usecase_list)
            {
                // Use case intro
                output_to_html += "<h" + title1 + ">Use Case: " + uc.Value.uc_name + "</h" + title1 + ">\n";
                output_to_html += uc.Value.uc_text.Replace("\n"," ") + " <br>\n";

                // Acquire the pre/post conditions
                sql = "select Constraint, Notes from t_objectconstraint where Object_ID = " + uc.Value.objectID + " and ConstraintType = 'Pre-condition'";

                eapCommand.CommandText = sql;
                eapReader = eapCommand.ExecuteReader();

                output_to_html += "Pre-condition(s):<ul>";

                while (eapReader.Read())
                {
                    output_to_html += "<li><b>" + eapReader[0].ToString().Replace("\n", " ") + ":</b>" + eapReader[1].ToString() + "</li>";
                }

                output_to_html += "</ul>";
                eapReader.Close();
                sql = "select Constraint, Notes from t_objectconstraint where Object_ID = " + uc.Value.objectID + " and ConstraintType = 'Post-condition'";

                eapCommand.CommandText = sql;
                eapReader = eapCommand.ExecuteReader();

                output_to_html += "Post-condition(s):<ul>";

                while (eapReader.Read())
                {
                    output_to_html += "<li><b>" + eapReader[0].ToString().Replace("\n"," ") + ":</b>" + eapReader[1].ToString() + "</li>";
                }

                output_to_html += "</ul>";

                eapReader.Close();

                // If requested, insert the requirements
                if (insreq.CompareTo("1") == 0)
                {
                    sql = "select alias from t_object where Object_ID in (select end_object_id from t_connector where stereotype = 'trace' and start_object_id = " + uc.Value.objectID + ")";
                
                    eapCommand.CommandText = sql;
                    eapReader = eapCommand.ExecuteReader();
                    output_to_html += "Requirement(s) connected with the Use Case:<br>\n<ul>";
                    while (eapReader.Read())
                    {
                        output_to_html += "<li>" + eapReader[0].ToString() +  "</li>";
                    }
                    output_to_html += "</ul>";
                    eapReader.Close();
                }

                // Acquire the scenarios
                sql = "select XMLContent, Scenario, ScenarioType, Notes from t_objectscenarios where Object_ID = " + uc.Value.objectID + " order by EValue";
                
                eapCommand.CommandText = sql;
                eapReader = eapCommand.ExecuteReader();
                while (eapReader.Read())
                {
                    output_to_html += "<h" + title2 + ">" + eapReader[2].ToString() + ": " + eapReader[1].ToString() + "</h" + title2 + ">\n";
                    output_to_html +=  eapReader[3].ToString().Replace("\n", " ") + " <br>\n";

                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(eapReader[0].ToString());

                    XmlNodeList trans = xmldoc.GetElementsByTagName("step");

                    output_to_html += "\n<table border=\"1\" cellpadding=\"1\" cellspacing=\"2\" width=\"100%\">";
                    output_to_html += "<tr style=\"background-color:rgb(221,221,221)\"><td width=\"20%\"><b>Step</b></td><td width=\"80%\"><b>Description</b></td></tr>";

                    row = 1;
                    foreach (XmlNode node in trans)
                    {
                        if ((row % 2) != 0)
                        {
                            formatting = "";
                        }
                        else
                        {
                            formatting = "style=\"background-color:lightblue\"";
                        }

                        output_to_html += "<tr " + formatting + "><td width=\"20%\">" + node.Attributes["level"].Value.ToString() + "</td><td width=\"80%\">" + node.Attributes["name"].Value.ToString() + "</td></tr>";
                        row++;
                    }
                    output_to_html += "\n</table>";
                }

                eapReader.Close();
            }
            output_to_html += eadgsPlugin_Utilies.eadgsPlugin_Utilies.html_lower;

            String filename = auxdir + "\\" + funame + ".xhtml";

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.Write(output_to_html);
            file.Close();

            return true;
        }
    }
}
