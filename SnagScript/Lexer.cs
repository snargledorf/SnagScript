/*
 * Copyright (c) 2008 Cameron Zemek
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to
 * deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 */
using System;
using System.IO;
using System.Text;


namespace SnagScript
{

    /**
     * The <a href="http://en.wikipedia.org/wiki/Lexical_analysis#Scanner">lexer</a>
     * is used to read characters and identify tokens and pass them to the parser
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class Lexer
    {
        private const char END_OF_FILE = '\0';

        private int lineNo = 1;
        private int columnNo = 1;
        private PeekReader inReader;

        public Lexer(Stream inStream)
        {
            this.inReader = new PeekReader(inStream, 2);
        }

        private char LookAhead(int i)
        {
            return (char)inReader.Peek(i);
        }

        private int Read()
        {
            try
            {
                int c = inReader.Read(); ;
                if (c == '\n')
                {
                    lineNo++;
                    columnNo = 0;
                }
                columnNo++;
                return c;
            }
            catch (IOException e)
            {
                throw new LexerException(e.Message, lineNo, columnNo);
            }
        }

        private void Close()
        {
            try
            {
                inReader.Close();
            }
            catch (IOException)
            {
            }
        }

        private char Next()
        {
            Read();
            return LookAhead(1);
        }

        private char Match(char c)
        {
            int input = Read();
            if (input != c)
            {
                String inputChar = (input != END_OF_FILE) ? "" + (char)input : "END_OF_FILE";
                throw new LexerException("Expected '" + c + "' but got '" + inputChar + "'", lineNo, columnNo);
            }
            return c;
        }

        private String Match(String str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                Match(str[i]);
            }
            return str;
        }

        private Token CreateToken(TokenType type, char c)
        {
            SourcePosition pos = new SourcePosition(lineNo, columnNo);
            Match(c);
            return new Token(pos, type, "" + c);
        }

        private Token CreateToken(TokenType type, String str)
        {
            SourcePosition pos = new SourcePosition(lineNo, columnNo);
            Match(str);
            return new Token(pos, type, str);
        }

        public Token GetNextToken()
        {
            char character = LookAhead(1);
            // Skip whitespace
            while (character == ' ' || character == '\t' ||
                    character == '\r' || character == '\n')
            {
                character = Next();
            }
            switch (character)
            {
                case END_OF_FILE:
                    {
                        // End of character stream.
                        // Return null to indicate end of token stream
                        Close();
                        return null;
                    }
                case ';':
                    {
                        return CreateToken(TokenType.END_STATEMENT, ";");
                    }
                case '+':
                    {
                        return CreateToken(TokenType.PLUS, '+');
                    }
                case '-':
                    {
                        return CreateToken(TokenType.MINUS, '-');
                    }
                case '*':
                    {
                        return CreateToken(TokenType.MULTIPLY, '*');
                    }
                case '/':
                    {
                        int char2 = LookAhead(2);
                        if (char2 == '/')
                        {
                            return MatchLineComment();
                        }
                        else if (char2 == '*')
                        {
                            return MatchBlockComment();
                        }
                        else
                        {
                            return CreateToken(TokenType.DIVIDE, '/');
                        }
                    }
                case '%':
                    {
                        return CreateToken(TokenType.MOD, '%');
                    }
                case '^':
                    {
                        return CreateToken(TokenType.POWER, '^');
                    }
                case ',':
                    {
                        return CreateToken(TokenType.COMMA, ',');
                    }
                case ':':
                    {
                        return CreateToken(TokenType.COLON, ':');
                    }
                case '(':
                    {
                        return CreateToken(TokenType.LPAREN, '(');
                    }
                case ')':
                    {
                        return CreateToken(TokenType.RPAREN, ')');
                    }
                case '{':
                    {
                        return CreateToken(TokenType.LBRACE, '{');
                    }
                case '}':
                    {
                        return CreateToken(TokenType.RBRACE, '}');
                    }
                case '[':
                    {
                        return CreateToken(TokenType.LBRACKET, '[');
                    }
                case ']':
                    {
                        return CreateToken(TokenType.RBRACKET, ']');
                    }
                case '=':
                    {
                        if (LookAhead(2) == '=')
                        {
                            return CreateToken(TokenType.EQUAL, "==");
                        }
                        else
                        {
                            return CreateToken(TokenType.ASSIGN, '=');
                        }
                    }
                case '|':
                    {
                        return CreateToken(TokenType.OR, "||");
                    }
                case '&':
                    {
                        return CreateToken(TokenType.AND, "&&");
                    }
                case '!':
                    {
                        if (LookAhead(2) == '=')
                        {
                            return CreateToken(TokenType.NOT_EQUAL, "!=");
                        }
                        else
                        {
                            return CreateToken(TokenType.NOT, '!');
                        }
                    }
                case '<':
                    {
                        if (LookAhead(2) == '=')
                        {
                            return CreateToken(TokenType.LESS_EQUAL, "<=");
                        }
                        else
                        {
                            return CreateToken(TokenType.LESS_THEN, '<');
                        }
                    }
                case '>':
                    {
                        if (LookAhead(2) == '=')
                        {
                            return CreateToken(TokenType.GREATER_EQUAL, ">=");
                        }
                        else
                        {
                            return CreateToken(TokenType.GREATER_THEN, '>');
                        }
                    }
                case '\'':
                case '"':
                    {
                        return MatchStringLiteral((char)character);
                    }
                case '.':
                    {
                        return CreateToken(TokenType.PERIOD, '.');
                    }
                default:
                    {
                        if (character >= '0' && character <= '9')
                        {
                            return MatchNumber();
                        }
                        else if ((character >= 'A' && character <= 'Z') ||
                          (character >= 'a' && character <= 'z') ||
                          character == '_')
                        {
                            return MatchIdentifier();
                        }
                        else
                        {
                            throw new LexerException("Unexpected '" + ((char)character) + "' character", lineNo, columnNo);
                        }
                    }
            }
        }

        private Token MatchLineComment()
        {
            SourcePosition pos = new SourcePosition(lineNo, columnNo);
            Match("//");
            StringBuilder sb = new StringBuilder();
            int character = LookAhead(1);
            while (character != '\r' && character != '\n' && character != END_OF_FILE)
            {
                sb.Append((char)character);
                character = Next();
            }
            return new Token(pos, TokenType.COMMENT, sb.ToString());
        }

        private Token MatchBlockComment()
        {
            SourcePosition pos = new SourcePosition(lineNo, columnNo);
            Match("/*");
            StringBuilder sb = new StringBuilder();
            int character = LookAhead(1);
            while (true)
            {
                if (character == END_OF_FILE)
                {
                    throw new LexerException("Expecting */ but found end of file", lineNo, columnNo);
                }
                if (LookAhead(1) == '*' && LookAhead(2) == '/')
                {
                    break;
                }
                sb.Append((char)character);
                character = Next();
            }
            Match("*/");
            return new Token(pos, TokenType.COMMENT, sb.ToString());
        }

        private Token MatchNumber()
        {
            SourcePosition pos = new SourcePosition(lineNo, columnNo);
            StringBuilder sb = new StringBuilder();
            bool isDecimal = false;
            int character = LookAhead(1);
            while ((character >= '0' && character <= '9') || character == '.')
            {
                if (isDecimal && character == '.')
                {
                    throw new LexerException("Unexcepted '.' character", lineNo, columnNo);
                }
                else if (character == '.')
                {
                    isDecimal = true;
                }
                sb.Append((char)character);
                character = Next();
            }
            if (isDecimal)
            {
                return new Token(pos, TokenType.DECIMAL, sb.ToString());
            }
            else
            {
                return new Token(pos, TokenType.INTEGER, sb.ToString());
            }
        }

        /**
         * An identifier is either a keyword, function, or variable
         *
         * @return Token
         */
        private Token MatchIdentifier()
        {
            SourcePosition pos = new SourcePosition(lineNo, columnNo);
            StringBuilder sb = new StringBuilder();
            char character = LookAhead(1);
            while ((character >= 'a' && character <= 'z') ||
                    (character >= 'A' && character <= 'Z') ||
                    (character >= '0' && character <= '9') ||
                    character == '_')
            {
                sb.Append((char)character);
                character = Next();
            }
            String word = sb.ToString();
            if (word.Equals("true"))
            {
                return new Token(pos, TokenType.TRUE, word);
            }
            else if (word.Equals("false"))
            {
                return new Token(pos, TokenType.FALSE, word);
            }
            else if (word.Equals("if"))
            {
                return new Token(pos, TokenType.IF, word);
            }
            else if (word.Equals("else"))
            {
                return new Token(pos, TokenType.ELSE, word);
            }
            else if (word.Equals("while"))
            {
                return new Token(pos, TokenType.WHILE, word);
            }
            else if (word.Equals("foreach"))
            {
                return new Token(pos, TokenType.FOR_EACH, word);
            }
            else if (word.Equals("as"))
            {
                return new Token(pos, TokenType.AS, word);
            }
            else if (word.Equals("function"))
            {
                return new Token(pos, TokenType.FUNCTION, word);
            }
            else if (word.Equals("var"))
            {
                return new Token(pos, TokenType.VAR, word);
            }
            else if (word.Equals("return"))
            {
                return new Token(pos, TokenType.RETURN, word);
            }
            else if (word.Equals("new"))
            {
                return new Token(pos, TokenType.NEW, word);
            }
            else if (word.Equals("this"))
            {
                return new Token(pos, TokenType.THIS, word);
            }
            //else if (word.Equals("prototype")) 
            //{
            //    return new Token(pos, TokenType.PROTOTYPE, word);                
            //}
            else if (word.Equals("try"))
            {
                return new Token(pos, TokenType.TRY, word);
            }
            else if (word.Equals("catch"))
            {
                return new Token(pos, TokenType.CATCH, word);
            }
            else if (word.Equals("finally"))
            {
                return new Token(pos, TokenType.FINALLY, word);
            }
            else
            {
                return new Token(pos, TokenType.VARIABLE, word);
            }
        }

        private Token MatchStringLiteral(char quote)
        {
            SourcePosition pos = new SourcePosition(lineNo, columnNo);
            Match(quote);
            StringBuilder sb = new StringBuilder();
            int character = LookAhead(1);
            while (character != quote && character != END_OF_FILE)
            {
                sb.Append((char)character);
                character = Next();
            }
            Match(quote);
            return new Token(pos, TokenType.STRING_LITERAL, sb.ToString());
        }
    }
}