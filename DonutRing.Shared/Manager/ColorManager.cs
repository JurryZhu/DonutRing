namespace DonutRing.Shared.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public partial class ColorManager
    {
        #region Public Methods

        public static async Task<Tuple<string, string>> GetColorTupleAsync()
        {
            var result = new Tuple<string, string>(Settings.DEFAULT_COLOR_NAME, Settings.DEFAULT_COLOR_HEX);
            try
            {
                var httpClient = new HttpClient();
                var @string = await httpClient.GetStringAsync(string.Format(Settings.COLOR_LOVERS_URL, DateTime.Now.ToString()));
                var xelement = XElement.Parse(@string, LoadOptions.None);
                var titleElement = xelement.Descendants("title").FirstOrDefault();
                var hexElement = xelement.Descendants("hex").FirstOrDefault();
                if (titleElement != null && hexElement != null)
                {
                    result = new Tuple<string, string>(titleElement.Value, string.Format("#{0}", hexElement.Value));
                }
            }
            catch(Exception ex)
            {
                LogManager.Log(ex);
            }

            return result;
        }

        #endregion
    }
}
