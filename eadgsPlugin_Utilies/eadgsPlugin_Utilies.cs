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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;

namespace eadgsPlugin_Utilies
{
    public static class eadgsPlugin_Utilies
    {
        public static String html_upper = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\"\n\"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<title>Title of document</title>\n</head>\n<body>\n";
        public static String html_lower = "\n</body>\n</html>";

        // Recursive function to acquire the list of packages from a root
        public static String acquirePackageListID(String pkgID_list, String curPkgID, OdbcConnection eapConnection)
        {
            // Seek the PkgID for the current pointed package
            String sql = "select Package_ID from t_package where Parent_ID = " + curPkgID;

            OdbcCommand eapCommand = eapConnection.CreateCommand();
            eapCommand.CommandText = sql;
            OdbcDataReader eapReader = eapCommand.ExecuteReader();

            while (eapReader.Read())
            {
                curPkgID = eapReader[0].ToString();
                pkgID_list += "," + curPkgID;
                pkgID_list = acquirePackageListID(pkgID_list, curPkgID, eapConnection);
            }


            eapReader.Close();
            eapCommand.Dispose();

            return pkgID_list;
        }

        // Recursive function to find the Package ID to be used
        public static String acquirePackageID(int index, String[] pkg_tree, String ret, OdbcConnection eapConnection)
        {
            // Seek the PkgID for the current pointed package
            String sql = "";

            if (index == 0)
            {
                sql = "select Package_ID from t_package where Name ='" + pkg_tree[index] + "'";
            }
            else
            {
                sql = "select Package_ID from t_package where Name ='" + pkg_tree[index] + "' and Parent_ID = " + ret;
            }

            OdbcCommand eapCommand = eapConnection.CreateCommand();
            eapCommand.CommandText = sql;
            OdbcDataReader eapReader = eapCommand.ExecuteReader();
            eapReader.Read();
            ret = eapReader.GetString(0);

            index++;

            if (index == pkg_tree.Count())
            {
                // end of the recursion
            }
            else
            {
                // needs another step
                ret = acquirePackageID(index, pkg_tree, ret, eapConnection);
            }

            eapReader.Close();
            eapCommand.Dispose();

            return ret;
        }
    }
}
