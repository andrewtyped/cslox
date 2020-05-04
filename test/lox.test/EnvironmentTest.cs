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

        [TestMethod]
        public void CanAssignVariable()
        {
            var source = "foo";
            var token = new Token(0,
                                  2,
                                  1,
                                  TokenType.IDENTIFIER);
            this.environment.Define(source,
                                    token,
                                    null);

            this.environment.Assign(source,
                                    token,
                                    "bar");

            var value = this.environment.Get(source,
                                             token);

            Assert.AreEqual("bar",
                            value);
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void CannotAssignUndefinedVariable()
        {
            var source = "foo";
            var token = new Token(0,
                                  2,
                                  1,
                                  TokenType.IDENTIFIER);
           
            this.environment.Assign(source,
                                    token,
                                    "bar");

            var value = this.environment.Get(source,
                                             token);

            Assert.AreEqual("bar",
                            value);
        }

        [TestMethod]
        public void CanAccessVariablesInEnclosedScopes()
        {
            var source = "foo";
            var token = new Token(0,
                                  2,
                                  1,
                                  TokenType.IDENTIFIER);
            var parentEnvironment = new Environment();
            parentEnvironment.Define(source,
                                     token,
                                     1d);

            var childEnvironment = new Environment(parentEnvironment);

            var value = childEnvironment.Get(source,
                                             token);

            Assert.AreEqual(1d,
                            value);
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeError))]
        public void CannotAccessVariablesDefinedInChildScopeFromParentScope()
        {
            var source = "foo";
            var token = new Token(0,
                                  2,
                                  1,
                                  TokenType.IDENTIFIER);
            var parentEnvironment = new Environment();

            var childEnvironment = new Environment(parentEnvironment);

            childEnvironment.Define(source,
                                    token,
                                    1d);

            parentEnvironment.Get(source,
                                  token);
        }

        [TestMethod]
        public void VariablesDefinedInChildScopeCanShadowVariablesDefinedInParentScope()
        {
            var source = "foo";
            var token = new Token(0,
                                  2,
                                  1,
                                  TokenType.IDENTIFIER);
            var parentEnvironment = new Environment();

            parentEnvironment.Define(source,
                                     token,
                                     1d);

            var childEnvironment = new Environment(parentEnvironment);

            childEnvironment.Define(source,
                                    token,
                                    2d);

            var value = parentEnvironment.Get(source,
                                              token);

            Assert.AreEqual(1d,
                            value);

            value = childEnvironment.Get(source,
                                         token);

            Assert.AreEqual(2d,
                            value);
        }

        [TestMethod]
        public void CanAssignVariableDefinedInParentScopeFromChildScope()
        {
            var source = "foo";
            var token = new Token(0,
                                  2,
                                  1,
                                  TokenType.IDENTIFIER);
            var parentEnvironment = new Environment();

            parentEnvironment.Define(source,
                                     token,
                                     null);

            var childEnvironment = new Environment(parentEnvironment);

            childEnvironment.Assign(source,
                                    token,
                                    2d);

            var value = childEnvironment.Get(source,
                                             token);
            Assert.AreEqual(2d,
                            value);
        }

        [TestMethod]
        public void AssignmentsToShadowedVariablesDoNotAffectParentScope()
        {
            var source = "foo";
            var token = new Token(0,
                                  2,
                                  1,
                                  TokenType.IDENTIFIER);
            var parentEnvironment = new Environment();

            parentEnvironment.Define(source,
                                     token,
                                     1d);

            var childEnvironment = new Environment(parentEnvironment);

            childEnvironment.Define(source,
                                    token,
                                    2d);

            childEnvironment.Assign(source,
                                    token,
                                    3d);

            var value = parentEnvironment.Get(source,
                                              token);

            Assert.AreEqual(1d,
                            value);

            value = childEnvironment.Get(source,
                                         token);

            Assert.AreEqual(3d,
                            value);
        }
        #endregion
    }
}