// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System.Windows.Forms;
using Utils.Language;

namespace Forms.EditorControls
{
    public partial class MaterialAddOption : UserControl
    {
        public MaterialAddOption()
        {
            InitializeComponent();
            Localise();
        }

        private void Localise()
        {
            groupGeneral.Text = Language.GetString("$GENERAL");
            MafiaIIBrowser.Description = Language.GetString("$SELECT_MII_FOLDER");
        }
    }
}
