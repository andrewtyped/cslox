namespace lox.constants
{
    /// <summary>
    /// Defines all the possible syntax units in a Lox program.
    /// </summary>
    public enum TokenType
    {
        #region Single Character Tokens

        /// <summary>
        /// (
        /// </summary>
        LEFT_PAREN,

        /// <summary>
        /// )
        /// </summary>
        RIGHT_PAREN,

        /// <summary>
        /// {
        /// </summary>
        LEFT_BRACE,

        /// <summary>
        /// }
        /// </summary>
        RIGHT_BRACE,

        /// <summary>
        /// ,
        /// </summary>
        COMMA,

        /// <summary>
        /// .
        /// </summary>
        DOT,

        /// <summary>
        /// -
        /// </summary>
        MINUS,

        /// <summary>
        /// +
        /// </summary>
        PLUS,

        /// <summary>
        /// ;
        /// </summary>
        SEMICOLON,

        /// <summary>
        /// /
        /// </summary>
        SLASH,

        /// <summary>
        /// *
        /// </summary>
        STAR,

        #endregion

        #region One or Two Character Tokens

        /// <summary>
        /// !
        /// </summary>
        BANG,

        /// <summary>
        /// !=
        /// </summary>
        BANG_EQUAL,

        /// <summary>
        /// =
        /// </summary>
        EQUAL,

        /// <summary>
        /// ==
        /// </summary>
        EQUAL_EQUAL,

        /// <summary>
        /// >
        /// </summary>
        GREATER,

        /// <summary>
        /// >=
        /// </summary>
        GREATER_EQUAL,

        /// <summary>
        /// <
        /// </summary>
        LESS,

        /// <summary>
        /// <=
        /// </summary>
        LESS_EQUAL,

        #endregion

        #region Literals

        /// <summary>
        /// Any user-specified name for a variable, class, function, etc.
        /// </summary>
        IDENTIFIER,

        /// <summary>
        /// The characters between an opening " and closing ".
        /// </summary>
        STRING,

        /// <summary>
        /// 0-9+
        /// </summary>
        NUMBER,

        #endregion

        #region Keywords

        AND,

        CLASS,

        ELSE,

        FALSE,

        /// <summary>
        /// fun = function
        /// </summary>
        FUN,

        FOR,

        IF,

        /// <summary>
        /// nil = null
        /// </summary>
        NIL,

        OR,

        PRINT,

        RETURN,

        SUPER,

        THIS,

        TRUE,

        VAR,

        WHILE,

        #endregion

        #region End of File

        /// <summary>
        /// End of file
        /// </summary>
        EOF

        #endregion
    }
}