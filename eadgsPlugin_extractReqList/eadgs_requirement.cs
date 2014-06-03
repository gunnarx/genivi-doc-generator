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

namespace eadgsPlugin_extractReqList
{
    class eadgs_requirement
    {
        public String objectID { get; set; }
        public String req_text { get; set; }
        public String req_name { get; set; }
        public String req_stereo { get; set; }
        public String req_prio { get; set; }
        public String req_ratio { get; set; }
        public String req_alias { get; set; }
    }
}
