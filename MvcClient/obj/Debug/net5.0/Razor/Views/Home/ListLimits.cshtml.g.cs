#pragma checksum "C:\Users\seantal\source\repos\LoginOAuth\IdentityServerAuth\MvcClient\Views\Home\ListLimits.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "56f1497592061dfbc72135b1f994960b12989d93"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_ListLimits), @"mvc.1.0.view", @"/Views/Home/ListLimits.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"56f1497592061dfbc72135b1f994960b12989d93", @"/Views/Home/ListLimits.cshtml")]
    public class Views_Home_ListLimits : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<MvcClient.Contracts.ResponseObjects.LimitResponse>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\seantal\source\repos\LoginOAuth\IdentityServerAuth\MvcClient\Views\Home\ListLimits.cshtml"
  
    ViewBag.Title = "title";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h2>List Limits View</h2>


<table>

    <tr>
        <td>
            BrokerId
        </td>
        <td>
            LimitId
        </td>
        <td>
            Value
        </td>
        <td>
            CoverType
        </td>
    </tr>

");
#nullable restore
#line 27 "C:\Users\seantal\source\repos\LoginOAuth\IdentityServerAuth\MvcClient\Views\Home\ListLimits.cshtml"
      
        foreach (var limit in @Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <tr>\r\n                <td>\r\n                    ");
#nullable restore
#line 32 "C:\Users\seantal\source\repos\LoginOAuth\IdentityServerAuth\MvcClient\Views\Home\ListLimits.cshtml"
               Write(limit.BrokerId);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 35 "C:\Users\seantal\source\repos\LoginOAuth\IdentityServerAuth\MvcClient\Views\Home\ListLimits.cshtml"
               Write(limit.LimitId);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 38 "C:\Users\seantal\source\repos\LoginOAuth\IdentityServerAuth\MvcClient\Views\Home\ListLimits.cshtml"
               Write(limit.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n                <td>\r\n                    ");
#nullable restore
#line 41 "C:\Users\seantal\source\repos\LoginOAuth\IdentityServerAuth\MvcClient\Views\Home\ListLimits.cshtml"
               Write(limit.CoverType);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </td>\r\n            </tr>\r\n");
#nullable restore
#line 44 "C:\Users\seantal\source\repos\LoginOAuth\IdentityServerAuth\MvcClient\Views\Home\ListLimits.cshtml"
        }
    

#line default
#line hidden
#nullable disable
            WriteLiteral("</table>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<MvcClient.Contracts.ResponseObjects.LimitResponse>> Html { get; private set; }
    }
}
#pragma warning restore 1591