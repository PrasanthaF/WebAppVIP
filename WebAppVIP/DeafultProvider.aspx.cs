using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Profile;

namespace WebAppVIP
{
    public partial class DeafultProvider : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProviderNameLabel.Text = ProfileManager.Provider.Name;
            ProviderTypeLabel.Text = ProfileManager.Provider.GetType().ToString();
            ProviderDescriptionLabel.Text = ProfileManager.Provider.Description;
        }

    }
}