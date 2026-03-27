using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RadarPNCP.Util
{
    public class Web
    {
        public static void AbrirLink(string url)
        {
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };

            Process.Start(psi);
        }
    }
}
