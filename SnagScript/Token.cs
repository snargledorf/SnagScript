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


namespace SnagScript
{

    /**
     * A <a href="http://en.wikipedia.org/wiki/Lexical_analysis#Token">token</a>
     * is a categorized block of text that represents an atomic element in the source code.
     *
     * @author <a href="mailto:grom@zeminvaders.net">Cameron Zemek</a>
     */
    public class Token
    {
        private SourcePosition position;
        private TokenType type;
        private String text;

        public Token(SourcePosition position, TokenType type, String text)
        {
            this.position = position;
            this.type = type;
            this.text = text;
        }

        public SourcePosition Position
        {
            get { return position; }
        }

        public TokenType Type
        {
            get { return type; }
        }

        public String Text
        {
            get { return text; }
        }

        public override bool Equals(Object obj)
        {
            if (this == obj)
                return true;
            if (!(obj is Token))
                return false;
            Token other = (Token)obj;
            return this.type == other.type && this.text.Equals(other.text) && this.position.Equals(other.position);
        }
        public override int GetHashCode()
        {
            return 1;
        }

        public override String ToString()
        {
            return type + ",'" + text + "'";
        }
    }
}