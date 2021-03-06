using System;
using Xunit;

using StaticSiteGenerator.FileManipulation;
using StaticSiteGenerator.FileManipulation.FileException;

using StaticSiteGenerator.UnitTests.Helpers;

namespace StaticSiteGenerator.UnitTests.FileManipulation
{
    public class FileReaderTest
    {
        [Fact]
        public void FileDoesNotExist()
        {
            var FileReader = new FileReader();

            var filePath = "NonExistantFileName.txt";

            Assert.Throws<FileManipulationException>(() => { FileReader.ReadFile(filePath); });
        }

        [Fact]
        public void FileExistsButIsEmpty()
        {
            using(var file = TempFileHelper.GetTempTextFile())
            {
                var fileReader = new FileReader();

                var fileContents = fileReader.ReadFile(file.Path);

                Assert.Equal("", fileContents);
            }
        }

        [Fact]
        public void FileExists()
        {
            using(var file = TempFileHelper.GetTempTextFile())
            {
                var contents = "Test File Contents";
                file.WriteToFile(contents);

                var fileReader = new FileReader();

                var fileContents = fileReader.ReadFile(file.Path);


                Assert.Contains(contents, fileContents);
            }
        }
    }
}
