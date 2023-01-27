﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace CheckTestOutput.Example
{
    public class BinaryTest
    {
        private const string ImagePath = "Image.png";

        OutputChecker check = new OutputChecker("testoutputs");

        [Fact]
        public void CheckStagedBinaryData()
        {
            check.CheckBinary(File.ReadAllBytes(Path.Combine(check.CheckDirectory, ImagePath)));
            Assert.True(true);
        }

        [Fact]
        public void CheckModifiedBinaryData()
        {
            var imgPath = Path.Combine(check.CheckDirectory, ImagePath);
            byte[] previousData = File.ReadAllBytes(imgPath);
            byte[] modifiedData = previousData.Append((byte)64).ToArray();

            // File contents do not match
            Assert.Throws<Exception>(() => check.CheckBinary(modifiedData));

            // Return back the original content
            File.WriteAllBytes(Path.Combine(check.CheckDirectory, $"{nameof(BinaryTest)}.{nameof(CheckModifiedBinaryData)}.bin"), previousData);
        }

        [Fact]
        public void CheckNonexistentBinaryData()
        {
            var imageBytes = File.ReadAllBytes(Path.Combine(check.CheckDirectory, ImagePath));
            // File does not exist
            Assert.Throws<ArgumentNullException>(() => check.CheckBinary(imageBytes));
        }
        
        [Fact]
        public void CheckUTF8TextConvertedToBinaryData()
        {
            const string TEXT = "( ͡° ͜ʖ ͡°)";

            check.CheckBinary(Encoding.UTF8.GetBytes(TEXT));
            Assert.True(true);
        }   
        
        [Fact]
        public void CheckUTF8TextConvertedWithASCIIBinaryDataThrows()
        {
            const string TEXT = "( ͡° ͜ʖ ͡°)";

            Assert.Throws<Exception>(() => check.CheckBinary(Encoding.ASCII.GetBytes(TEXT)));
            
            // Return back the original content
            File.WriteAllBytes(Path.Combine(check.CheckDirectory, $"{nameof(BinaryTest)}.{nameof(CheckUTF8TextConvertedWithASCIIBinaryDataThrows)}.bin"), Encoding.UTF8.GetBytes(TEXT));
        }
    }
}
