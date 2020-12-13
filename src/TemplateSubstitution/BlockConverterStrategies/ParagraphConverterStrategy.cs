using System;

using StaticSiteGenerator.TemplateSubstitution;
using StaticSiteGenerator.Markdown.BlockElement;
using StaticSiteGenerator.TemplateSubstitution.TemplateTags;

using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace StaticSiteGenerator.TemplateSubstitution.BlockConverterStrategies
{
    [HtmlConverterFor(nameof(Paragraph))]
    public class ParagraphConverterStrategy: IBlockHtmlConverterStrategy
    {
        private MarkdownInlineConverter InlineConverter;
        private TemplateReader TemplateReader;

        public ParagraphConverterStrategy(MarkdownInlineConverter inlineConverter, TemplateReader reader)
        {
            InlineConverter = inlineConverter;
            TemplateReader = reader;
        }
        public string Convert(IBlockElement block)
        {
            var b = (Paragraph) block;
            var inlineText = InlineConverter.Convert(b.Inlines);

            var template = TemplateReader.GetTemplateTagForType(TagType.Paragraph);

            return template.ToHtml(inlineText);
        }
    }
}