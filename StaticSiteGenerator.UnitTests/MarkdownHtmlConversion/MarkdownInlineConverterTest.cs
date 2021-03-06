using System;
using System.Collections.Generic;
using Xunit;
using StaticSiteGenerator.Markdown.InlineElement;
using StaticSiteGenerator.Utilities.StrategyPattern;
using Test.Markdown.Parser;
using StaticSiteGenerator.MarkdownHtmlConversion.InlineConverterStrategies;
using StaticSiteGenerator.MarkdownHtmlConversion.MarkdownHtmlConverters;
using StaticSiteGenerator.MarkdownHtmlConversion;

namespace Test.MarkdownHtmlConversion
{
    public class MarkdownInlineConverterTest
    {
        private StrategyCollectionMockFactory mockFactory => new StrategyCollectionMockFactory();

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ConverterCallsCorrectStrategyWhenExists(int numInlines)
        {
            TestConverter testConverter = new TestConverter();
            Dictionary<string, IInlineConverterStrategy> strategyMappings = new Dictionary<string, IInlineConverterStrategy>
            {
                { nameof(Text), testConverter }
            };

            Moq.Mock<StrategyCollection<IInlineConverterStrategy>> mock = mockFactory.Get<IInlineConverterStrategy>(strategyMappings);

            var converter = new MarkdownInlineConverter(mock.Object);

            var inline = new Text();

            var inlines = new List<IInlineElement>();

            for (var i = 0; i < numInlines; i++)
            {
                inlines.Add(inline);
            }

            converter.Convert(inlines);

            if(numInlines > 0)
            {
                Assert.True(testConverter.ConverterCalled);
            }
            else
            {
                Assert.False(testConverter.ConverterCalled);
            }
        }

        [Fact]
        public void ConverterThrowsExceptionWhenNoMatchingStrategyExists()
        {
            var converter = new MarkdownInlineConverter(mockFactory.Get(new Dictionary<string, IInlineConverterStrategy>()).Object);

            var block = new Text();

            Assert.Throws<StrategyNotFoundException>(() => { converter.Convert(block); });
        }
        [HtmlConverterFor(nameof(Text))]
        private class TestConverter : IInlineConverterStrategy
        {
            public bool ConverterCalled = false;

            public string Convert(IInlineElement _)
            {
                ConverterCalled = true;
                return String.Empty;
            }
        }
    }
}
