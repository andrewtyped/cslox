using lox.constants;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace lox.test
{
    [TestClass]
    public class EnvironmentTest
    {
        #region Fields

        private readonly Environment environment;

        #endregion

        #region Constructors

        public EnvironmentTest()
        {
            this.environment = new Environment();
        }

        #endregion

        #region Instance Methods

        [TestMethod]
        public void CanDefineVariable()
        {
            var source = "foo";
            var token = new Token(0,
                                  2,
                                  1,
                                  TokenType.IDENTIFIER);
            this.environment.Define(source,
                                    token,
                                    1d);

            var value = this.environment.Get(source,
                                             token);

            Assert.AreEqual(1d,
                            value);
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void CannotGetUndefinedVariable()
        {
            var source = "foo";
            var token = new Token(0,
                                  2,
                                  1,
                                  TokenType.IDENTIFIER);

            this.environment.Get(source,
                                 token);
        }

        [TestMethod]
        public void CanRedefineVariable()
        {
            var source = "foo foo";
            var token = new Token(0,
                                  2,
                                  1,
                                  TokenType.IDENTIFIER);
            this.environment.Define(source,
                                    token,
                                    1d);

            var token2 = new Token(4,
                                   6,
                                   1,
                                   TokenType.IDENTIFIER);
            this.environment.Define(source,
                                    token2,
                                    2d);

            var value = this.environment.Get(source,
                                             token);

            //tokens with identical lexemes should refer to the same value
            Assert.AreEqual(2d,
                            value);

            var value2 = this.environment.Get(source,
                                              token2);

            Assert.AreEqual(2d,
                            value2);
        }

        #endregion
    }
}