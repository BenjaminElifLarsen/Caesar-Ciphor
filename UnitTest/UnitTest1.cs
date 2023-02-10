using H5Encryption1;
using Test;

namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var m = "successful unit test";
            var f = Caesar.FormatMessage(m);
            var e = Caesar.Encrypt(f, 5);
            Assert.Equal("XZHHJXXKZQ ZSNY YJXY", e);
        }
        [Fact]
        public void Test2()
        {
            var m = "finally done!'";
            var f = Caesar.FormatMessage(m);
            var e = Caesar.Encrypt(f, 13);
            Assert.Equal("SVANYYL QBAR", e);
        }
        [Fact]
        public void Test3()
        {
            var d = Caesar.Decrypt("XZHHJXXKZQ ZSNY YJXY", 5);
            Assert.Equal("SUCCESSFUL UNIT TEST", d);
        }
        [Fact]
        public void Test4()
        {
            var d = Caesar.Decrypt("SVANYYL QBAR", 13);
            Assert.Equal("FINALLY DONE", d);
        }

        [Fact]
        public void VigenereTest1()
        {
            var m = "a";
            var k = "b";
            var e = Vigenere.Encrypt(m, k);
            Assert.Equal("B", e);
        }

        [Fact]
        public void VigenereTest2()
        {
            var m = "b";
            var k = "b";
            var d = Vigenere.Decrypt(m, k);
            Assert.Equal("A", d);
        }

        [Fact]
        public void VigenereTest3()
        {
            var m = "zz";
            var k = "za";
            var e = Vigenere.Encrypt(m, k);
            Assert.Equal("YZ", e);
        }

        [Fact]
        public void VigenereTest4()
        {
            var m = "yz";
            var k = "za";
            var d = Vigenere.Decrypt(m, k);
            Assert.Equal("ZZ", d);
        }

        [Fact]
        public void VigenereTest5()
        {
            var m = "this is a test";
            var k = "za";
            var e = Vigenere.Encrypt(m, k);
            Assert.Equal("SHHS HS Z TDSS", e);
        }

        [Fact]
        public void VigenereTest6()
        {
            var m = "SHHS HS Z TDSS";
            var k = "za";
            var d = Vigenere.Decrypt(m, k);
            Assert.Equal("THIS IS A TEST", d);
        }
    }
}