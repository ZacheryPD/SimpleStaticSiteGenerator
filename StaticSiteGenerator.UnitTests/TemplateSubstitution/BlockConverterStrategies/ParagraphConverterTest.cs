using System.Collections.Generic;

using Moq;
using Xunit;
using StaticSiteGenerator.Markdown.InlineElement;
using StaticSiteGenerator.Markdown.BlockElement;
using StaticSiteGenerator.TemplateSubstitution;
using StaticSiteGenerator.TemplateSubstitution.TemplateTags;
using StaticSiteGenerator.TemplateSubstitution.BlockConverterStrategies;
using StaticSiteGenerator.TemplateSubstitution.MarkdownHtmlConverters;
using Test.Markdown.Parser;
using StaticSiteGenerator.UnitTests.Doubles;

namespace Test.TemplateSubstitution.BlockConverterStrategies
{

    public class ParagraphConverterTest
    {
        private TemplateCollectionMockFactory templateCollectionMockFactory => new TemplateCollectionMockFactory();
        private StrategyCollectionMockFactory strategyCollectionMockFactory => new StrategyCollectionMockFactory();

        [Fact]
        public void Test()
        {
            var inlineConverterMock = GetInlineConverterMock("TestText");

            Mock<ITemplateTagCollection> templateReader = templateCollectionMockFactory
                .Get(new List<TemplateTag> {
                        new TemplateTag {
                            Template ="<p>{{}}</p>",
                            Type = TagType.Paragraph
                        }
                    });

            var templateFillerMock = TemplateFillerMockFactory.Get();

            var converter = new ParagraphHtmlConverterStrategy(
                inlineConverterMock.Object,
                templateReader.Object,
                templateFillerMock.Object);

            var headerBlock = new Paragraph
            {
                Inlines = new List<IInlineElement>()
            };
            var result = converter.Convert(headerBlock);

            Assert.Equal("<p>TestText</p>", result);
        }

        private static Mock<IMarkdownInlineConverter> GetInlineConverterMock(string resultText)
        {
            var inlineConverterMock = new Mock<IMarkdownInlineConverter>();

            inlineConverterMock
                .Setup(c => c.Convert(It.IsAny<IList<IInlineElement>>()))
                .Returns(resultText);
            return inlineConverterMock;
        }
    }
}
