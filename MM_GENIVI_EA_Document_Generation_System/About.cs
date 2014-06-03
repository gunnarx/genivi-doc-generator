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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MM_GENIVI_EA_Document_Generation_System
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
