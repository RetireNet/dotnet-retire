# Retire Vulnerability Report

## VulnerableApp (VulnerableApp/VulnerableApp.csproj)

### Microsoft.AspNetCore.SpaServices (v2.1.0)
 * [Microsoft Security Advisory CVE-2019-1302: ASP.NET Core Elevation Of Privilege Vulnerability](https://github.com/aspnet/Announcements/issues/384)
   * **Microsoft.AspNetCore.SpaServices**

### System.IO.Compression.ZipFile (v4.3.0)
 * [Microsoft Security Advisory CVE-2018-8416: .NET Core Tampering Vulnerability](https://github.com/dotnet/announcements/issues/95)
   * Microsoft.AspNetCore.SpaServices
     * Microsoft.AspNetCore.Mvc.ViewFeatures
       * Newtonsoft.Json.Bson
         * NETStandard.Library
           * **System.IO.Compression.ZipFile**

     * Microsoft.AspNetCore.Mvc.TagHelpers
       * Microsoft.AspNetCore.Mvc.Razor
         * Microsoft.AspNetCore.Mvc.ViewFeatures
           * Newtonsoft.Json.Bson
             * NETStandard.Library
               * **System.IO.Compression.ZipFile**

### System.Net.Http (v4.3.0)
 * [Microsoft Security Advisory CVE-2018-8292: .NET Core Information Disclosure Vulnerability](https://github.com/dotnet/announcements/issues/88)
   * Microsoft.AspNetCore.SpaServices
     * Microsoft.AspNetCore.Mvc.ViewFeatures
       * Newtonsoft.Json.Bson
         * NETStandard.Library
           * **System.Net.Http**

     * Microsoft.AspNetCore.Mvc.TagHelpers
       * Microsoft.AspNetCore.Mvc.Razor
         * Microsoft.AspNetCore.Mvc.ViewFeatures
           * Newtonsoft.Json.Bson
             * NETStandard.Library
               * **System.Net.Http**

