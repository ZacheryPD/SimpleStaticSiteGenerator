using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;

using StaticSiteGenerator.Markdown.BlockElement;
using StaticSiteGenerator.Markdown.BlockElementConverter;

using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace StaticSiteGenerator.Markdown.Parser.BlockParser
{

    [TransientService]
    public class MarkdownBlockParser: IMarkdownBlockParser
    {
        IEnumerable<IBlockElementConverter> Converters;

        public MarkdownBlockParser(IEnumerable<IBlockElementConverter> converters)
        {
            Converters = converters;
        }

        public IList<IBlockElement> Parse(IList<MarkdownBlock> inputBlocks)
        {
            var list = new List<IBlockElement>();
            foreach(var block in inputBlocks)
            {
                list.Add(Parse(block));
            }

            return list;
        }

        public IBlockElement Parse(MarkdownBlock block)
        {
            var converter = GetElementConverterFor(block.GetType());

            return converter.Convert(block);
        }

        private IBlockElementConverter GetElementConverterFor(Type type)
        {
            foreach(var converter in Converters)
            {
                if(ConverterHasMatchingAttributeType(converter, type)){
                    return converter;
                }
            }

            // TODO: should probably thow a custom exception type
            throw new Exception($"Converter for type {type.Name} not found");
        }

        private bool ConverterHasMatchingAttributeType(IBlockElementConverter converter, Type type)
        {
            var converterType = converter.GetType();

            var attribute = (MarkdownConverterForAttribute) Attribute.GetCustomAttribute(converterType, typeof(MarkdownConverterForAttribute));

            return attribute?.TypeName == type.Name;
        }
    }
}