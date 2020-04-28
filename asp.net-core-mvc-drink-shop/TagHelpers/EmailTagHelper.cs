using Microsoft.AspNetCore.Razor.TagHelpers;

namespace asp_net_core_mvc_drink_shop.TagHelpers
{

    public class EmailTagHelper: TagHelper
    {
        public string Address { get; set; }

        public string Content { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", "mailto:" + Address);
            output.Content.SetContent(Content);
        }

    }
}
