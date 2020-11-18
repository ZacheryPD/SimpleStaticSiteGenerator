using System;

using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using StaticSiteGenerator.Markdown.BlockElement;

using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace StaticSiteGenerator.Markdown.BlockElementConverter
{
    [TransientService]
    [MarkdownConverterFor(nameof(HeaderBlock))]
    public class HeaderConverter: IBlockElementConverter
    {
        public IBlockElement Convert(MarkdownBlock block)
        {
            HeaderBlock b = (HeaderBlock)block;
            return new Header
            {
                Level = (byte)b.HeaderLevel,
                // TODO: HeaderBlocks actually have inlines and we need to process them rather than just ToString-ing
                Text = b.ToString()
            };
        }
    }
}
