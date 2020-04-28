using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace asp_net_core_mvc_drink_shop.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("test")]
    public class TestTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // output.TagName = "a";
            // output.Attributes.SetAttribute("href", "derp");
            // output.Content.SetContent("I can't see this");
            // base.Process(context, output);
        }
    }
}
