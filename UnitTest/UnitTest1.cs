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
    }
}